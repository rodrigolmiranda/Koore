using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Npgsql;

namespace KooreApi.Business
{
    public class DB
    {
        private readonly ILogger<DB> _logger;
        private readonly IConfiguration _config;

        public DB(ILogger<DB> logger, IConfiguration configuration)
        {
            _logger = logger;
            _config = configuration;
        }

        public List<Campaigns> GetUserCampaigns(Int64? user_id)
        {
            try
            {
                using var con = new NpgsqlConnection(_config.GetConnectionString("KooreDB"));
                con.Open();

                var sql = "select get_user_campaigns(@puser_id)";

                using var cmd = new NpgsqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@puser_id", NpgsqlTypes.NpgsqlDbType.Uuid , user_id == null ? DBNull.Value : user_id);

                string dr = cmd.ExecuteScalar().ToString();

                return JsonConvert.DeserializeObject<List<Campaigns>>(dr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public audience_report GetAudienceReports(string pstat_time_day_min, string pstat_time_day_max)
        {
            try
            {
                using var con = new NpgsqlConnection(_config.GetConnectionString("KooreDB"));
                con.Open();

                var sql = "select get_audience_report(pstat_time_day_min:= @pstat_time_day_min::timestamp, pstat_time_day_max:= @pstat_time_day_max::timestamp)";

                using var cmd = new NpgsqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@pstat_time_day_min", pstat_time_day_min);
                cmd.Parameters.AddWithValue("@pstat_time_day_max", pstat_time_day_max);

                string dr = cmd.ExecuteScalar().ToString();

                return JsonConvert.DeserializeObject<audience_report>(dr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
   
        public int BulkInsertAudienceReport(string _report)
        {
            try
            {
                using var con = new NpgsqlConnection(_config.GetConnectionString("KooreDB"));
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
        public int InsertAdvertiser(Int64 padvertiser_id, string padvertiser_name)
        {
            try
            {
                using var con = new NpgsqlConnection(_config.GetConnectionString("KooreDB"));
                con.Open();

                //var sql = $"copy reports_async FROM '{_report}' DELIMITER ',' CSV HEADER";
                var sql = $"call insert_update_advertiser (@padvertiser_id, @padvertiser_name, @newLines)";
                using var cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.Add(new NpgsqlParameter("newLines", DbType.Int32) { Direction = ParameterDirection.InputOutput });
                cmd.Parameters.AddWithValue("padvertiser_id", padvertiser_id);
                cmd.Parameters.AddWithValue("padvertiser_name", padvertiser_name);
                
                cmd.Parameters[0].Value = 0;

                int qtdRet = cmd.ExecuteNonQuery();
                return int.Parse(cmd.Parameters[0].Value.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int InsertCampaign(Int64 pcampaign_id, string pcampaign_name, string pcampaign_type, Int64 padvertiser_id, float? pbudget, string pbudget_mode,
                                    string pstatus, string popt_status, string pobjective, string pobjective_type, int? pbudget_optimize_switch, int? pbid_type,
                                    string poptimize_goal, string psplit_test_variable, bool? pis_new_structure, string pcreate_time, string pmodify_time)
        {
            try
            {
                using var con = new NpgsqlConnection(_config.GetConnectionString("KooreDB"));
                con.Open();

                var sql = $"call insert_update_campaign (@pcampaign_id,@pcampaign_name,@pcampaign_type,@padvertiser_id,@pbudget,@pbudget_mode," +
                            $"@pstatus,@popt_status,@pobjective,@pobjective_type,@pbudget_optimize_switch,@pbid_type,@poptimize_goal," +
                            $"@psplit_test_variable,@pis_new_structure,@pcreate_time,@pmodify_time,@newLines)";
                
                using var cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.Add(new NpgsqlParameter("newLines", DbType.Int32) { Direction = ParameterDirection.InputOutput });
                cmd.Parameters.AddWithValue("pcampaign_id", NpgsqlTypes.NpgsqlDbType.Bigint, pcampaign_id);
                cmd.Parameters.AddWithValue("pcampaign_name", NpgsqlTypes.NpgsqlDbType.Varchar, pcampaign_name);
                cmd.Parameters.AddWithValue("pcampaign_type", NpgsqlTypes.NpgsqlDbType.Varchar, pcampaign_type == null ? DBNull.Value : pcampaign_type);
                cmd.Parameters.AddWithValue("padvertiser_id", NpgsqlTypes.NpgsqlDbType.Bigint, padvertiser_id);
                cmd.Parameters.AddWithValue("pbudget", pbudget == null ? DBNull.Value : pbudget);
                cmd.Parameters.AddWithValue("pbudget_mode", NpgsqlTypes.NpgsqlDbType.Varchar, pbudget_mode == null ? DBNull.Value : pbudget_mode);
                cmd.Parameters.AddWithValue("pstatus", NpgsqlTypes.NpgsqlDbType.Varchar, pstatus == null ? DBNull.Value : pstatus);
                cmd.Parameters.AddWithValue("popt_status", NpgsqlTypes.NpgsqlDbType.Varchar, popt_status == null ? DBNull.Value : popt_status);
                cmd.Parameters.AddWithValue("pobjective", NpgsqlTypes.NpgsqlDbType.Varchar, pobjective == null ? DBNull.Value : pobjective);
                cmd.Parameters.AddWithValue("pobjective_type", NpgsqlTypes.NpgsqlDbType.Varchar, pobjective_type == null ? DBNull.Value : pobjective_type);
                cmd.Parameters.AddWithValue("pbudget_optimize_switch", NpgsqlTypes.NpgsqlDbType.Integer, pbudget_optimize_switch==null ? DBNull.Value: pbudget_optimize_switch);
                cmd.Parameters.AddWithValue("pbid_type", NpgsqlTypes.NpgsqlDbType.Integer, pbid_type == null ? DBNull.Value : pbid_type);
                cmd.Parameters.AddWithValue("poptimize_goal", NpgsqlTypes.NpgsqlDbType.Varchar, poptimize_goal == null ? DBNull.Value : poptimize_goal);
                cmd.Parameters.AddWithValue("psplit_test_variable", NpgsqlTypes.NpgsqlDbType.Varchar, psplit_test_variable == null ? DBNull.Value : psplit_test_variable);
                cmd.Parameters.AddWithValue("pis_new_structure", NpgsqlTypes.NpgsqlDbType.Boolean, pis_new_structure == null ? DBNull.Value : pis_new_structure);
                cmd.Parameters.AddWithValue("pcreate_time", NpgsqlTypes.NpgsqlDbType.Timestamp, DateTime.Parse(pcreate_time));
                cmd.Parameters.AddWithValue("pmodify_time", NpgsqlTypes.NpgsqlDbType.Timestamp, DateTime.Parse(pmodify_time));
                
                cmd.Parameters[0].Value = 0;

                int qtdRet = cmd.ExecuteNonQuery();
                return int.Parse(cmd.Parameters[0].Value.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
