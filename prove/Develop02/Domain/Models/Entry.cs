namespace Develop02.Domain.Models;

public class Entry
{
    public Guid Id { get; set; }

    public string Text { get; set; }

    public string Prompt { get; set; }

    public DateTime Date { get; set; }

    public Guid JournalId { get; set; }

    public Journal Journal { get; set; }
}