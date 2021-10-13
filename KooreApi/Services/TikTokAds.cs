using KooreApi.Business;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TikTokAdsAPI.Model;

namespace KooreApi.Services
{
    public class TikTokAds
    {

        private readonly ILogger<TikTokAds> _logger;
        private readonly IConfiguration _config;

        public TikTokAds(ILogger<TikTokAds> logger, IConfiguration configuration)
        {
            _logger = logger;
            _config = configuration;
        }


        public async Task<string> GetAdvertisers()
        {
            try
            {
                string accessToken = "fca4fa8a5ae74321ba910ca7ba1c21706cdab7d4";
                string appId = "6998450446943649794";
                string secret = "e4c1c7d87d711d49662897802bbe8eb8d7aec8d2";
                TikTokAdsAPI.Api.AdvertiserApi _adv = new TikTokAdsAPI.Api.AdvertiserApi();

                _adv.Configuration.BasePath = "https://business-api.tiktok.com/open_api/";
                var _ret = _adv.GetAdvertiser(accessToken: accessToken, appId: appId, secret: secret);
                Ads _db = new Ads(null, _config);
                int retTotInc = 0;

                foreach (var _a in _ret.Data._List)
                {
                    retTotInc += _db.InsertAdvertiser((long)_a.AdvertiserId, _a.AdvertiserName);
                    GetCampaigns(accessToken, (long)_a.AdvertiserId);

                }
                return $"{retTotInc} rows included";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public async Task<string> GetCampaigns(string accessToken, long _advertiserID)
        {
            try
            {
                TikTokAdsAPI.Api.CampaignApi _camp = new TikTokAdsAPI.Api.CampaignApi();

                _camp.Configuration.BasePath = "https://business-api.tiktok.com/open_api/";
                var _ret = _camp.GetCampaign(accessToken: accessToken, advertiserId: _advertiserID);
                Ads _db = new Ads(null, _config);
                int retTotInc = 0;

                foreach (var _a in _ret.Data._List)
                {
                    retTotInc += _db.InsertCampaign((long)_a.CampaignId,_a.CampaignName,_a.CampaignType, (long)_a.AdvertiserId,_a.Budget,_a.BudgetMode,_a.Status,
                                                    _a.OptStatus,_a.Objective,_a.ObjectiveType, _a.BudgetOptimizeSwitch, _a.BidType,_a.OptimizeGoal,
                                                    _a.SplitTestVariable,_a.IsNewStructure,_a.CreateTime,_a.ModifyTime);
                }
                return $"{retTotInc} rows included";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public async Task<string> GetSyncAudienceReport()
        {
            try
            {
                Ads _db = new Ads(null, _config);


                var _campaigns = _db.GetUserCampaigns(null);

                //List<long?> CampaignIds = new List<long?> { 1710135894381586, 1710137408454705 };
                List<string> Dimensions = new List<string> { "campaign_id", "stat_time_day", "gender", "age" };
                List<string> Metrics = new List<string> { "campaign_name", "spend", "cpc", "cpm", "impressions", "clicks", "ctr", "conversion", "cost_per_conversion", "conversion_rate", "real_time_conversion", "real_time_cost_per_conversion", "real_time_conversion_rate", "result", "cost_per_result", "result_rate", "real_time_result", "real_time_cost_per_result", "real_time_result_rate" };

                TikTokAdsAPI.Api.ReportApi r = new TikTokAdsAPI.Api.ReportApi();

                r.Configuration.BasePath = "https://business-api.tiktok.com/open_api/";
                List<SyncReportList> _report = new List<SyncReportList>();
                SyncReportReturn _ret = new SyncReportReturn();

                foreach (var c in _campaigns)
                {
                    int page = 1;
                    do
                    {
                        _ret = r.GetReportSync(accessToken: c.access_token,
                                    advertiserId: c.advertiser_id,
                                    campaignIds: JsonConvert.SerializeObject(c.list),
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
                }
                var b = JsonConvert.SerializeObject(_report.Select(x => new { x.Metrics, x.Dimensions }));
                var metrics = _report.Select(x => x.Metrics).ToList().ToList();
                var dimensions = _report.Select(x => x.Dimensions).ToList().ToList();
                //var newList = ls.Zip(li, (a, c) => new { a, c }).ToList();
                //var newList2 = ls.Intersect(li).ToList();
                var metricsjson = JsonConvert.SerializeObject(metrics);
                var dimensionsjson = JsonConvert.SerializeObject(dimensions);
                //var cc = JsonConvert.SerializeObject(_report.Select(x => x.Dimensions));
                //var dd = aa.Concat(cc);

                //var aaa = _report.Select(x => x.Metrics);
                //var ccc = _report.Select(x => x.Dimensions);
                //var ddd = aaa.Union(ccc);

                //string _metricsCsv = CsvSerializer.SerializeToCsv(_report.Select(x => x.Metrics));
                ////var a = JsonConvert.SerializeObject(_report.Select(x => (x.Metrics, x.Dimensions)));
                //string _dimensionsCsv = CsvSerializer.SerializeToCsv(_report.Select(x => x.Dimensions));
                

                //var output = string.Join(System.Environment.NewLine, _dimensionsCsv.Split("\r\n", StringSplitOptions.RemoveEmptyEntries)
                //       .Zip(_metricsCsv.Split("\r\n", StringSplitOptions.RemoveEmptyEntries), (a, b) => string.Join(",", a, b)));//.Replace("\r\n", "\n\n").Replace("\r", "xxxxxxxxxxx").Replace("\n\n", "\r\n");


                //string filename = Path.GetFullPath($"Reports//{Guid.NewGuid()}.csv");
                //System.IO.File.WriteAllText(filename, output);


                int retBulk = _db.BulkInsertAudienceReport(metricsjson, dimensionsjson);
                //System.IO.File.Delete(filename);
                return $"{retBulk} rows included";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
