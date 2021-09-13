using System;

namespace KooreApi
{
    public class audience_report
    {
        public Int64 id { get; set; }
        public Int64 campaign_id { get; set; }
        public DateTime stat_datetime { get; set; }
        public string ac { get; set; }
        public string age { get; set; }
        public string country_id { get; set; }
        public string interest_category { get; set; }
        public string interest_category_v2 { get; set; }
        public string gender { get; set; }
        public string language { get; set; }
        public string placement_id { get; set; }
        public string platform { get; set; }
        public double click_cnt { get; set; }
        public double conversion_cost { get; set; }
        public double conversion_rate { get; set; }
        public double convert_cnt { get; set; }
        public double ctr { get; set; }
        public double show_cnt { get; set; }
        public double stat_cost { get; set; }
        public double time_attr_convert_cnt { get; set; }
        public DateTime created_at { get; set; }

    }
    public class audience_report_monthly
    {
        public string stat_month_year { get; set; }
        public string stat_month { get; set; }
        public string stat_year { get; set; }
        public double click_cnt { get; set; }
        public double conversion_cost { get; set; }
        public double conversion_rate { get; set; }
        public double convert_cnt { get; set; }
        public double ctr { get; set; }
        public double show_cnt { get; set; }
        public double stat_cost { get; set; }
        public double time_attr_convert_cnt { get; set; }
    }
    public class audience_report_age
    {
        public string age { get; set; }
        public double click_cnt { get; set; }
        public double conversion_cost { get; set; }
        public double conversion_rate { get; set; }
        public double convert_cnt { get; set; }
        public double ctr { get; set; }
        public double show_cnt { get; set; }
        public double stat_cost { get; set; }
        public double time_attr_convert_cnt { get; set; }
    }
}
