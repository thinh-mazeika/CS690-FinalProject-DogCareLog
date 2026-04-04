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
                        .AddChoices([
                            "1. Feed Dog",
                            "2. Walk Dog",
                            "3. View Last Activities",
                            "4. Check If Dog Needs Food",
                            "5. View Full Log History",
                            "6. Exit"
                        ]));

                switch (choice)
                {
                    case "1. Feed Dog": FeedDog(); break;
                    case "2. Walk Dog": WalkDog(); break;
                    case "3. View Last Activities": ShowLastActivities(); break;
                    case "4. Check If Dog Needs Food": CheckFood(); break;
                    case "5. View Full Log History": ShowLogs(); break;
                    case "6. Exit": return;
                }

                AnsiConsole.MarkupLine("\nPress [green]Enter[/] to return to menu...");
                Console.ReadLine();
            }
        }

        // helper function to set user
        private static User SelectUser(string action)
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
                AnsiConsole.MarkupLine($"\n[red]WARNING: [/]{safeMessage}");
                return; // early return to prevent user over-feeding
            }

            var user = SelectUser("fed");
            int grams = AnsiConsole.Ask<int>("How many grams of food?");

            _dog.Feed(DateTime.Now);
            _logManager.LogAction(new ActivityLog("Fed", DateTime.Now, user.Name, $"Amount: {grams}g"));

            AnsiConsole.MarkupLine("\n[green]Feeding logged.[/]");
        }

        private void WalkDog()
        {
            var user = SelectUser("walked");
            int minutes = AnsiConsole.Ask<int>("How many minutes of walk?");

            _dog.Walk(DateTime.Now);
            _logManager.LogAction(new ActivityLog("Walked", DateTime.Now, user.Name, $"Duration: {minutes}min"));

            AnsiConsole.MarkupLine("\n[green]Walking logged.[/]");
        }

        private void CheckFood()
        {
            if (_dog.LastFeedTime == default)
            {
                AnsiConsole.MarkupLine("\n[blue]No feeding history.[/]");
                return;
            }
            if (_dog.NeedsFood())
            {
                AnsiConsole.MarkupLine("\n[red]Dog needs food.[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("\n[green]Dog is full.[/]");
            }
        }

        private void ShowLastActivities()
        {
            var logs = _logManager.GetLatestActivitiesByType();

            if (logs.Count == 0)
            {
                AnsiConsole.MarkupLine("\n[blue]No logs found.[/]");
            }

            ShowLogsSummary(logs);
        }

        private static void ShowLogsTable(List<ActivityLog> logs)
        {
            var table = new Table();
            table.AddColumn("[bold]Type[/]");
            table.AddColumn("[bold]Time[/]");
            table.AddColumn("[bold]Details[/]");
            table.AddColumn("[bold]PerformedBy[/]");

            int index = 0;

            foreach (var log in logs)
            {
                // highlight the first row in the table
                if (index == 0)
                {
                    table.AddRow($"[bold yellow]{log.Type}[/]", $"[bold yellow]{log.Timestamp.ToString()}[/]",
                    $"[bold yellow]{log.Details}[/]",
                    $"[bold yellow]{log.PerformedBy}[/]");
                }
                else
                {
                    table.AddRow(
                        log.Type,
                        log.Timestamp.ToString(),
                        log.Details,
                        log.PerformedBy
                    );
                }
                index++;
            }
            AnsiConsole.Write(table);
        }

        private static void ShowLogsSummary(List<ActivityLog> logs)
        {
            foreach (var log in logs)
            {
                AnsiConsole.MarkupLine($"\n[bold green]{log.ToFriendlyString()}[/]");
            }
        }

        private void ShowLogs(int take = int.MaxValue)
        {
            // sort most recent activity at the top
            var logs = _logManager.ReadAllLogs().OrderByDescending(line => line.Timestamp).Take(take).ToList();

            if (logs.Count == 0)
            {
                AnsiConsole.MarkupLine("\n[blue]No logs found.[/]");
            }

            ShowLogsTable(logs);
        }
    }
}