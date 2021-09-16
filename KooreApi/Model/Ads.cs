using System;

namespace KooreApi
{
    public class audience_report
    {
        public Int64 id { get; set; }
        public Int64 campaign_id { get; set; }
        public DateTime stat_time_day { get; set; }
        public string age { get; set; }
        public string gender { get; set; }
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
        public DateTime created_at { get; set; }

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
