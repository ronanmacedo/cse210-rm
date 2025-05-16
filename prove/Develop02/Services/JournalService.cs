using Develop02.Domain.Interfaces;
using Develop02.Domain.Models;
using System.Text;

namespace Develop02.Services;

public class JournalService(IJournalRepository journalRepository) : IJournalService
{
    public Entry AddEntry(Guid journalId, string text, string prompt)
    {
        Console.WriteLine("Adding a new entry...");
        return new Entry
        {
            Id = Guid.Empty,
            Prompt = prompt,
            Text = text,
            Date = DateTime.UtcNow,
            JournalId = journalId
        };
    }

    public async Task<Journal> AddJournalAsync(List<Entry> entries, string journalName)
    {
        Console.WriteLine("Adding a new journal...");
        Journal existingJournal = await journalRepository.GetJournalAsync(journalName);

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
        string path = "C:/exports";
        Directory.CreateDirectory(path);
        string fileName = $"{journal.Name ?? $"temp{DateTime.UtcNow:yyyy-MM-dd-HHmm}"}.csv";
        string fileDirectory = $"{path}/{fileName}.csv";

        var csvBuilder = new StringBuilder();

        csvBuilder.AppendLine("Prompt, Text, Date");
        foreach (var entry in journal.Entries ?? [])
        {
            csvBuilder.AppendLine($"{EscapeCsv(entry.Prompt)}, {EscapeCsv(entry.Text)}, {entry.Date:MM/dd/yyyy HH:mm tt}");
        }

        File.WriteAllText(fileDirectory, csvBuilder.ToString());
        Utils.DisplaySuccessMessage($"{fileName} exported successfully!");
    }

    public async Task<Journal> GetJournalAsync(string journalName)
    {
        Console.WriteLine("Getting a journal...");
        return await journalRepository.GetJournalAsync(journalName);
    }

    private async Task HandleUpdateJournalAsync(Journal existingJournal, List<Entry> entries)
    {
        List<Entry> newEntries = [.. entries.Where(
            currentEntries => !existingJournal.Entries.Any(existingEntries => existingEntries.Id == currentEntries.Id))];

        if (newEntries.Count > 0)
        {
            newEntries.ForEach(newEntry =>
            {
                newEntry.Id = Guid.NewGuid();
                newEntry.JournalId = existingJournal.Id;
            });

            await journalRepository.AddEntriesAsync(newEntries);
        }

        existingJournal.Entries = [.. existingJournal.Entries ?? [], .. newEntries];

        journalRepository.UpdateJournal(existingJournal);
    }

    private static string EscapeCsv(string field)
    {
        if (field.Contains('"') || field.Contains(',') || field.Contains('\n'))
        {
            field = field.Replace("\"", "\"\"");
            return $"\"{field}\"";
        }

        return field;
    }
}