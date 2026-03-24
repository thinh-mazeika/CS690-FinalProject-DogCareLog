using DogLog.Domain;

namespace DogLog.Tests.Domain;

public class ActivityLogTests
{
    [Fact]
    public void ToFileString_And_FromFileString_RoundTrip_Works()
    {
        var log = new ActivityLog("Fed", DateTime.Now, "Alex", "Amount: 100g");

        var fileString = log.ToFileString();
        var parsed = ActivityLog.FromFileString(fileString);

        Assert.Equal(log.Type, parsed.Type);
        Assert.Equal(log.PerformedBy, parsed.PerformedBy);
        Assert.Equal(log.Details, parsed.Details);
        Assert.Equal(log.Timestamp.ToString(), parsed.Timestamp.ToString());
    }

    [Fact]
    public void ToFriendlyString_ReturnsExpectedFormat()
    {
        var time = DateTime.Now.AddHours(-2);
        var log = new ActivityLog("Fed", time, "Alex", "Amount: 100g");

        var result = log.ToFriendlyString(DateTime.Now);

        Assert.Contains("Fed", result);
        Assert.Contains("Alex", result);
        Assert.Contains("hours ago", result);
    }
}