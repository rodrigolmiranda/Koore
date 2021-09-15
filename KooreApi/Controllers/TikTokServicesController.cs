using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KooreApi.Business;
using TikTokAdsAPI.Api;
using TikTokAdsAPI.Model;
using TikTokAdsAPI.Client;
using Newtonsoft.Json;
using ServiceStack.Text;
using System.IO;

namespace KooreApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TikTokServicesController : ControllerBase
    {
        private readonly ILogger<KooreApiController> _logger;
        private readonly IConfiguration _config;

        public TikTokServicesController(ILogger<KooreApiController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _config = configuration;
        }

        [Route("[controller]/syncReport/Audience")]
        [HttpGet]
        [ProducesResponseType(typeof(List<audience_report>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            try
            {
                List<long?> CampaignIds = new List<long?> { 1710135894381586, 1710137408454705 };
                List<string> Dimensions = new List<string> { "campaign_id", "stat_time_day", "gender", "age" };
                List<string> Metrics = new List<string> { "campaign_name", "spend", "cpc", "cpm", "impressions", "clicks", "ctr", "conversion", "cost_per_conversion", "conversion_rate", "real_time_conversion", "real_time_cost_per_conversion", "real_time_conversion_rate", "result", "cost_per_result", "result_rate", "real_time_result", "real_time_cost_per_result", "real_time_result_rate" };

                TikTokAdsAPI.Api.ReportApi r = new TikTokAdsAPI.Api.ReportApi();

                r.Configuration.BasePath = "https://business-api.tiktok.com/open_api/";
                List<SyncReportList> _report = new List<SyncReportList>();
                SyncReportReturn _ret = new SyncReportReturn();

                int page = 1;
                do
                {
                    _ret = r.GetReportSync(accessToken: "fca4fa8a5ae74321ba910ca7ba1c21706cdab7d4",
                                advertiserId: 6999016508227584002,
                                campaignIds: JsonConvert.SerializeObject(CampaignIds),
                                startDate: DateTime.Now.AddDays(-21).ToString("yyyy-MM-dd"),
                                endDate: DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"),
                                reportType: AsyncReportBody.ReportTypeEnum.AUDIENCE.ToString(),
                                dataLevel: "AUCTION_CAMPAIGN",
                                dimensions: JsonConvert.SerializeObject(Dimensions),
                                metrics: JsonConvert.SerializeObject(Metrics),
                                pageSize: 1000,
                                page: page);

                    _report.AddRange(_ret.Data._List);
                    page += 1;

                } while (_ret.Data.PageInfo.Page < _ret.Data.PageInfo.TotalPage);

                string _metricsCsv = CsvSerializer.SerializeToCsv(_report.Select(x => x.Metrics));
                string _dimensionsCsv = CsvSerializer.SerializeToCsv(_report.Select(x => x.Dimensions));

                var output = string.Join(System.Environment.NewLine, _dimensionsCsv.Split("\r\n", StringSplitOptions.RemoveEmptyEntries)
                       .Zip(_metricsCsv.Split("\r\n", StringSplitOptions.RemoveEmptyEntries), (a, b) => string.Join(",", a, b)));//.Replace("\r\n", "\n\n").Replace("\r", "xxxxxxxxxxx").Replace("\n\n", "\r\n");

                
                string filename = Path.GetFullPath($"Reports//{Guid.NewGuid()}.csv");
                System.IO.File.WriteAllText(filename, output);
                
                Ads _ads = new Ads();
                int retBulk = _ads.BulkInsertAudienceReport(_config.GetConnectionString("KooreDB"), filename);
                System.IO.File.Delete(filename);
                return StatusCode(200, $"{retBulk} rows included");
            }
            catch (Exception ex)
            {
                return (StatusCode(400, ex.Message));
            }
        }

    }
}
