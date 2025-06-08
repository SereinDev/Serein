using System.IO;
using Microsoft.EntityFrameworkCore;
using Serein.Core.Models.Bindings;
using Serein.Core.Utils;

namespace Serein.Core.Services.Bindings;

#pragma warning disable CS8618

internal sealed class BindingRecordDbContext : DbContext
{
    public DbSet<BindingRecord> Datas { get; private set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(
            $"Data Source={Path.GetFullPath(PathConstants.BindingRecordsFile)};"
        );
    }
}
