using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Npgsql;
using StackExchange.Redis;

using KooreApi.Model;
using KooreApi.Authorization;
using System.Threading;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace KooreApi.Business
{
    public interface IAuth
    {
        Task<bool> CheckEmailExists(string email);
        Task<string> SignUp(User user);
        Task<AuthenticateResponse> SignIn(UserSession user);
        Task<UserSession> GetById(string id);
        Task<AuthenticateResponse> RefreshToken(string refreshToken);
        Task<bool> DeleteToken(string refreshToken);
        Task<bool> SendPasswordEmail(string email);
        Task<bool> ResetPassword(string id, string token, string password);
    }

    public class Auth : IAuth
    {
        private readonly IConfiguration _config;


        private ConnectionMultiplexer redis;
        private IDatabase redisDb;

        private UserSession _user;
        private IJwtUtils _jwtUtils;
        private readonly AppSettings _appSettings;


        public Auth(UserSession user,
            IJwtUtils jwtUtils,
            IOptions<AppSettings> appSettings,
            IConfiguration configuration)
        {
            try
            {
                _config = configuration;

                _user = user;
                _jwtUtils = jwtUtils;
                _appSettings = appSettings.Value;

                ConfigurationOptions conf = new ConfigurationOptions();
                conf.ConnectTimeout = 100;
                conf.EndPoints.Add(_appSettings.REDIS_URL, _appSettings.REDIS_PORT);
                conf.AbortOnConnectFail = false;
                redis = ConnectionMultiplexer.Connect(conf);
                redisDb = redis.GetDatabase();

                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> CheckEmailExists(string email)
        {
            try
            {
                UserSession usr = await GetByEmail<UserSession>(email);
                if (usr is null)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<T> GetByEmail<T>(string email)
        {
            try
            {
                using var con = new NpgsqlConnection(_config.GetConnectionString("KooreDB"));
                con.Open();

                var sql = "select get_user(pemail:= @pemail::varchar)";

                using var cmd = new NpgsqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@pemail", email);

                var dr = cmd.ExecuteScalar();
                if (dr == null)
                    return default(T);

                var userRet = JsonConvert.DeserializeObject<T>(dr.ToString());

                return userRet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> SignUp(User user)
        {
            try
            {

                var salt = System.Web.Helpers.Crypto.GenerateSalt();
                var hashPassword = System.Web.Helpers.Crypto.HashPassword(user.Password + salt);
                user.Password = hashPassword;
                user.Salt = salt;

                using var con = new NpgsqlConnection(_config.GetConnectionString("KooreDB"));
                con.Open();
                var sql = "select post_insert_user(pemail:= @pemail::varchar, ppassword:= @ppassword::varchar, psalt:= @psalt::varchar, pfirst_name:= @pfirst_name::varchar, plast_name:= @plast_name::varchar)";

                using var cmd = new NpgsqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@pemail", user.Email);
                cmd.Parameters.AddWithValue("@ppassword", user.Password);
                cmd.Parameters.AddWithValue("@psalt", user.Salt);
                cmd.Parameters.AddWithValue("@pfirst_name", user.Profile.first_name);
                cmd.Parameters.AddWithValue("@plast_name", user.Profile.first_name);

                string dr = cmd.ExecuteScalar().ToString();

                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<AuthenticateResponse> SignIn(UserSession user)
        {
            try
            {
                var userRet = await GetByEmail<UserSession>(user.Email);

                var validaPwd = System.Web.Helpers.Crypto.VerifyHashedPassword(userRet.Password, user.Password + userRet.Salt);

                if (validaPwd)
                {
                    var jwtToken = _jwtUtils.GenerateJwtToken(userRet.Id);
                    var refreshToken = _jwtUtils.GenerateRefreshToken(userRet.Id);

                    userRet.AccessToken = jwtToken;
                    userRet.RefreshToken = refreshToken;
                    await redisDb.StringSetAsync(userRet.Id, JsonConvert.SerializeObject(userRet), expiry: TimeSpan.FromDays(_appSettings.RefreshTokenTTL));

                    AuthenticateResponse ret = new AuthenticateResponse(jwtToken, refreshToken);
                    return ret;
                }
                else
                {
                    throw new AppException("Invalid combination of email/password.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<AuthenticateResponse> RefreshToken(string refreshToken)
        {
            try
            {
                string id = _jwtUtils.ValidateRefreshToken(refreshToken);
                UserSession userSession;

                var redisUser = await redisDb.StringGetAsync(id);
                if (redisUser.HasValue)
                {
                    userSession = JsonConvert.DeserializeObject<UserSession>(redisUser);
                    if (userSession.RefreshToken != refreshToken)
                    {
                        await redisDb.KeyDeleteAsync(id);
                        throw new UnauthorizedAccessException("Unauthorized");
                    }
                }
                else
                {
                    throw new UnauthorizedAccessException("Unauthorized");
                }
                if (id is not null && id.Length > 0)
                {
                    var jwtToken = _jwtUtils.GenerateJwtToken(id);
                    refreshToken = _jwtUtils.GenerateRefreshToken(id);

                    userSession = JsonConvert.DeserializeObject<UserSession>(await redisDb.StringGetAsync(id));
                    userSession.AccessToken = jwtToken;
                    userSession.RefreshToken = refreshToken;

                    await redisDb.StringSetAsync(id, JsonConvert.SerializeObject(userSession), expiry: TimeSpan.FromDays(_appSettings.RefreshTokenTTL));

                    AuthenticateResponse ret = new AuthenticateResponse(jwtToken, refreshToken);
                    return ret;
                }
                else
                {
                    throw new UnauthorizedAccessException("Unauthorized");
                }
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> DeleteToken(string refreshToken)
        {
            try
            {
                string id = _jwtUtils.ValidateRefreshToken(refreshToken);
                UserSession userSession;

                var redisUser = await redisDb.StringGetAsync(id);
                if (redisUser.HasValue)
                {
                    await redisDb.KeyDeleteAsync(id);
                }
                else
                {
                    throw new UnauthorizedAccessException("Unauthorized");
                }

                return true;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> SendPasswordEmail(string email)
        {
            try
            {
                var userRet = await GetByEmail<User>(email);
                if (userRet is null)
                    throw new KeyNotFoundException();

                var jwtToken = _jwtUtils.GenerateTempToken(userRet.Id);
                string url = _appSettings.PASS_RESET_URL + $"{userRet.Id}/{jwtToken}";

                var subject = "Koore Password Reset";

                var htmlContent = @$"    <p>Hi {userRet.Profile.first_name},</p>
                                  <p>You can use the following link to reset your password:</p>
                                  <a href={url}>{url}</a>
                                  <p>Please be aware that this link is valid for 30 mins only.</p>
                                  <p>–Thank you very much, Koore</p>";

                await redisDb.StringSetAsync(userRet.Id, jwtToken, expiry: TimeSpan.FromDays(_appSettings.RefreshTokenTTL));

                await KooreApi.Email.Execute(subject, userRet.Profile.first_name, email, htmlContent);


                return true;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> ResetPassword(string id, string token, string password)
        {
            try
            {
                var idToken = _jwtUtils.ValidateTempToken(token, id);
                if (id != idToken)
                    throw new KeyNotFoundException();

                var userRet = await GetById(id);
                if (userRet is null)
                    throw new KeyNotFoundException();

                var salt = System.Web.Helpers.Crypto.GenerateSalt();
                var hashPassword = System.Web.Helpers.Crypto.HashPassword(password + salt);
                userRet.Password = hashPassword;
                userRet.Salt = salt;

                using var con = new NpgsqlConnection(_config.GetConnectionString("KooreDB"));
                con.Open();
                var sql = "select post_reset_password(pid:= @pid::uuid, ppassword:= @ppassword::varchar, psalt:= @psalt::varchar)";

                using var cmd = new NpgsqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@pid", userRet.Id);
                cmd.Parameters.AddWithValue("@ppassword", userRet.Password);
                cmd.Parameters.AddWithValue("@psalt", userRet.Salt);


                string dr = cmd.ExecuteScalar().ToString();
                if (dr == "1")
                {
                    await redisDb.KeyDeleteAsync(id);
                    return true;
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
            catch
            {
                throw;
            }
        }
        public async Task<UserSession> GetById(string id)
        {
            try
            {
                using var con = new NpgsqlConnection(_config.GetConnectionString("KooreDB"));
                con.Open();

                var sql = "select get_user(pid:= @pid::uuid)";
                using var cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@pid", id);

                string dr = cmd.ExecuteScalar().ToString();
                var userRet = JsonConvert.DeserializeObject<UserSession>(dr);

                return userRet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
