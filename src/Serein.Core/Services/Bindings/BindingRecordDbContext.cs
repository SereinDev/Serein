using System.IO;
using Microsoft.EntityFrameworkCore;
using Serein.Core.Models.Bindings;
using Serein.Core.Utils;

namespace Serein.Core.Services.Bindings;

internal sealed class BindingRecordDbContext : DbContext
{
    public DbSet<BindingRecord> Records { get; private set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(
            $"Data Source={Path.GetFullPath(PathConstants.BindingRecordsFile)};"
        );
    }
}
