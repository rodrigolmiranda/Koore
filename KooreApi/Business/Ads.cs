using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Npgsql;

namespace KooreApi.Business
{
    public class Ads
    {
        public List<audience_report> GetAudienceReports(string strCon, string pstat_time_day_min, string pstat_time_day_max)
        {
            try
            {
                using var con = new NpgsqlConnection(strCon);
                con.Open();

                var sql = "select get_audience_report(pstat_time_day_min:= @pstat_time_day_min::timestamp, pstat_time_day_max:= @pstat_time_day_max::timestamp)";

                using var cmd = new NpgsqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@pstat_time_day_min", pstat_time_day_min);
                cmd.Parameters.AddWithValue("@pstat_time_day_max", pstat_time_day_max);

                string dr = cmd.ExecuteScalar().ToString();

                return JsonConvert.DeserializeObject<List<audience_report>>(dr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<audience_report_monthly> GetAudienceReportsMonthly(string strCon, string pstat_time_day_min, string pstat_time_day_max)
        {
            try
            {
                using var con = new NpgsqlConnection(strCon);
                con.Open();

                var sql = "select get_audience_report_monthly(pstat_time_day_min:= @pstat_time_day_min::timestamp, pstat_time_day_max:= @pstat_time_day_max::timestamp)";

                using var cmd = new NpgsqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@pstat_time_day_min", pstat_time_day_min);
                cmd.Parameters.AddWithValue("@pstat_time_day_max", pstat_time_day_max);

                string dr = cmd.ExecuteScalar().ToString();

                return JsonConvert.DeserializeObject<List<audience_report_monthly>>(dr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<audience_report_age> GetAudienceReportsAge(string strCon, string pstat_time_day_min, string pstat_time_day_max)
        {
            try
            {
                using var con = new NpgsqlConnection(strCon);
                con.Open();

                var sql = "select get_audience_report_age(pstat_time_day_min:= @pstat_time_day_min::timestamp, pstat_time_day_max:= @pstat_time_day_max::timestamp)";

                using var cmd = new NpgsqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@pstat_time_day_min", pstat_time_day_min);
                cmd.Parameters.AddWithValue("@pstat_time_day_max", pstat_time_day_max);

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
                var sql = $"call import_audience_reports (@_report, @newLines)";
                using var cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("_report", _report);
                cmd.Parameters.Add(new NpgsqlParameter("newLines", DbType.Int32) { Direction = ParameterDirection.InputOutput });
                cmd.Parameters[1].Value = 0;

                int qtdRet = cmd.ExecuteNonQuery();
                return int.Parse(cmd.Parameters[1].Value.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
