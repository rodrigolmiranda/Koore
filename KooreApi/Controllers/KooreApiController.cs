using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KooreApi.Business;
namespace KooreApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KooreApiController : ControllerBase
    {
        private readonly ILogger<KooreApiController> _logger;
        private readonly IConfiguration _config;

        public KooreApiController(ILogger<KooreApiController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _config = configuration;
        }

        [Route("[controller]/report/audience")]
        [HttpGet]
        [ProducesResponseType(typeof(List<audience_report>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(DateTime dt_min, DateTime dt_max)
        {
            try
            {
                Ads _ads = new Ads();
                string stat_min = null, stat_max = null;
                if (dt_min == DateTime.MinValue)
                    throw new Exception("pstat_min is blank or invalid");
                else
                    stat_min = dt_min.ToString("yyyy-MM-dd HH:mm:ss");
                if (dt_max == DateTime.MinValue)
                    throw new Exception("pstat_min is blank or invalid");
                else
                    stat_max = dt_max.ToString("yyyy-MM-dd HH:mm:ss");
                List<audience_report> _audience_reports = _ads.GetAudienceReports(_config.GetConnectionString("KooreDB"), stat_min, stat_max);

                return StatusCode(200, _audience_reports);
                //return Ok(_audience_reports);
            }
            catch (Exception ex)
            {
                return (StatusCode(400, ex.Message));
            }
        }
        [Route("[controller]/report/audience/month")]
        [HttpGet]
        [ProducesResponseType(typeof(List<audience_report>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMonth(DateTime dt_min, DateTime dt_max)
        {
            try
            {
                Ads _ads = new Ads();
                string stat_min = null, stat_max = null;
                if (dt_min == DateTime.MinValue)
                    throw new Exception("pstat_min is blank or invalid");
                else
                    stat_min = dt_min.ToString("yyyy-MM-dd HH:mm:ss");
                if (dt_max == DateTime.MinValue)
                    throw new Exception("pstat_min is blank or invalid");
                else
                    stat_max = dt_max.ToString("yyyy-MM-dd HH:mm:ss");
                List<audience_report_monthly> _audience_reports = _ads.GetAudienceReportsMonthly(_config.GetConnectionString("KooreDB"), stat_min, stat_max);

                return StatusCode(200, _audience_reports);
                //return Ok(_audience_reports);
            }
            catch (Exception ex)
            {
                return (StatusCode(400, ex.Message));
            }
        }
        [Route("[controller]/report/audience/age")]
        [HttpGet]
        [ProducesResponseType(typeof(List<audience_report>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAge(DateTime dt_min, DateTime dt_max)
        {
            try
            {
                Ads _ads = new Ads();
                string stat_min = null, stat_max = null;
                if (dt_min == DateTime.MinValue)
                    throw new Exception("pstat_min is blank or invalid");
                else
                    stat_min = dt_min.ToString("yyyy-MM-dd HH:mm:ss");
                if (dt_max == DateTime.MinValue)
                    throw new Exception("pstat_min is blank or invalid");
                else
                    stat_max = dt_max.ToString("yyyy-MM-dd HH:mm:ss");
                List<audience_report_age> _audience_reports = _ads.GetAudienceReportsAge(_config.GetConnectionString("KooreDB"), stat_min, stat_max);

                return StatusCode(200, _audience_reports);
                //return Ok(_audience_reports);
            }
            catch (Exception ex)
            {
                return (StatusCode(400, ex.Message));
            }
        }
    }
}
