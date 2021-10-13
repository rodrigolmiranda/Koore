using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using KooreApi.Model;
using KooreApi.Business;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace KooreApi.Authorization
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;
        private ConnectionMultiplexer redis;
        private IDatabase redisDb;


        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            ConfigurationOptions conf = new ConfigurationOptions();
            conf.ConnectTimeout = 100;
            conf.EndPoints.Add(_appSettings.REDIS_URL, _appSettings.REDIS_PORT);
            conf.AbortOnConnectFail = false;
            redis = ConnectionMultiplexer.Connect(conf);
            redisDb = redis.GetDatabase();

            _next = next;
            
        }

        public async Task Invoke(HttpContext context, IAuth auth, IJwtUtils jwtUtils)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = jwtUtils.ValidateJwtToken(token);
            if (userId != null)
            {
                
                UserSession userSession;

                var redisUser = await redisDb.StringGetAsync(userId);
                if (redisUser.HasValue)
                {
                    userSession = JsonConvert.DeserializeObject<UserSession>(redisUser);
                    if (userSession.AccessToken != token)
                    {
                        await redisDb.KeyDeleteAsync(userId);
                    }
                    else
                    {
                        // attach user to context on successful jwt validation
                        context.Items["User"] = userSession;
                    }

                }
            }

            await _next(context);
        }
    }
}