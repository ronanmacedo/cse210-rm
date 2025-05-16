using Develop02.Domain.Models;

namespace Develop02.Domain.Interfaces;

public interface IJournalService
{
    public Entry AddEntry(Guid journalId, string text, string prompt);

    public Task<Journal> AddJournalAsync(List<Entry> entries, string journalName);

    public Task<Journal> GetJournalAsync(string journalName);

    public void ExportJournal(Journal journal);
}