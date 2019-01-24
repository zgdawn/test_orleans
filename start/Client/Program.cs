using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using OrleansBasic;

namespace Client
{
    class Program
    {
        static int Main(string[] args)
        {
            return RunMainAsync().Result;
        }

        private static async Task<int> RunMainAsync()
        {
            try
            {
                using(var client = await ConnectClient())
                {
                    await DoClientWork(client);
                    Console.ReadKey();
                }
                return 0;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"\nException while trying to run client: {ex.Message}");
                Console.WriteLine("Make sure the silo the client is trying to connect is running.");
                Console.ReadKey();
                return 1;
            }
        }

        private static async Task<IClusterClient> ConnectClient()
        {
            var client = new ClientBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "OrleansBasics";
                })
                .ConfigureLogging(logging => logging.AddConsole())
                .Build();
            await client.Connect();
            Console.WriteLine("client successfully connected to silo host \n");
            return client;
        }

        private static async Task DoClientWork(IClusterClient client)
        {
            var friend = client.GetGrain<IHello>(0);
            var response = await friend.SayHello("Hello world!");
            Console.WriteLine($"\n\n{response}\n\n");
        }
    }
}
