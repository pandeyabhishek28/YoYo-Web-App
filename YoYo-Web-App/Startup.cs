using MatBlazor;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using System.IO;
using YoYo_Web_App.BusinessLogic.Managers;
using YoYo_Web_App.BusinessLogic.Managers.Contracts;
using YoYo_Web_App.DAL.Repositories;
using YoYo_Web_App.DAL.Services;
using YoYo_Web_App.DAL.Services.Contracts;
using YoYo_Web_App.Domain.Configurations;
using YoYo_Web_App.Domain.Repositories;

namespace YoYo_Web_App
{
    public class Startup
    {
        private readonly IWebHostEnvironment env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            this.env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppConfiguration>(Configuration.GetSection(AppConfiguration.AppConfigKey));
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddMatBlazor();

            services.AddTransient<IFitnessRatingRepository, FitnessRatingRepository>(sp =>
            {
                var appSettings = sp.GetRequiredService<IOptions<AppConfiguration>>();

                var dataFileAbsolutePath = Path.Combine(env.ContentRootPath,
                    appSettings.Value.DataFilePath, appSettings.Value.DataFileName);
                if (!File.Exists(dataFileAbsolutePath))
                {
                    throw new FileNotFoundException(appSettings.Value.DataFileName);
                }
                return new FitnessRatingRepository(dataFileAbsolutePath);
            });
            services.AddSingleton<IFitnessRatingService, FitnessRatingService>();
            services.AddSingleton<IDummyDataService, DummyDataService>();
            services.AddSingleton<ITestManager, TestManager>();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSerilogRequestLogging();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
