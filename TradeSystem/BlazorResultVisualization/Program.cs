using Binance.Net;
using BlazorResultVisualization.DataServices;
using BlazorResultVisualization.Strategies;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using StrategyTester.Models;
using Syncfusion.Blazor;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorResultVisualization
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDc1OTA4QDMxMzkyZTMyMmUzMERaSERFMzhJdkRvVlN2cVlHT0t0WFVVczQvQmxvNU9PNzRLM3VqYjlEaWc9");

            //builder.Services.AddHttpClient<IDataService, RESTDataService>
            //    (spds => spds.BaseAddress = new Uri(builder.Configuration["api_base_url"]));

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddSingleton(ConfigureStrategies());

            builder.Services.AddSyncfusionBlazor();

            await builder.Build().RunAsync();
        }

        private static SettingsDataService ConfigureStrategies()
        {
            var client = new BinanceClient();
            var settings = new SettingsDataService();
            settings.StrategiesDict = new Dictionary<string, BaseStrategy>();
            settings.StrategiesDict.Add("TwoEma", new TwoEmaStrategy());
            return settings;
        }
    }
}
