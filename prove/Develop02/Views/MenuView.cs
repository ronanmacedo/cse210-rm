using Develop02.Domain.Enums;
using Develop02.Domain.Interfaces;
using Develop02.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Develop02.Views;

internal class MenuView(IServiceScopeFactory serviceScopeFactory) : IHostedService
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
        await using AsyncServiceScope scope = serviceScopeFactory.CreateAsyncScope();
        IJournalService journalService = scope.ServiceProvider.GetRequiredService<IJournalService>();
        _writingJournal = true;
        Console.WriteLine("Welcome to the Journal Application!");

        while (_writingJournal)
        {
            try
            {
                DisplayOptions();

                if (!int.TryParse(Console.ReadLine(), out int option) ||
                    !Enum.IsDefined(typeof(MenuOptions), option))
                {
                    Utils.DisplayErrorMessage("Please provide one of provided options.");
                    continue;
                }

                await HandleOptionsAsync(journalService, (MenuOptions)option);
            }
            catch (Exception ex)
            {
                Utils.DisplayErrorMessage($"Exception: {ex.Message} \n StackTrace: {ex.StackTrace} \n InnerException: {ex.InnerException}");
            }
        }
    }

    private static void DisplayOptions()
    {
        Console.WriteLine(
                "Please choose one of the options:\n" +
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
        foreach (Entry entry in entries)
        {
            Console.WriteLine($"{entry.Date:MM/dd/yyyy HH:mm tt} - Prompt: {entry.Prompt}");
            Console.WriteLine(entry.Text);
        }
        Console.ResetColor();
    }

    private async Task HandleOptionsAsync(IJournalService journalService, MenuOptions option)
    {
        switch (option)
        {
            case MenuOptions.WriteEntry:
                string prompt = GetPrompt();
                Console.WriteLine(prompt);
                string text = Console.ReadLine();
                Guid currentJournalId = _currentJournal.Id != Guid.Empty ? _currentJournal.Id : Guid.Empty;
                Entry entry = journalService.AddEntry(currentJournalId, text, prompt);
                _entries.Add(entry);
                Utils.DisplaySuccessMessage("Entry added.");
                break;

            case MenuOptions.ShowEntries:
                if (_entries.Count > 0)
                {
                    DisplayEntries(_entries);
                    break;
                }

                Utils.DisplayErrorMessage("No entries available.");
                break;

            case MenuOptions.SaveJournal:
                if (_entries.Count > 0)
                {
                    Console.WriteLine("Please, provide a name for the journal:");
                    _currentJournal = await journalService.AddJournalAsync(_entries, Console.ReadLine());
                    _entries = [.. _currentJournal.Entries];
                    break;
                }

                Utils.DisplayErrorMessage("No entries available to save on a journal.");
                break;

            case MenuOptions.LoadJournal:
                Console.WriteLine("What is the journal's name?");
                string journalName = Console.ReadLine();
                Journal journal = await journalService.GetJournalAsync(journalName);
                if (journal != null)
                {
                    _currentJournal = journal;
                    _entries = [.. _currentJournal.Entries];
                    Utils.DisplaySuccessMessage($"{_currentJournal.Name} found with {_entries.Count} {(_entries.Count > 1 ? "entries" : "entry")}");
                    break;
                }

                Utils.DisplayErrorMessage("No journal found.");
                break;

            case MenuOptions.ExportJournal:
                if (_entries.Count > 0 && _currentJournal.Id != Guid.Empty)
                {
                    journalService.ExportJournal(_currentJournal);
                    break;
                }
                else if (_entries.Count > 0)
                {
                    _currentJournal.Entries = _entries;
                    journalService.ExportJournal(_currentJournal);
                    break;
                }

                Utils.DisplayErrorMessage("No entries available to export on a journal.");
                break;

            case MenuOptions.Quit:
                _writingJournal = false;
                break;

            default:
                Utils.DisplayErrorMessage("No option available.");
                break;
        }
    }

    private string GetPrompt()
    {
        return _questions[Random.Shared.Next(_questions.Count)];
    }
}