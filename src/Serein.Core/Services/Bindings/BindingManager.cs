using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Extensions.DependencyInjection;

using Serein.Core.Models.Bindings;
using Serein.Core.Services.Data;

namespace Serein.Core.Services.Bindings;

public sealed class BindingManager(SettingProvider settingProvider, IServiceProvider services)
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
        {
            throw new BindingFailureException("游戏名称格式不正确");
        }
    }

    public List<BindingRecord> Records => [.. BindingRecordDbContext.Records];

    public bool TryGetValue(long id, [NotNullWhen(true)] out BindingRecord? bindingRecord)
    {
        lock (_lock)
        {
            using var context = BindingRecordDbContext;
            return TryGetValue(context, id, out bindingRecord);
        }
    }

    private bool TryGetValue(
        BindingRecordDbContext context,
        long id,
        [NotNullWhen(true)] out BindingRecord? bindingRecord
    )
    {
        lock (_lock)
        {
            bindingRecord = context.Records.FirstOrDefault((v) => v.UserId == id);
        }
        return bindingRecord is not null;
    }

    public void CheckConflict(long id, string gameId)
    {
        lock (_lock)
        {
            using var context = BindingRecordDbContext;
            CheckConflict(context, id, gameId);
        }
    }

    private void CheckConflict(BindingRecordDbContext context, long id, string gameId)
    {
        lock (_lock)
        {
            foreach (var record in context.Records)
            {
                if (record.UserId != id && record.GameIds.Contains(gameId))
                {
                    throw new BindingFailureException("此Id已被占用");
                }
            }
        }
    }

    public void Add(long id, string gameId, string? shownName = null)
    {
        ValidateGameId(gameId);

        lock (_lock)
        {
            using var context = BindingRecordDbContext;

            CheckConflict(context, id, gameId);
            if (!TryGetValue(context, id, out var record))
            {
                record = new()
                {
                    UserId = id,
                    GameIds = [gameId],
                    ShownName = shownName ?? string.Empty,
                    Time = DateTime.Now,
                };
                context.Records.Add(record);
            }
            else
            {
                if (!string.IsNullOrEmpty(shownName))
                {
                    record.ShownName = shownName;
                }

                if (record.GameIds.Contains(gameId))
                {
                    throw new BindingFailureException("已经绑定过此Id了");
                }
                else
                {
                    record.GameIds.Add(gameId);
                }
                record.Update();
            }

            context.SaveChanges();
        }
    }

    public void Remove(long id, string gameId)
    {
        lock (_lock)
        {
            using var context = BindingRecordDbContext;
            if (!TryGetValue(context, id, out var record) || !record.GameIds.Remove(gameId))
            {
                throw new BindingFailureException("未绑定此Id");
            }
            record.Update();

            context.SaveChanges();
        }
    }
}
