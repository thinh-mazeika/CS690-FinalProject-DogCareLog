namespace DogLog.Domain
{
    public class ActivityLog
    {
        public string Type { get; }
        public DateTime Timestamp { get; }
        public string PerformedBy { get; }
        public string Details { get; }

        public ActivityLog(string type, DateTime timestamp, string performedBy, string details)
        {
            Type = type;
            Timestamp = timestamp;
            PerformedBy = performedBy;
            Details = details;
        }

        public string ToFileString()
        {
            return $"{Type} | {Timestamp} | {Details} | By: {PerformedBy}";
        }

        public static ActivityLog FromFileString(string line)
        {
            var parts = line.Split('|');
            return new ActivityLog(
                parts[0].Trim(),
                DateTime.Parse(parts[1]),
                parts[3].Replace("By:", "").Trim(),
                parts.Length > 2 ? parts[2].Trim() : ""
            );
        }
    }
}

