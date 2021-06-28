using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace GoodNewsAggregator.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Logger(lc =>
                    lc.Filter.ByIncludingOnly(evt => evt.Level == LogEventLevel.Information)
                        .WriteTo.File(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\LogsDebug\\log.log"))
                .WriteTo.File(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Logs\\log.log", LogEventLevel.Warning)
                .CreateLogger();

            Log.Information("Starting web api host");

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
