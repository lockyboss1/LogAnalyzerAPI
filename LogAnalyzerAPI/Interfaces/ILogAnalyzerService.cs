namespace LogAnalyzerAPI.Interfaces
{
	public interface ILogAnalyzerService
	{
        Dictionary<string, int> CountUniqueErrors(List<string> logDirectories);
        Dictionary<string, int> CountDuplicatedErrors(List<string> logDirectories);
        void DeleteLogsFromPeriod(DateTime startDate, DateTime endDate, List<string> logDirectories);
        void ArchiveLogsFromPeriod(DateTime startDate, DateTime endDate, List<string> logDirectories);
        void UploadLogsToRemoteServer(string remoteUrl, string directory);
        int CountTotalLogsInPeriod(DateTime startDate, DateTime endDate, List<string> logDirectories);
        List<string> SearchLogsBySize(long minSizeInKb, long maxSizeInKb, List<string> logDirectories);
        List<string> SearchLogsByDirectory(string directory, string logFileName);
        void DeleteArchivesFromPeriod(DateTime startDate, DateTime endDate, List<string> logDirectories);
    }
}