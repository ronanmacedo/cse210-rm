using Develop02.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Develop02.Data;

public class JournalContext : DbContext
{
    public DbSet<Journal> Journals { get; set; }

    public DbSet<Entry> Entries { get; set; }

    private readonly string _dbPath;

    public JournalContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        _dbPath = Path.Join(path, "journal.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={_dbPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Journal>()
            .HasMany(_ => _.Entries)
            .WithOne(_ => _.Journal)
            .HasForeignKey(_ => _.JournalId);
    }
}