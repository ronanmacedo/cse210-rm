using Develop02.Domain.Models;

namespace Develop02.Domain.Interfaces;

public interface IJournalService
{
    public Entry AddEntry(Guid journalId);

    public Task<Journal> AddJournalAsync(List<Entry> entries);

    public Task<Journal> GetJournalAsync();

    public void ExportJournal(Journal journal);
}