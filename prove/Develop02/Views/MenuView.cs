using Develop02.Domain.Interfaces;
using Develop02.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Develop02.Views;

internal class MenuView(IServiceScopeFactory serviceScopeFactory) : IHostedService
{
    private Journal _currentJournal = new();

    private List<Entry> _entries = [];

    private bool _writingJournal;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await DisplayMenuAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Journal app is stopping...");
        return Task.CompletedTask;
    }

    private async Task DisplayMenuAsync()
    {
        try
        {
            await using var scope = serviceScopeFactory.CreateAsyncScope();
            var journalService = scope.ServiceProvider.GetRequiredService<IJournalService>();
            _writingJournal = true;
            Console.WriteLine("Welcome to the Journal Application!");

            while (_writingJournal)
            {
                DisplayOptions();

                var input = int.TryParse(Console.ReadLine(), out var option);
                if (!input)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please provide one of provided options.");
                    Console.ResetColor();
                }
                else
                {
                    await HandleOptionsAsync(journalService, option);
                }
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Exception: {ex.Message}");
            Console.WriteLine($"StackTrace: {ex.StackTrace}");
            Console.WriteLine($"InnerException: {ex.InnerException}");
            Console.ResetColor();
        }
        finally
        {
            Console.WriteLine("Finishing journal writing...");
        }
    }

    private static void DisplayOptions()
    {
        Console.WriteLine("Please choose one of the options:\n" +
                "1 - Write a new entry\n" +
                "2 - Show all the entries\n" +
                "3 - Save your journal\n" +
                "4 - Load a journal\n" +
                "5 - Export the current journal to csv\n" +
                "6 - Quit");
    }

    private static void DisplayEntries(List<Entry> entries)
    {
        Console.WriteLine("All entries:");
        Console.ForegroundColor = ConsoleColor.Blue;
        foreach (var entry in entries)
        {
            Console.WriteLine($"{entry.Date:MM/dd/yyyy HH:mm tt} - Prompt: {entry.Prompt}");
            Console.WriteLine(entry.Text);
        }
        Console.ResetColor();
    }

    private async Task HandleOptionsAsync(IJournalService journalService, int option)
    {
        switch (option)
        {
            case 1:
                var currentJournalId = _currentJournal.Id != Guid.Empty ? _currentJournal.Id : Guid.Empty;
                var entry = journalService.AddEntry(currentJournalId);
                _entries.Add(entry);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Entry added.");
                Console.ResetColor();
                break;

            case 2:
                if (_entries.Count > 0)
                {
                    DisplayEntries(_entries);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No entries available.");
                    Console.ResetColor();
                }
                break;

            case 3:
                if (_entries.Count > 0)
                {
                    _currentJournal = await journalService.AddJournalAsync(_entries);
                    _entries = [.. _currentJournal.Entries];
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No entries available to save on a Journal.");
                    Console.ResetColor();
                }
                break;

            case 4:
                var journal = await journalService.GetJournalAsync();
                if (journal != null)
                {
                    _currentJournal = journal;
                    _entries = [.. _currentJournal.Entries];
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{_currentJournal.Name} found with {_entries.Count} {(_entries.Count > 1 ? "entries" : "entry")}");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No journal found.");
                    Console.ResetColor();
                }
                break;

            case 5:
                if (_entries.Count > 0)
                {
                    journalService.ExportJournal(_currentJournal);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No entries available to export on a Journal.");
                    Console.ResetColor();
                }
                break;

            case 6:
                _writingJournal = false;
                break;

            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No option available.");
                Console.ResetColor();
                break;
        }
    }
}