using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KooreApi.Business;
using KooreApi.Model;
using KooreApi.Authorization;

namespace KooreApi.Controllers
{
    [Authorize]
    public class AdsController : ControllerBase
    {
        private readonly ILogger<AdsController> _logger;
        private readonly IConfiguration _config;

        public AdsController(ILogger<AdsController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _config = configuration;
        }

        [HttpGet]
        [Route("/api/v1/[controller]/report/audience")]
        [ProducesResponseType(typeof(audience_report), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(DateTime dt_min, DateTime dt_max)
        {
            try
            {
                Ads _db = new Ads(null, _config);
                string stat_min = null, stat_max = null;
                if (dt_min == DateTime.MinValue)
                    throw new Exception("pstat_min is blank or invalid");
                else
                    stat_min = dt_min.ToString("yyyy-MM-dd HH:mm:ss");
                if (dt_max == DateTime.MinValue)
                    throw new Exception("pstat_min is blank or invalid");
                else
                    stat_max = dt_max.ToString("yyyy-MM-dd HH:mm:ss");
                audience_report _audience_reports = _db.GetAudienceReports(stat_min, stat_max);

                return StatusCode(200, _audience_reports);
                //return Ok(_audience_reports);
            }
            catch (Exception ex)
            {
                return (StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
    }
}
