using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Serein.Core.Models.Bindings;
using Serein.Core.Services.Data;

namespace Serein.Core.Services.Bindings;

public sealed class BindingManager(SettingProvider settingProvider, IServiceProvider services)
{
    private readonly object _lock = new();

    private BindingRecordDbContext BindingRecordDbContext
    {
        get
        {
            var ctx = services.GetRequiredService<BindingRecordDbContext>();
            ctx.Database.EnsureCreated();

            try
            {
                var databaseCreator =
                    ctx.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                databaseCreator?.CreateTables();
            }
            catch (SqliteException)
            {
                // A SqlException will be thrown if tables already exist. So simply ignore it.
            }

            return ctx;
        }
    }

    public void ValidateGameId(string gameId)
    {
        ArgumentException.ThrowIfNullOrEmpty(gameId, nameof(gameId));

        var regex = new Regex(settingProvider.Value.Application.RegexForCheckingGameId);

        if (!regex.IsMatch(gameId))
        {
            throw new BindingFailureException("游戏名称格式不正确");
        }
    }

    public IReadOnlyList<BindingRecord> Records => [.. BindingRecordDbContext.Datas];

    public bool TryGetValue(string id, [NotNullWhen(true)] out BindingRecord? bindingRecord)
    {
        lock (_lock)
        {
            using var context = BindingRecordDbContext;
            return TryGetValue(context, id, out bindingRecord);
        }
    }

    private bool TryGetValue(
        BindingRecordDbContext context,
        string id,
        [NotNullWhen(true)] out BindingRecord? bindingRecord
    )
    {
        lock (_lock)
        {
            bindingRecord = context.Datas.FirstOrDefault((v) => v.UserId == id);
        }
        return bindingRecord is not null;
    }

    public void CheckConflict(string userId, string gameId)
    {
        lock (_lock)
        {
            using var context = BindingRecordDbContext;
            CheckConflict(context, userId, gameId);
        }
    }

    private void CheckConflict(BindingRecordDbContext context, string id, string gameId)
    {
        lock (_lock)
        {
            foreach (var record in context.Datas)
            {
                if (
                    record.UserId != id
                    && record.GameIds.Contains(gameId, StringComparer.InvariantCultureIgnoreCase)
                )
                {
                    throw new BindingFailureException("此Id已被占用");
                }
            }
        }
    }

    public void Add(BindingRecord bindingRecord)
    {
        ArgumentNullException.ThrowIfNull(bindingRecord, nameof(bindingRecord));
        ArgumentException.ThrowIfNullOrEmpty(bindingRecord.UserId, nameof(bindingRecord.UserId));

        lock (_lock)
        {
            using var context = BindingRecordDbContext;

            foreach (var gameId in bindingRecord.GameIds)
            {
                ValidateGameId(gameId);
                CheckConflict(context, bindingRecord.UserId, gameId);
            }

            if (!TryGetValue(context, bindingRecord.UserId, out var record))
            {
                context.Datas.Add(bindingRecord);
            }
            else
            {
                if (!string.IsNullOrEmpty(bindingRecord.ShownName))
                {
                    record.ShownName = bindingRecord.ShownName;
                }

                foreach (var gameId in bindingRecord.GameIds)
                {
                    if (record.GameIds.Contains(gameId, StringComparer.InvariantCultureIgnoreCase))
                    {
                        throw new BindingFailureException($"已经绑定过此Id了（Id={gameId}）");
                    }
                    else
                    {
                        record.GameIds.Add(gameId);
                    }
                }

                record.Update();
            }

            context.SaveChanges();
        }
    }

    public void Add(string userId, string gameId, string? shownName = null)
    {
        Add(
            new()
            {
                UserId = userId,
                GameIds = [gameId],
                ShownName = shownName ?? string.Empty,
            }
        );
    }

    public void Remove(string userId, string gameId)
    {
        lock (_lock)
        {
            using var context = BindingRecordDbContext;
            if (!TryGetValue(context, userId, out var record) || !record.GameIds.Remove(gameId))
            {
                throw new BindingFailureException("未绑定此Id");
            }
            record.Update();

            context.SaveChanges();
        }
    }
}
