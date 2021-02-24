using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RPS.Data;

namespace RPS.Web
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
            var tempDataContext = new PtInMemoryContext();

            services.AddSingleton<IPtUserRepository, PtUserRepository>(c => new PtUserRepository(tempDataContext));
            services.AddSingleton<IPtItemsRepository, PtItemsRepository>(c => new PtItemsRepository(tempDataContext));
            services.AddSingleton<IPtDashboardRepository, PtDashboardRepository>(c => new PtDashboardRepository(tempDataContext));
            services.AddSingleton<IPtTasksRepository, PtTasksRepository>(c => new PtTasksRepository(tempDataContext));
            services.AddSingleton<IPtCommentsRepository, PtCommentsRepository>(c => new PtCommentsRepository(tempDataContext));

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddRazorPages()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AddPageRoute("/Dashboard", "");
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
                endpoints.MapRazorPages());
        }
    }
}
