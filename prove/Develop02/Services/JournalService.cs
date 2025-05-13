using Develop02.Domain.Interfaces;
using Develop02.Domain.Models;
using System.Text;

namespace Develop02.Services;

public class JournalService(IJournalRepository journalRepository) : IJournalService
{
    private readonly List<string> _questions = [
    "Who was the most interesting person I interacted with today?",
    "What was the best part of my day?",
    "How did I see the hand of the Lord in my life today?",
    "What was the strongest emotion I felt today?",
    "If I had one thing I could do over today, what would it be?",
    "What challenge did I face today, and how did I respond to it?",
    "Did I offer or receive meaningful support from someone today?",
    "What brought me the most joy or made me smile today?",
    "Did I collaborate with anyone on a task or shared goal today?",
    "How was your scriptures study today?",
    "What did you eat today?",
    "Did you drink your favorite drink today?"];

    public Entry AddEntry(Guid journalId)
    {
        Console.WriteLine("Adding a new Entry...");
        var prompt = GetPrompt();
        Console.WriteLine(prompt);
        var text = Console.ReadLine();
        var entry = new Entry
        {
            Id = Guid.Empty,
            Prompt = prompt,
            Text = text,
            Date = DateTime.UtcNow,
            JournalId = journalId
        };
        return entry;
    }

    public async Task<Journal> AddJournalAsync(List<Entry> entries)
    {
        Console.WriteLine("Adding a new Journal...");
        Console.WriteLine("Please, provide a name for the Journal:");
        var journalName = Console.ReadLine();
        var existingJournal = await journalRepository.GetJournalAsync(journalName);

        if (existingJournal != null)
        {
            await HandleUpdateJournalAsync(existingJournal, entries);
            return existingJournal;
        }

        var journal = new Journal
        {
            Id = Guid.NewGuid(),
            Name = journalName,
        };
        entries.ForEach(_ =>
        {
            _.Id = Guid.NewGuid();
            _.JournalId = journal.Id;
        });
        journal.Entries = entries;

        await journalRepository.AddJournalAsync(journal);
        return journal;
    }

    public void ExportJournal(Journal journal)
    {
        Console.WriteLine("Exporting...");
        var path = "C:/exports";
        Directory.CreateDirectory(path);
        var fileDirectory = $"{path}/{journal.Name}.csv";

        var csvBuilder = new StringBuilder();

        csvBuilder.AppendLine("Prompt, Text, Date");
        foreach (var entry in journal.Entries)
        {
            csvBuilder.AppendLine($"{entry.Prompt}, {entry.Text}, {entry.Date:MM/dd/yyyy HH:mm tt}");
        }

        File.WriteAllText(fileDirectory, csvBuilder.ToString());
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"{journal.Name} exported successfully!");
        Console.ResetColor();
    }

    public async Task<Journal> GetJournalAsync()
    {
        Console.WriteLine("Getting a Journal...");
        Console.WriteLine("What is the Journal's name?");
        var journalName = Console.ReadLine();
        var journal = await journalRepository.GetJournalAsync(journalName);
        return journal;
    }

    private string GetPrompt()
    {
        var prompt = _questions[Random.Shared.Next(_questions.Count)];
        return prompt;
    }

    private async Task HandleUpdateJournalAsync(Journal existingJournal, List<Entry> entries)
    {
        var newEntries = entries
            .Where(currentEntries => !existingJournal.Entries.Any(existingEntries => existingEntries.Id == currentEntries.Id))
            .ToList();

        if (newEntries.Count > 0)
        {
            newEntries.ForEach(newEntry =>
            {
                newEntry.Id = Guid.NewGuid();
                if (newEntry.JournalId == Guid.Empty)
                {
                    newEntry.JournalId = existingJournal.Id;
                }
            });

            await journalRepository.AddEntriesAsync(newEntries);
        }

        existingJournal.Entries = [.. existingJournal.Entries ?? [], .. newEntries];

        journalRepository.UpdateJournal(existingJournal);
    }
}