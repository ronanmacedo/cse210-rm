namespace Develop02.Domain.Models;

public class Journal
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public IList<Entry> Entries { get; set; }
}