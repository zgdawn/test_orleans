using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using OrleansBasic;

namespace Silo
{
    class Program
    {
        private static int Main(string[] args) => RunMainAsync().Result;

        private static async Task<int> RunMainAsync()
        {
            try
            {
                var host = await StartSilo();
                Console.WriteLine("\n\n Press enter to terminate... \n\n");
                Console.ReadLine();

                await host.StopAsync();
                return 0;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return 1;
            }
        }

        private static async Task<ISiloHost> StartSilo()
        {
            var builder = new SiloHostBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "OrleansBasics";
                })
                .ConfigureApplicationParts(parts => 
                    parts.AddApplicationPart(typeof(HelloGrain).Assembly)
                    .WithReferences())
                .ConfigureLogging(logging => logging.AddConsole());
            var host = builder.Build();
            await host.StartAsync();
            return host;
        }
    }
}
