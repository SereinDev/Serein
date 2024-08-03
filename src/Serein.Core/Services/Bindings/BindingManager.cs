using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Extensions.DependencyInjection;

using Serein.Core.Models.Bindings;
using Serein.Core.Services.Data;

namespace Serein.Core.Services.Bindings;

public class BindingManager(SettingProvider settingProvider, IServiceProvider services)
{
    private readonly SettingProvider _settingProvider = settingProvider;
    private readonly IServiceProvider _services = services;
    private readonly object _lock = new();

    private BindingRecordDbContext BindingRecordDbContext
    {
        get
        {
            var ctx = _services.GetRequiredService<BindingRecordDbContext>();
            ctx.Database.EnsureCreated();
            return ctx;
        }
    }

    public void ValidateGameId(string gameId)
    {
        ArgumentException.ThrowIfNullOrEmpty(gameId, nameof(gameId));

        var regex = new Regex(_settingProvider.Value.Application.RegexForCheckingGameId);

        if (!regex.IsMatch(gameId))
            throw new BindingFailureException("游戏名称格式不正确");
    }

    public List<BindingRecord> Records => [.. BindingRecordDbContext.Records];

    public bool TryGetValue(long id, [NotNullWhen(true)] out BindingRecord? bindingRecord)
    {
        lock (_lock)
            bindingRecord = BindingRecordDbContext.Records.FirstOrDefault((v) => v.UserId == id);
        return bindingRecord is not null;
    }

    public void CheckConflict(long id, string gameId)
    {
        lock (_lock)
            foreach (var record in BindingRecordDbContext.Records)
                if (record.UserId != id && record.GameIds.Contains(gameId))
                    throw new BindingFailureException("此Id已被占用");
    }

    public void Add(long id, string gameId, string? shownName = null)
    {
        ValidateGameId(gameId);
        CheckConflict(id, gameId);

        if (!TryGetValue(id, out var record))
        {
            record = new()
            {
                UserId = id,
                GameIds = [gameId],
                ShownName = shownName ?? string.Empty,
                Time = DateTime.Now
            };
            BindingRecordDbContext.Records.Add(record);
        }
        else
        {
            if (!string.IsNullOrEmpty(shownName))
                record.ShownName = shownName;

            if (record.GameIds.Contains(gameId))
                throw new BindingFailureException("已经绑定过此Id了");
            else
                record.GameIds.Add(gameId);

            record.Update();
        }

        BindingRecordDbContext.SaveChanges();
    }

    public void Remove(long id, string gameId)
    {
        if (!TryGetValue(id, out var record) || !record.GameIds.Remove(gameId))
            throw new BindingFailureException("未绑定此Id");

        record.Update();

        BindingRecordDbContext.SaveChanges();
    }
}