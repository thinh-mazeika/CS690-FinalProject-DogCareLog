using DogLog.Domain;
using DogLog.Infrastructure;
using DogLog.ConsoleUI;

namespace DogLog;

class Program
{
    static void Main()
    {
        var dog = new Dog("Alex");
        var logManager = new LogManager("doglog.txt");
        var menu = new AppMenu(dog, logManager);

        menu.Run();
    }
}
