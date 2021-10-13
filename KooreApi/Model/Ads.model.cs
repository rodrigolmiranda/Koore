using System;
using System.Collections.Generic;

namespace KooreApi.Model
{
    public class Campaigns
    {
        public Int64 user_id { get; set; }
        public Int64 advertiser_id { get; set; }
        public string access_token { get; set; }
        public List<long> list{ get; set; }
    }
    public class audience_report
    {
        public List<years> years { get; set; }
    }
    public class years
    { 
        public Int32 year { get; set; }
        public double spend { get; set; }
        public double cpc { get; set; }
        public double cpm { get; set; }
        public double impressions { get; set; }
        public double clicks { get; set; }
        public double ctr { get; set; }
        public double conversion { get; set; }
        public double cost_per_conversion { get; set; }
        public double conversion_rate { get; set; }
        public double real_time_conversion { get; set; }
        public double real_time_cost_per_conversion { get; set; }
        public double real_time_conversion_rate { get; set; }
        public double result { get; set; }
        public double cost_per_result { get; set; }
        public double result_rate { get; set; }
        public double real_time_result { get; set; }
        public double real_time_cost_per_result { get; set; }
        public double real_time_result_rate { get; set; }

        public double? ctr_trend { get; set; }
        public double? cpm_trend { get; set; }
        public double? cpc_trend { get; set; }
        public double? impressions_trend { get; set; }
        public List<months> months { get; set; }

    }
    public class months
    {
        public string month { get; set; }
        public double spend { get; set; }
        public double cpc { get; set; }
        public double cpm { get; set; }
        public double impressions { get; set; }
        public double clicks { get; set; }
        public double ctr { get; set; }
        public double conversion { get; set; }
        public double cost_per_conversion { get; set; }
        public double conversion_rate { get; set; }
        public double real_time_conversion { get; set; }
        public double real_time_cost_per_conversion { get; set; }
        public double real_time_conversion_rate { get; set; }
        public double result { get; set; }
        public double cost_per_result { get; set; }
        public double result_rate { get; set; }
        public double real_time_result { get; set; }
        public double real_time_cost_per_result { get; set; }
        public double real_time_result_rate { get; set; }
        public double? ctr_trend { get; set; }
        public double? cpm_trend { get; set; }
        public double? cpc_trend { get; set; }
        public double? impressions_trend { get; set; }
        public List<ages> ages { get; set; }
    }
    public class ages
    {
        public string age { get; set; }
        public double spend { get; set; }
        public double cpc { get; set; }
        public double cpm { get; set; }
        public double impressions { get; set; }
        public double clicks { get; set; }
        public double ctr { get; set; }
        public double conversion { get; set; }
        public double cost_per_conversion { get; set; }
        public double conversion_rate { get; set; }
        public double real_time_conversion { get; set; }
        public double real_time_cost_per_conversion { get; set; }
        public double real_time_conversion_rate { get; set; }
        public double result { get; set; }
        public double cost_per_result { get; set; }
        public double result_rate { get; set; }
        public double real_time_result { get; set; }
        public double real_time_cost_per_result { get; set; }
        public double real_time_result_rate { get; set; }
    }
    public class audience_report_monthly
    {
        public string stat_month_year { get; set; }
        public string stat_month { get; set; }
        public string stat_year { get; set; }
        public double spend { get; set; }
        public double cpc { get; set; }
        public double cpm { get; set; }
        public double impressions { get; set; }
        public double clicks { get; set; }
        public double ctr { get; set; }
        public double conversion { get; set; }
        public double cost_per_conversion { get; set; }
        public double conversion_rate { get; set; }
        public double real_time_conversion { get; set; }
        public double real_time_cost_per_conversion { get; set; }
        public double real_time_conversion_rate { get; set; }
        public double result { get; set; }
        public double cost_per_result { get; set; }
        public double result_rate { get; set; }
        public double real_time_result { get; set; }
        public double real_time_cost_per_result { get; set; }
        public double real_time_result_rate { get; set; }

    }
    public class audience_report_age
    {
        public string age { get; set; }
        public double spend { get; set; }
        public double cpc { get; set; }
        public double cpm { get; set; }
        public double impressions { get; set; }
        public double clicks { get; set; }
        public double ctr { get; set; }
        public double conversion { get; set; }
        public double cost_per_conversion { get; set; }
        public double conversion_rate { get; set; }
        public double real_time_conversion { get; set; }
        public double real_time_cost_per_conversion { get; set; }
        public double real_time_conversion_rate { get; set; }
        public double result { get; set; }
        public double cost_per_result { get; set; }
        public double result_rate { get; set; }
        public double real_time_result { get; set; }
        public double real_time_cost_per_result { get; set; }
        public double real_time_result_rate { get; set; }

    }
}
