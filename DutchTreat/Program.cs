using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DutchTreat.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DutchTreat
{
  public class Program
  {
    public static void Main(string[] args)
    {
            var host = BuildWebHost(args);
             RunSeeding(host);
            
            host.Run();
    }

        private static void  RunSeeding(IWebHost host)
        {
            var scopfactor = host.Services.GetService<IServiceScopeFactory>();

            using (var scope=scopfactor.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetService<DutchSeeder>();
                seeder.SeedAsync().Wait(); 
            }
        }

        public static IWebHost BuildWebHost(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(setupConfiguration)
            .UseStartup<Startup>()
            .Build();

        private static void setupConfiguration(WebHostBuilderContext arg1, IConfigurationBuilder builder)
        {
            builder.Sources.Clear();
            builder.AddJsonFile("config.json", false, true)
                .AddXmlFile("config.xml", true)
                .AddEnvironmentVariables();

        }
    }
}
