using DogLog.Domain;
using DogLog.Infrastructure;
using DogLog.ConsoleUI;

namespace DogLog;

class Program
{
    static void Main()
    {
        var dog = new Dog("Max");
        var logManager = new LogManager("doglog.txt");

        // without this data syncing, lastFedTime is incorrect when user runs the application
        logManager.RestoreDogState(dog);

        var menu = new AppMenu(dog, logManager);

        menu.Run();
    }
}
