using System.Globalization;
using System.IO.Compression;

namespace LogAnalyzerLibrary
{
    public class LogAnalyzer
    {
        private readonly List<string> _logDirectories;

        public LogAnalyzer(List<string> logDirectories)
        {
            _logDirectories = logDirectories;
        }

        public Dictionary<string, int> CountUniqueErrors()
        {
            var errorCounts = new Dictionary<string, int>();

            foreach (var directory in _logDirectories)
            {
                var logFiles = Directory.GetFiles(directory, "*.log", SearchOption.AllDirectories);
                foreach (var file in logFiles)
                {
                    var lines = File.ReadAllLines(file);
                    var uniqueErrors = new HashSet<string>();

                    var currentError = ""; // Holds the current error message

                    foreach (var line in lines)
                    {
                        if (line.Trim().Equals("__________________________"))
                        {
                            // End of an error message, add it to uniqueErrors
                            if (!string.IsNullOrEmpty(currentError))
                            {
                                uniqueErrors.Add(currentError);
                                currentError = ""; // Reset currentError for the next error
                            }
                        }
                        else
                        {
                            // Accumulate the error message
                            currentError += line + "\n";
                        }
                    }

                    // Store the count of unique errors for the current file
                    errorCounts[file] = uniqueErrors.Count;
                }
            }

            return errorCounts;
        }

        public Dictionary<string, int> CountDuplicatedErrors()
        {
            var errorCounts = new Dictionary<string, int>();

            foreach (var directory in _logDirectories)
            {
                var logFiles = Directory.GetFiles(directory, "*.log", SearchOption.AllDirectories);
                foreach (var file in logFiles)
                {
                    var lines = File.ReadAllLines(file);
                    var currentError = "";
                    var errorsInFile = new HashSet<string>();

                    foreach (var line in lines)
                    {
                        if (line.Trim().Equals("__________________________"))
                        {
                            // End of an error message, add it to errorsInFile
                            if (!string.IsNullOrEmpty(currentError))
                            {
                                if (!errorsInFile.Contains(currentError))
                                {
                                    errorsInFile.Add(currentError);
                                }
                                else
                                {
                                    // Error is already in the set, it's a duplicate
                                    if (!errorCounts.ContainsKey(file))
                                    {
                                        errorCounts[file] = 0;
                                    }
                                    errorCounts[file]++;
                                }
                                currentError = ""; // Reset currentError for the next error
                            }
                        }
                        else
                        {
                            // Accumulate the error message
                            currentError += line + "\n";
                        }
                    }
                }
            }

            // Ensure each log file is included in the result even if no duplicates are found
            foreach (var directory in _logDirectories)
            {
                var logFiles = Directory.GetFiles(directory, "*.log", SearchOption.AllDirectories);
                foreach (var file in logFiles)
                {
                    if (!errorCounts.ContainsKey(file))
                    {
                        errorCounts[file] = 0;
                    }
                }
            }

            // Sort the error counts by logfile name
            var sortedErrorCounts = errorCounts.OrderBy(pair => pair.Key)
                                               .ToDictionary(pair => pair.Key, pair => pair.Value);

            return sortedErrorCounts;
        }

