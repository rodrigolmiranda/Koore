using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Npgsql;

namespace KooreApi.Business
{
    public class Ads
    {
        public List<audience_report> GetAudienceReports(string strCon, string pstat_datetime_min, string pstat_datetime_max)
        {
            try
            {
                using var con = new NpgsqlConnection(strCon);
                con.Open();

                var sql = "select get_audience_report(pstat_datetime_min:= @pstat_datetime_min::timestamp, pstat_datetime_max:= @pstat_datetime_max::timestamp)";

                using var cmd = new NpgsqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@pstat_datetime_min", pstat_datetime_min);
                cmd.Parameters.AddWithValue("@pstat_datetime_max", pstat_datetime_max);

                string dr = cmd.ExecuteScalar().ToString();

                return JsonConvert.DeserializeObject<List<audience_report>>(dr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<audience_report_monthly> GetAudienceReportsMonthly(string strCon, string pstat_datetime_min, string pstat_datetime_max)
        {
            try
            {
                using var con = new NpgsqlConnection(strCon);
                con.Open();

                var sql = "select get_audience_report_monthly(pstat_datetime_min:= @pstat_datetime_min::timestamp, pstat_datetime_max:= @pstat_datetime_max::timestamp)";

                using var cmd = new NpgsqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@pstat_datetime_min", pstat_datetime_min);
                cmd.Parameters.AddWithValue("@pstat_datetime_max", pstat_datetime_max);

                string dr = cmd.ExecuteScalar().ToString();

                return JsonConvert.DeserializeObject<List<audience_report_monthly>>(dr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<audience_report_age> GetAudienceReportsAge(string strCon, string pstat_datetime_min, string pstat_datetime_max)
        {
            try
            {
                using var con = new NpgsqlConnection(strCon);
                con.Open();

                var sql = "select get_audience_report_age(pstat_datetime_min:= @pstat_datetime_min::timestamp, pstat_datetime_max:= @pstat_datetime_max::timestamp)";

                using var cmd = new NpgsqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@pstat_datetime_min", pstat_datetime_min);
                cmd.Parameters.AddWithValue("@pstat_datetime_max", pstat_datetime_max);

                string dr = cmd.ExecuteScalar().ToString();

                return JsonConvert.DeserializeObject<List<audience_report_age>>(dr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int BulkInsertAudienceReport(string strCon, string _report)
        {
            try
            {
                using var con = new NpgsqlConnection(strCon);
                con.Open();

                //var sql = $"copy reports_async FROM '{_report}' DELIMITER ',' CSV HEADER";
                var sql = $"call import_audience_reports (@_report)";
                using var cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@_report", _report);

                int qtdRet = cmd.ExecuteNonQuery();
                return qtdRet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
