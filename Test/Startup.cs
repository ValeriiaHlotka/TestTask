using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Test.Models;
using AspNetCoreRateLimit;
using AspNetCoreRateLimit.Redis;
using System.Collections.Generic;
using System.Configuration;

namespace Test
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            services.AddMemoryCache();

            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));

            services.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimitPolicies"));

            services.AddInMemoryRateLimiting();
            
            services.AddMvc();

            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            string con = "Server=(localdb)\\mssqllocaldb;Database=dogsdbstore;Trusted_Connection=True;";
            services.AddDbContext<DogContext>(options => options.UseSqlServer(con));

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseIpRateLimiting();

            app.UseMvc();

            app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); 
            });
        }
    }
}