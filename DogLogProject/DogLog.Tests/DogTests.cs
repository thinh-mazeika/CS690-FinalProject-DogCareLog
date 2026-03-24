using DogLog.Domain;

namespace DogLog.Tests.Domain;

public class DogTests
{
    [Fact]
    public void NeedsFood_WheneverFed_ReturnsTrue()
    {
        var dog = new Dog("Max");
        var result = dog.NeedsFood();

        Assert.True(result);
    }

    [Fact]
    public void NeedsFood__WhenFedRecently_ReturnsFalse()
    {
        var dog = new Dog("Max");
        dog.Feed(DateTime.Now.AddHours(-2));
        var result = dog.NeedsFood();

        Assert.False(result);
    }

    [Fact]
    public void NeedsFood_WhenFedLongAgo_ReturnsTrue()
    {
        var dog = new Dog("Max");
        dog.Feed(DateTime.Now.AddHours(-5));
        var result = dog.NeedsFood();

        Assert.True(result);
    }

    [Fact]
    public void Feed_SetsLastFeedTime()
    {
        var dog = new Dog("Max");
        var time = DateTime.Now;
        dog.Feed(time);

        Assert.Equal(time, dog.LastFeedTime);
    }

    [Fact]
    public void Feed_SetsLastWalkTime()
    {
        var dog = new Dog("Max");
        var time = DateTime.Now;
        dog.Walk(time);

        Assert.Equal(time, dog.LastWalkTime);
    }
}
