using Spectre.Console;

namespace DogLog.Domain
{
    public class Dog
    {
        public string Name { get; }
        public DateTime LastFeedTime { get; private set; }
        public DateTime LastWalkTime { get; private set; }

        private const int FeedingWarningHours = 4;

        public Dog(string name)
        {
            Name = name;
        }

        public void Feed(DateTime time) => LastFeedTime = time;
        public void Walk(DateTime time) => LastWalkTime = time;

        public bool NeedsFood()
        {
            TimeSpan diff = DateTime.Now - LastFeedTime;
            return diff.TotalHours > FeedingWarningHours;
        }
    }
}

