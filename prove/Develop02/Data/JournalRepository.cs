using Develop02.Domain.Interfaces;
using Develop02.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Develop02.Data;

public class JournalRepository(JournalContext journalContext) : IJournalRepository, IDisposable
{
    private readonly DbSet<Journal> _journals = journalContext.Journals;

    private readonly DbSet<Entry> _entries = journalContext.Entries;

    private bool _disposedValue;

    public async Task AddEntriesAsync(List<Entry> entries)
    {
        await _entries.AddRangeAsync(entries);
        var saved = journalContext.SaveChanges();
        journalContext.ChangeTracker.Clear();
        if (saved >= 1)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Entries added successfully!");
            Console.ResetColor();
            return;
        }

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("An error happened while adding entries.");
        Console.ResetColor();
    }

    public async Task<Journal> AddJournalAsync(Journal journalModel)
    {
        await _journals.AddAsync(journalModel);
        var saved = journalContext.SaveChanges();
        journalContext.ChangeTracker.Clear();
        if (saved >= 1)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Journal saved successfully!");
            Console.ResetColor();
            return journalModel;
        }

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("An error happened while saving a new journal.");
        Console.ResetColor();
        return null;
    }

    public async Task<Journal> GetJournalAsync(string journalName)
    {
        var journal = await _journals.AsNoTracking()
            .Where(_ => _.Name == journalName)
            .AsNoTracking()
            .Include(_ => _.Entries)
            .FirstOrDefaultAsync();
        return journal;
    }

    public void UpdateJournal(Journal journalModel)
    {
        _journals.Update(journalModel);
        var saved = journalContext.SaveChanges();
        journalContext.ChangeTracker.Clear();
        if (saved >= 1)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Journal updated successfully!");
            Console.ResetColor();
            return;
        }

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("An error happened while update a journal.");
        Console.ResetColor();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                journalContext.Dispose();
            }

            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}