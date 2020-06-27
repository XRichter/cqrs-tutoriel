using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Steeltoe.Common.Hosting;
using Steeltoe.Discovery.Client;
using Steeltoe.Extensions.Configuration.CloudFoundry;

namespace Formation.CQRS.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseCloudHosting(5000)
                .AddCloudFoundry()
                .AddServiceDiscovery();
    }
}
