using DogLog.Domain;
using DogLog.Infrastructure;
using Spectre.Console;

namespace DogLog.ConsoleUI
{
    public class AppMenu
    {
        private readonly Dog _dog;
        private readonly LogManager _logManager;

        public AppMenu(Dog dog, LogManager logManager)
        {
            _dog = dog;
            _logManager = logManager;
        }

        public void Run()
        {
            while (true)
            {
                Console.Clear();
                AnsiConsole.Write(new FigletText("DogLog").Color(Color.Blue));

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Select an [blue]option[/]:")
                        .AddChoices(new[] {
                            "1. Feed Dog",
                            "2. Walk Dog",
                            "3. View Last Activities",
                            "4. Check If Dog Needs Food",
                            "5. Exit"
                        }));

                switch (choice)
                {
                    case "1. Feed Dog": FeedDog(); break;
                    case "2. Walk Dog": WalkDog(); break;
                    case "3. View Last Activities": ShowLogs(); break;
                    case "4. Check If Dog Needs Food": CheckFood(); break;
                    case "5. Exit": return;
                }

                AnsiConsole.MarkupLine("\nPress [green]Enter[/] to return to menu...");
                Console.ReadLine();
            }
        }

        private User SelectUser(string action)
        {
            var name = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"Who {action} the dog?")
                    .AddChoices("Alex", "Roommate")
            );
            return new User(name);
        }

        private void FeedDog()
        {
            if (!_dog.NeedsFood())
            {
                string message = $"Dog was last fed at {_dog.LastFeedTime}. Too soon!";
                string safeMessage = Markup.Escape(message);
                AnsiConsole.MarkupLine($"[red]WARNING:[/]{safeMessage}");
                return;
            }

            var user = SelectUser("fed");
            int grams = AnsiConsole.Ask<int>("How many grams of food?");

            _dog.Feed(DateTime.Now);
            _logManager.LogAction(new ActivityLog("Fed", DateTime.Now, user.Name, $"Amount: {grams}g"));

            AnsiConsole.MarkupLine("[green]Feeding logged.[/]");
        }

        private void WalkDog()
        {
            var user = SelectUser("walked");
            int minutes = AnsiConsole.Ask<int>("How many minutes of walk?");

            _dog.Walk(DateTime.Now);
            _logManager.LogAction(new ActivityLog("Walk", DateTime.Now, user.Name, $"Duration: {minutes}min"));

            AnsiConsole.MarkupLine("[green]Walking logged.[/]");
        }

        private void CheckFood()
        {
            if (_dog.LastFeedTime == default)
            {
                AnsiConsole.MarkupLine("[blue]No feeding history.[/]");
                return;
            }
            if (_dog.NeedsFood())
            {
                AnsiConsole.MarkupLine("[red]Dog needs food.[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[green]Dog is full.[/]");
            }
        }

        private void ShowLogs(int take = int.MaxValue)
        {
            var logs = _logManager.ReadAllLogs().OrderByDescending(line => line.Timestamp).Take(take);

            if (!logs.Any())
            {
                AnsiConsole.MarkupLine("[blue]No logs found.[/]");
            }

            var table = new Table();
            table.AddColumn("Type");
            table.AddColumn("Time");
            table.AddColumn("Details");
            table.AddColumn("PerformedBy");

            foreach (var log in logs)
            {
                table.AddRow(
                    log.Type,
                    log.Timestamp.ToString(),
                    log.Details,
                    log.PerformedBy
                );
            }
            AnsiConsole.Write(table);
        }
    }
}