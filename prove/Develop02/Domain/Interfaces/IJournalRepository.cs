using Develop02.Domain.Models;

namespace Develop02.Domain.Interfaces;

public interface IJournalRepository
{
    public Task AddEntriesAsync(List<Entry> entries);

    public Task<Journal> AddJournalAsync(Journal journalModel);

    public void UpdateJournal(Journal journalModel);

    public Task<Journal> GetJournalAsync(string journalName);
}