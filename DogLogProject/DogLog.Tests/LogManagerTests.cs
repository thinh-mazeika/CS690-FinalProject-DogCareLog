using DogLog.Domain;
using DogLog.Infrastructure;

namespace DogLog.Tests.Infrastructure;

public class LogManagerTests
{
    private static string GetTempFilePath()
    {
        return Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".txt");
    }

    [Fact]
    public void LogAction_Then_ReadAllLogs_ReturnLogs()
    {
        var path = GetTempFilePath();
        var manager = new LogManager(path);
        var log = new ActivityLog("Fed", DateTime.Now, "Alex", "Amount: 100g");
        manager.LogAction(log);
        var logs = manager.ReadAllLogs();

        Assert.Single(logs);
        Assert.Equal("Fed", logs[0].Type);
    }

    [Fact]
    public void GetLatestActivitiesByType_ReturnsOnlyLatest()
    {
        var path = GetTempFilePath();
        var manager = new LogManager(path);

        manager.LogAction(new ActivityLog("Fed", DateTime.Now.AddHours(-5), "Alex", "Old"));
        manager.LogAction(new ActivityLog("Fed", DateTime.Now, "Alex", "New"));

        var latest = manager.GetLatestActivitiesByType();

        Assert.Single(latest);
        Assert.Equal("New", latest[0].Details);
    }

    [Fact]
    public void RestoreDogState_SetsDogPropertiesCorrectly()
    {
        var path = GetTempFilePath();
        var manager = new LogManager(path);

        var feedTime = DateTime.Now.AddHours(-2);
        var walkTime = DateTime.Now.AddHours(-1);

        manager.LogAction(new ActivityLog("Fed", feedTime, "Alex", ""));
        manager.LogAction(new ActivityLog("Walked", walkTime, "Alex", ""));

        var dog = new Dog("Max");
        manager.RestoreDogState(dog);

        Assert.Equal(feedTime, dog.LastFeedTime);
        Assert.Equal(walkTime, dog.LastWalkTime);
    }
}