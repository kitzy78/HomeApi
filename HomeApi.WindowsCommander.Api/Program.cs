using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.Logging;

namespace HomeApi.WindowsCommander.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var isDebugSession = Debugger.IsAttached || args.Contains("--console") || true;

            if (!isDebugSession)
            {
                var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                var pathToContentRoot = Path.GetDirectoryName(pathToExe);
                Directory.SetCurrentDirectory(pathToContentRoot);
            }

            var builder = CreateWebHostBuilder(args.Where(arg => arg != "--console").ToArray());
            var host = builder.Build();

            if (!isDebugSession)
            {
                host.RunAsService();
            }
            else
            {
                host.Run();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, logging) => { logging.AddEventLog(); })
                .ConfigureAppConfiguration((context, config) => { })
                .UseStartup<Startup>();
    }
}
