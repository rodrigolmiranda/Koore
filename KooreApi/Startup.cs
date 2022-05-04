using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Hangfire.PostgreSql;
using Hangfire;
using KooreApi.Authorization;
using KooreApi.Model;
using KooreApi.Business;

namespace KooreApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkNpgsql().AddDbContext<DefaultDbContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("HangfireDB"));
            });
            services.AddHangfire(x => x.UsePostgreSqlStorage(Configuration.GetConnectionString("HangfireDB")));
            
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "KooreApi", Version = "v1" });
            });


            // configure strongly typed settings object
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            // configure DI for application services
            services.AddScoped<IJwtUtils, JwtUtils>();
            services.AddScoped<IAuth, Auth>();
            services.AddScoped<UserSession, UserSession>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("MyPolicy");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "KooreApi v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthorization();

            // global error handler
            app.UseMiddleware<ErrorHandlerMiddleware>();

            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseHangfireDashboard();
            app.UseHangfireServer();

            //JOBS
            //TikTok Ads
            Services.TikTokAds _ads = new Services.TikTokAds(null, Configuration);
            RecurringJob.AddOrUpdate("Koore.AudienceReport", () => _ads.GetSyncAudienceReport(), Cron.Daily(9));
            RecurringJob.AddOrUpdate("Koore.Campaigns", () => _ads.GetAdvertisers(), Cron.Daily(8));

            //RecurringJob.RemoveIfExists("teste");

        }
    }
}