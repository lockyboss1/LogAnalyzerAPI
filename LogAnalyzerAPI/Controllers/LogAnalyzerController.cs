using LogAnalyzerAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LogAnalyzerApi.Controllers
{
    [Route("api/logAnalyzer")]
    [ApiController]
    public class LogAnalyzerController : ControllerBase
    {
        private readonly ILogAnalyzerService _logAnalyzerService;

        public LogAnalyzerController(ILogAnalyzerService logAnalyzerService)
        {
            _logAnalyzerService = logAnalyzerService;
        }

        [HttpGet("count-unique-errors")]
        public ActionResult<Dictionary<string, int>> CountUniqueErrors([FromQuery] List<string> logDirectories)
        {
            return Ok(_logAnalyzerService.CountUniqueErrors(logDirectories));
        }

        [HttpGet("count-duplicated-errors")]
        public ActionResult<Dictionary<string, int>> CountDuplicatedErrors([FromQuery] List<string> logDirectories)
        {
            return Ok(_logAnalyzerService.CountDuplicatedErrors(logDirectories));
        }

        [HttpPost("archive-logs-from-period")]
        public IActionResult ArchiveLogsFromPeriod([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] List<string> logDirectories)
        {
            _logAnalyzerService.ArchiveLogsFromPeriod(startDate, endDate, logDirectories);
            return NoContent();
        }

        [HttpDelete("delete-archives-from-period")]
        public IActionResult DeleteArchivesFromPeriod([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] List<string> logDirectories)
        {
            _logAnalyzerService.DeleteArchivesFromPeriod(startDate, endDate, logDirectories);
            return NoContent();
        }

        [HttpPost("upload-logs")]
        public async Task<IActionResult> UploadLogsToRemoteServer([FromQuery] string apiUrl, [FromQuery] string directory)
        {
            _logAnalyzerService.UploadLogsToRemoteServer(apiUrl, directory);
            return NoContent();
        }

        [HttpDelete("delete-logs-from-period")]
        public IActionResult DeleteLogsFromPeriod([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] List<string> logDirectories)
        {
            _logAnalyzerService.DeleteLogsFromPeriod(startDate, endDate, logDirectories);
            return NoContent();
        }

        [HttpGet("count-total-logs")]
        public ActionResult<int> CountTotalLogsInPeriod([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] List<string> logDirectories)
        {
            return Ok(_logAnalyzerService.CountTotalLogsInPeriod(startDate, endDate, logDirectories));
        }

        [HttpGet("search-logs-by-size")]
        public ActionResult<List<string>> SearchLogsBySize([FromQuery] long minSizeInKb, [FromQuery] long maxSizeInKb, [FromQuery] List<string> logDirectories)
        {
            return Ok(_logAnalyzerService.SearchLogsBySize(minSizeInKb, maxSizeInKb, logDirectories));
        }

        [HttpGet("search-logs-by-directory")]
        public ActionResult<List<string>> SearchLogsByDirectory([FromQuery] string directory, [FromQuery] string logFileName)
        {
            var result = _logAnalyzerService.SearchLogsByDirectory(directory, logFileName);
            return Ok(result);
        }
    }
}