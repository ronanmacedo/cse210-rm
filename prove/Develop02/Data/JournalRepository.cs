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
            Utils.DisplaySuccessMessage("Entries added successfully!");
            return;
        }

        Utils.DisplayErrorMessage("An error happened while adding entries.");
    }

    public async Task<Journal> AddJournalAsync(Journal journalModel)
    {
        await _journals.AddAsync(journalModel);
        var saved = journalContext.SaveChanges();
        journalContext.ChangeTracker.Clear();
        if (saved >= 1)
        {
            Utils.DisplaySuccessMessage("Journal saved successfully!");
            return journalModel;
        }

        Utils.DisplayErrorMessage("An error happened while saving a new journal.");
        return null;
    }

    public async Task<Journal> GetJournalAsync(string journalName)
    {
        return await _journals.AsNoTracking()
            .Where(_ => _.Name == journalName)
            .AsNoTracking()
            .Include(_ => _.Entries)
            .FirstOrDefaultAsync();
    }

    public void UpdateJournal(Journal journalModel)
    {
        _journals.Update(journalModel);
        var saved = journalContext.SaveChanges();
        journalContext.ChangeTracker.Clear();
        if (saved >= 1)
        {
            Utils.DisplaySuccessMessage("Journal updated successfully!");
            return;
        }

        Utils.DisplaySuccessMessage("An error happened while update a journal.");
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