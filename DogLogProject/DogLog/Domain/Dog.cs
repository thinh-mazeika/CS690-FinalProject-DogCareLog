namespace DogLog.Domain
{
    public class Dog(string name)
    {
        public string Name { get; } = name;
        public DateTime LastFeedTime { get; private set; }
        public DateTime LastWalkTime { get; private set; }

        private const int FeedingWarningHours = 4;

        public void Feed(DateTime time) => LastFeedTime = time;
        public void Walk(DateTime time) => LastWalkTime = time;

        public bool NeedsFood(DateTime? now = null)
        {
            var currentTime = now ?? DateTime.Now;
            var timeDiff = currentTime - LastFeedTime;
            if (LastFeedTime == default) return true;

            return timeDiff.TotalHours > FeedingWarningHours;
        }
    }
}