        public void DeleteLogsFromPeriod(DateTime startDate, DateTime endDate, List<string> logDirectories)
        {
            foreach (var directory in logDirectories)
            {
                // Get all log files within the directory and its subdirectories
                var logFiles = Directory.GetFiles(directory, "*.log", SearchOption.AllDirectories);

                foreach (var file in logFiles)
                {
                    // Extract date from the file name
                    var fileName = Path.GetFileNameWithoutExtension(file);
                    if (DateTime.TryParseExact(fileName, "yyyy.MM.dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fileDate))
                    {
                        // Check if the file's date falls within the specified period
                        if (fileDate >= startDate && fileDate <= endDate)
                        {
                            // Delete the log file
                            File.Delete(file);
                        }
                    }
                }
            }
        }

        public void ArchiveLogsFromPeriod(DateTime startDate, DateTime endDate, List<string> logDirectories)
        {
            var archiveName = $"{startDate:dd_MM_yyyy}-{endDate:dd_MM_yyyy}.zip";
            var archivePath = Path.Combine(logDirectories.FirstOrDefault(), archiveName);

            using (var zipArchive = ZipFile.Open(archivePath, ZipArchiveMode.Create))
            {
                foreach (var directory in logDirectories)
                {
                    var logFiles = Directory.GetFiles(directory, "*.log", SearchOption.AllDirectories);
                    foreach (var file in logFiles)
                    {
                        var lines = File.ReadAllLines(file);
                        var logFileName = Path.GetFileName(file);

                        // Check if the log file falls within the specified date range
                        if (IsLogFileWithinPeriod(file, startDate, endDate))
                        {
                            // Add the file to the zip archive
                            var entry = zipArchive.CreateEntry(logFileName);

                            // Write the contents of the file into the zip entry
                            using (var writer = new StreamWriter(entry.Open()))
                            {
                                foreach (var line in lines)
                                {
                                    writer.WriteLine(line);
                                }
                            }

                            // Delete the log file after archiving
                            File.Delete(file);
                        }
                    }
                }
            }
        }

        public void DeleteArchivesFromPeriod(DateTime startDate, DateTime endDate, List<string> logDirectories)
        {
            foreach (var directory in logDirectories)
            {
                // Get all zip files within the directory and its subdirectories
                var archiveFiles = Directory.GetFiles(directory, "*.zip", SearchOption.AllDirectories);

                foreach (var file in archiveFiles)
                {
                    // Extract start and end dates from the file name
                    var fileName = Path.GetFileNameWithoutExtension(file);
                    var dateParts = fileName.Split('-');
                    if (dateParts.Length == 2 &&
                        DateTime.TryParseExact(dateParts[0], "dd_MM_yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fileStartDate) &&
                        DateTime.TryParseExact(dateParts[1], "dd_MM_yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fileEndDate))
                    {
                        // Delete the file and its containing directory recursively
                        File.Delete(file);
                    }
                }
            }
        }

        public async Task UploadLogsToRemoteServer(string apiUrl, string directory)
        {
            var client = new HttpClient();

            var logFiles = Directory.GetFiles(directory, "*.log", SearchOption.AllDirectories);
            foreach (var file in logFiles)
            {
                using var content = new MultipartFormDataContent();
                using var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
                using var streamContent = new StreamContent(fileStream);

                content.Add(streamContent, "file", Path.GetFileName(file));

                await client.PostAsync(apiUrl, content);
            }
        }

        public int CountTotalLogsInPeriod(DateTime startDate, DateTime endDate, List<string> logDirectories)
        {
            int totalCount = 0;

            foreach (var directory in logDirectories)
            {
                // Get all log files within the directory and its subdirectories
                var logFiles = Directory.GetFiles(directory, "*.log", SearchOption.AllDirectories);

                foreach (var file in logFiles)
                {
                    // Extract date from the file name
                    var fileName = Path.GetFileNameWithoutExtension(file);
                    if (DateTime.TryParseExact(fileName, "yyyy.MM.dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fileDate))
                    {
                        // Check if the file's date falls within the specified period
                        if (fileDate >= startDate && fileDate <= endDate)
                        {
                            // Increment the total count
                            totalCount++;
                        }
                    }
                }
            }

            return totalCount;
        }

        public List<string> SearchLogsBySize(long minSizeInKb, long maxSizeInKb, List<string> logDirectories)
        {
            if (minSizeInKb < 0 || maxSizeInKb < 0)
            {
                throw new ArgumentException("Size values must be non-negative.");
            }

            if (minSizeInKb > maxSizeInKb)
            {
                throw new ArgumentException("Minimum size cannot be greater than maximum size.");
            }

            var matchingFiles = new List<string>();

            foreach (var directory in logDirectories)
            {
                // Get all log files within the directory and its subdirectories
                var logFiles = Directory.GetFiles(directory, "*.log", SearchOption.AllDirectories);

                foreach (var file in logFiles)
                {
                    // Check if the file size falls within the specified size range
                    var fileInfo = new FileInfo(file);
                    if (fileInfo.Length >= minSizeInKb * 1024 && fileInfo.Length <= maxSizeInKb * 1024)
                    {
                        matchingFiles.Add(file);
                    }
                }
            }

            return matchingFiles;
        }

        public List<string> SearchLogsByDirectory(string directory, string logFileName)
        {
            var logContent = new List<string>();

            // Ensure the directory exists
            if (Directory.Exists(directory))
            {
                // Get all log files within the directory and its subdirectories
                var logFiles = Directory.GetFiles(directory, "*.log", SearchOption.AllDirectories);

                bool fileFound = false;

                foreach (var file in logFiles)
                {
                    // Check if the file matches the specified log file name
                    if (Path.GetFileName(file).Equals(logFileName, StringComparison.OrdinalIgnoreCase))
                    {
                        // Read the content of the log file and add it to the result
                        logContent.AddRange(File.ReadAllLines(file));
                        fileFound = true;
                    }
                }

                // If the file was not found, add a message indicating that the log file was not found
                if (!fileFound)
                {
                    logContent.Add($"Log file '{logFileName}' not found in the specified directory.");
                }
            }
            else
            {
                logContent.Add($"Directory '{directory}' does not exist.");
            }

            return logContent;
        }

        private bool IsLineWithinPeriod(string line, DateTime startDate, DateTime endDate)
        {
            // Assumes log line starts with a timestamp in a parseable format.
            if (DateTime.TryParse(line.Substring(0, 19), out var logDate))
            {
                return logDate >= startDate && logDate <= endDate;
            }
            return false;
        }

        private bool IsLogFileWithinPeriod(string filePath, DateTime startDate, DateTime endDate)
        {
            var fileInfo = new FileInfo(filePath);
            var fileLastWriteTime = fileInfo.LastWriteTime;
            return fileLastWriteTime >= startDate && fileLastWriteTime <= endDate.AddDays(1);
        }
    }
}