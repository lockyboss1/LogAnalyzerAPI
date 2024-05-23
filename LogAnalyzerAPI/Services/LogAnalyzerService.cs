using LogAnalyzerAPI.Interfaces;
using LogAnalyzerLibrary;

namespace LogAnalyzerApi.Services
{
    public class LogAnalyzerService : ILogAnalyzerService
    {
        private readonly LogAnalyzer _logAnalyzer;

        public LogAnalyzerService()
        {
            var logDirectories = new List<string>
            {
                @"C:\AmadeoLogs",
                @"C:\AWIErrors",
                @"C:\Loggings"
            };
            _logAnalyzer = new LogAnalyzer(logDirectories);
        }

        public Dictionary<string, int> CountUniqueErrors(List<string> logDirectories)
        {
            var logAnalyzer = new LogAnalyzer(logDirectories);
            return logAnalyzer.CountUniqueErrors();
        }

        public Dictionary<string, int> CountDuplicatedErrors(List<string> logDirectories)
        {
            var logAnalyzer = new LogAnalyzer(logDirectories);
            return logAnalyzer.CountDuplicatedErrors();
        }

        public void ArchiveLogsFromPeriod(DateTime startDate, DateTime endDate, List<string> logDirectories)
        {
            var logAnalyzer = new LogAnalyzer(logDirectories);
            logAnalyzer.ArchiveLogsFromPeriod(startDate, endDate, logDirectories);
        }

        public void DeleteArchivesFromPeriod(DateTime startDate, DateTime endDate, List<string> logDirectories)
        {
            var logAnalyzer = new LogAnalyzer(new List<string>());
            logAnalyzer.DeleteArchivesFromPeriod(startDate, endDate, logDirectories);
        }

        public void DeleteLogsFromPeriod(DateTime startDate, DateTime endDate, List<string> logDirectories)
        {
            var logAnalyzer = new LogAnalyzer(logDirectories);
            logAnalyzer.DeleteLogsFromPeriod(startDate, endDate, logDirectories);
        }

        public void UploadLogsToRemoteServer(string remoteUrl, string directory)
        {
            var logAnalyzer = new LogAnalyzer(new List<string> { directory });
            _ = logAnalyzer.UploadLogsToRemoteServer(remoteUrl, directory);
        }

        public int CountTotalLogsInPeriod(DateTime startDate, DateTime endDate, List<string> logDirectories)
        {
            var logAnalyzer = new LogAnalyzer(logDirectories);
            return logAnalyzer.CountTotalLogsInPeriod(startDate, endDate, logDirectories);
        }

        public List<string> SearchLogsBySize(long minSizeInKb, long maxSizeInKb, List<string> logDirectories)
        {
            var logAnalyzer = new LogAnalyzer(logDirectories);
            return logAnalyzer.SearchLogsBySize(minSizeInKb, maxSizeInKb, logDirectories);
        }

        public List<string> SearchLogsByDirectory(string directory, string logFileName)
        {
            var logAnalyzer = new LogAnalyzer(new List<string> { directory });
            return logAnalyzer.SearchLogsByDirectory(directory, logFileName);
        }
    }
}
