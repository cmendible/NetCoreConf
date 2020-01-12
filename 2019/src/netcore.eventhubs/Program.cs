using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Processor;
using Microsoft.Extensions.Configuration;

namespace netcore.eventhubs
{
    class Program
    {
        private static void PrintUsage()
            => Console.WriteLine("Usage: <produce|consume>");

        private static IConfiguration BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true);

            var devEnvironmentVariable = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");

            var isDevelopment = string.IsNullOrEmpty(devEnvironmentVariable) ||
                                devEnvironmentVariable.ToLower() == "development";

            if (isDevelopment)
            {
                builder.AddUserSecrets<Program>();
            }

            return builder.Build();
        }

        static async Task Produce(string connectionString, string eventHubName)
        {
            try
            {
                await using (var client = new EventHubClient(connectionString, eventHubName))
                await using (var producer = client.CreateProducer())
                {
                    var temperature = new Random();

                    // Send 10 messages to the topic
                    for (int i = 0; i < 10; i++)
                    {
                        var message = $"Temperature is: {temperature.Next(0, 45)}";

                        await producer.SendAsync(new EventData(Encoding.UTF8.GetBytes(message)));

                        Console.WriteLine($"SENT: {message}");
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("Exception Occurred - {0}", e.Message));
            }
        }

        static async Task Consume(string connectionString, string consumerGroup, string eventHubName)
        {
            await using (var client = new EventHubClient(connectionString, eventHubName))
            {
                Func<PartitionContext, CheckpointManager, IPartitionProcessor> partitionProcessorFactory =
                    (partitionContext, checkpointManager) => new TemperatureProcessor(partitionContext.PartitionId);

                var partitionManager = new InMemoryPartitionManager();

                var eventProcessorOptions = new EventProcessorOptions
                {
                    InitialEventPosition = EventPosition.Latest,
                    MaximumReceiveWaitTime = TimeSpan.FromSeconds(1)
                };

                var eventProcessor = new EventProcessor(
                    consumerGroup,
                    client,
                    partitionProcessorFactory,
                    partitionManager,
                    eventProcessorOptions);

                await eventProcessor.StartAsync();

                Console.WriteLine("Receiving...");
                Console.ReadLine();
            }
        }

        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                PrintUsage();
                return;
            }

            var mode = args[0];

            var config = BuildConfiguration();
            var connectionString = config["eventHubsConnectionString"];
            var consumerGroup = config["eventHubsConsumerGroups"];
            var eventHubName = config["eventHub"];

            switch (mode)
            {
                case "produce":
                    Produce(connectionString, eventHubName).Wait();
                    break;
                case "consume":
                    Consume(connectionString, consumerGroup, eventHubName).Wait();
                    break;
                default:
                    PrintUsage();
                    break;
            }
        }
    }
}
