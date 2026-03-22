using DogLog.Domain;

namespace DogLog.Infrastructure
{
    public class LogManager
    {
        private readonly string _filePath;

        public LogManager(string filePath)
        {
            _filePath = filePath;
            if (!File.Exists(_filePath))
            {
                File.Create(_filePath).Close();
            }
        }

        public void LogAction(ActivityLog action)
        {
            File.AppendAllText(_filePath, action.ToFileString() + Environment.NewLine);
        }

        public List<ActivityLog> ReadAllLogs()
        {
            return [.. File.ReadAllLines(_filePath)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(ActivityLog.FromFileString)];
        }

        public List<ActivityLog> GetLatestActivitiesByType()
        {
            return [.. ReadAllLogs()
                .GroupBy(log => log.Type)
                .Select(group => group
                    .OrderByDescending(l => l.Timestamp)
                    .First()
                )];
        }

        public ActivityLog? GetLatestLogByType(string type)
        {
            return ReadAllLogs()
                .Where(l => l.Type == type)
                .OrderByDescending(l => l.Timestamp)
                .FirstOrDefault();
        }
    }
}

