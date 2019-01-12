using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.EventHubs.Processor;

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
                var connectionStringBuilder = new EventHubsConnectionStringBuilder(connectionString)
                {
                    EntityPath = eventHubName
                };

                var eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());

                var temperature = new Random();

                // Send 10 messages to the topic
                for (int i = 0; i < 10; i++)
                {
                    var message = $"Temperature is: {temperature.Next(0, 45)}";

                    await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(message)));

                    Console.WriteLine($"SENT: {message}");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("Exception Occurred - {0}", e.Message));
            }
        }

        static async Task Consume(string connectionString, string consumerGroup, string eventHubName, string storageConnectionString, string storageContainerName)
        {
            var eventProcessorHost = new EventProcessorHost(
                eventHubName,
                consumerGroup,
                connectionString,
                storageConnectionString,
                storageContainerName);

            // Registers the Event Processor Host and starts receiving messages
            await eventProcessorHost.RegisterEventProcessorAsync<TemperatureProcessor>();

            Console.WriteLine("Receiving. Press ENTER to stop worker.");
            Console.ReadLine();

            // Disposes of the Event Processor Host
            await eventProcessorHost.UnregisterEventProcessorAsync();
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
            var storageConnectionString = config["storageConnectionString"];
            var storageContainerName = config["storageContainerName"];

            switch (mode)
            {
                case "produce":
                    Produce(connectionString, eventHubName).Wait();
                    break;
                case "consume":
                    Consume(connectionString, consumerGroup, eventHubName, storageConnectionString, storageContainerName).Wait();
                    break;
                default:
                    PrintUsage();
                    break;
            }
        }
    }
}
