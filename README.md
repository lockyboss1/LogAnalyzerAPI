# LogAnalyzerAPI

Postman calls for test purposes

1. Count Number of unique errors per log files

- https://localhost:7165/api/logAnalyzer/count-unique-errors?logDirectories=Logs-samples%2FAmadeoLogs

2. Count number of duplicated errors per log files

- https://localhost:7165/api/logAnalyzer/count-duplicated-errors?logDirectories=Logs-samples%2FAmadeoLogs

3.  Archive logs from period

- https://localhost:7165/api/logAnalyzer/archive-logs-from-period?startDate=2019.10.19&endDate=2020.04.24&logDirectories=Logs-samples%2FAmadeoLogs

4. Delete archive from a period

- https://localhost:7165/api/logAnalyzer/delete-archives-from-period?startDate=2019.10.19&endDate=2020.04.24&logDirectories=Logs-samples%2FAmadeoLogs

5. Upload logs

- https://localhost:7165/api/logAnalyzer/upload-logs

6. Delete logs from a period

- https://localhost:7165/api/logAnalyzer/delete-logs-from-period?startDate=2019.10.19&endDate=2020.04.24&logDirectories=Logs-samples%2FAmadeoLogs

7. Count total logs

- https://localhost:7165/api/logAnalyzer/count-total-logs?startDate=2019.10.19&endDate=2020.04.24&logDirectories=Logs-samples%2FAmadeoLogs

8. Search logs by size

- https://localhost:7165/api/logAnalyzer/search-logs-by-size?startDate=2019.10.19&endDate=2020.04.24&logDirectories=Logs-samples%2FAmadeoLogs

9. Search logs by directory

- https://localhost:7165/api/logAnalyzer/search-logs-by-directory?startDate=2019.10.19&endDate=2020.04.24&logDirectories=Logs-samples%2FAmadeoLogs