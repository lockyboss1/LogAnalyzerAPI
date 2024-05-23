using System.ComponentModel.DataAnnotations;
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
        public ActionResult<Dictionary<string, int>> CountUniqueErrors([FromQuery, Required] List<string> logDirectories)
        {
            return Ok(_logAnalyzerService.CountUniqueErrors(logDirectories));
        }

        [HttpGet("count-duplicated-errors")]
        public ActionResult<Dictionary<string, int>> CountDuplicatedErrors([FromQuery, Required] List<string> logDirectories)
        {
            return Ok(_logAnalyzerService.CountDuplicatedErrors(logDirectories));
        }

        [HttpPost("archive-logs-from-period")]
        public IActionResult ArchiveLogsFromPeriod([FromQuery, Required] DateTime startDate, [FromQuery, Required] DateTime endDate, [FromQuery, Required] List<string> logDirectories)
        {
            _logAnalyzerService.ArchiveLogsFromPeriod(startDate, endDate, logDirectories);
            return NoContent();
        }

        [HttpDelete("delete-archives-from-period")]
        public IActionResult DeleteArchivesFromPeriod([FromQuery, Required] DateTime startDate, [FromQuery, Required] DateTime endDate, [FromQuery, Required] List<string> logDirectories)
        {
            _logAnalyzerService.DeleteArchivesFromPeriod(startDate, endDate, logDirectories);
            return NoContent();
        }

        [HttpPost("upload-logs")]
        public async Task<IActionResult> UploadLogsToRemoteServer([FromQuery, Required] string apiUrl, [FromQuery, Required] string directory)
        {
            _logAnalyzerService.UploadLogsToRemoteServer(apiUrl, directory);
            return NoContent();
        }

        [HttpDelete("delete-logs-from-period")]
        public IActionResult DeleteLogsFromPeriod([FromQuery, Required] DateTime startDate, [FromQuery, Required] DateTime endDate, [FromQuery, Required] List<string> logDirectories)
        {
            _logAnalyzerService.DeleteLogsFromPeriod(startDate, endDate, logDirectories);
            return NoContent();
        }

        [HttpGet("count-total-logs")]
        public ActionResult<int> CountTotalLogsInPeriod([FromQuery, Required] DateTime startDate, [FromQuery, Required] DateTime endDate, [FromQuery, Required] List<string> logDirectories)
        {
            return Ok(_logAnalyzerService.CountTotalLogsInPeriod(startDate, endDate, logDirectories));
        }

        [HttpGet("search-logs-by-size")]
        public ActionResult<List<string>> SearchLogsBySize([FromQuery, Required] long minSizeInKb, [FromQuery, Required] long maxSizeInKb, [FromQuery, Required] List<string> logDirectories)
        {
            return Ok(_logAnalyzerService.SearchLogsBySize(minSizeInKb, maxSizeInKb, logDirectories));
        }

        [HttpGet("search-logs-by-directory")]
        public ActionResult<List<string>> SearchLogsByDirectory([FromQuery, Required] string directory, [FromQuery, Required] string logFileName)
        {
            var result = _logAnalyzerService.SearchLogsByDirectory(directory, logFileName);
            return Ok(result);
        }
    }
}