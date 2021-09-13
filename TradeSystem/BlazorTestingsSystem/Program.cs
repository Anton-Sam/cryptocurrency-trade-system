using BlazorTestingsSystem.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace BlazorTestingsSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDc1OTA4QDMxMzkyZTMyMmUzMERaSERFMzhJdkRvVlN2cVlHT0t0WFVVczQvQmxvNU9PNzRLM3VqYjlEaWc9");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>

            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .ConfigureLogging((hostBuilderContext, logging) =>
            {
                logging.AddFileLogger(options =>
                {
                    hostBuilderContext.Configuration.GetSection("Logging").GetSection("FileLogger").GetSection("Options").Bind(options);
                });
            });
    }
}
