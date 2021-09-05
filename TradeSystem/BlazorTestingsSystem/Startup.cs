using BlazorTestingsSystem.DataServices;
using BlazorTestingsSystem.Strategies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StrategyTester.Models;
using Syncfusion.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace BlazorTestingsSystem
{
    public class Startup
    {
        public Startup(IConfiguration configuration,IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR(e =>
            {
                e.MaximumReceiveMessageSize = 102400000;
            });

            services.AddRazorPages();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddServerSideBlazor().AddCircuitOptions(o =>
            {
                if (_env.IsDevelopment()) //only add details when debugging
                {
                    o.DetailedErrors = true;
                }
            });
            services.AddSyncfusionBlazor();
            //services.AddSingleton(ConfigureStrategies());
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

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }

        //private static SettingsDataService ConfigureStrategies()
        //{
        //    //var client = new BinanceClient();
        //    var settings = new SettingsDataService();
        //    settings.StrategiesDict = new Dictionary<string, BaseStrategy>();
        //    settings.StrategiesDict.Add("TwoEma", new TwoEmaStrategy());
        //    return settings;
        //}
    }
}
