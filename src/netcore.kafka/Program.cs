using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using Microsoft.Extensions.Configuration;

namespace netcore.kafka
{
    class Program
    {
        private static string currentFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

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

        static async Task Produce(string bootstrapServers, string connectionString, string topic)
        {
            try
            {
                var producerConfig = new Dictionary<string, object>
                {
                    { "bootstrap.servers", $"{bootstrapServers}:9093" },
                    { "security.protocol", "SASL_SSL" },
                    { "sasl.mechanism", "PLAIN" },
                    { "sasl.username", "$ConnectionString" },
                    { "sasl.password", connectionString },
                    { "ssl.ca.location", $"{currentFolder}\\cacert.pem" },
                    // { "debug", "security,broker,protocol" }
                };

                var temperature = new Random();

                // Create the producer
                using (var producer = new Producer<long, string>(producerConfig, new LongSerializer(), new StringSerializer(Encoding.UTF8)))
                {
                    // Send 10 messages to the topic
                    for (int i = 0; i < 10; i++)
                    {
                        var message = $"Temperature is: {temperature.Next(0, 45)}";
                        var result = await producer.ProduceAsync(topic, DateTime.UtcNow.Ticks, message);
                        Console.WriteLine($"SENT: {message}");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("Exception Occurred - {0}", e.Message));
            }
        }

        static void Consume(string bootstrapServers, string connectionString, string consumerGroup, string topic)
        {
            var consumerConfig = new Dictionary<string, object>
            {
                { "bootstrap.servers", $"{bootstrapServers}:9093" },
                { "security.protocol", "SASL_SSL" },
                { "sasl.mechanism", "PLAIN" },
                { "sasl.username", "$ConnectionString" },
                { "sasl.password", connectionString },
                { "ssl.ca.location", $"{currentFolder}\\cacert.pem" },
                { "group.id", "$Default" },
                { "request.timeout.ms", 60000 },
                { "broker.version.fallback", "1.0.0" },
                { "auto.offset.reset", "earliest" },
                // { "debug", "security,broker,protocol" }
            };

            // Create the consumer
            using (var consumer = new Consumer<long, string>(consumerConfig, new LongDeserializer(), new StringDeserializer(Encoding.UTF8)))
            {
                // Subscribe to the OnMessage event
                consumer.OnMessage += (_, msg) =>
                {
                    Console.WriteLine($"RECEIVED: {msg.Value}");
                };

                // Subscribe to the Kafka topic
                consumer.Subscribe(topic);

                // Handle Cancel Keypress 
                CancellationTokenSource cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) =>
                {
                    e.Cancel = true; // prevent the process from terminating.
                    cts.Cancel();
                };

                Console.WriteLine("Ctrl-C to exit.");

                // Poll for messages
                while (!cts.IsCancellationRequested)
                {
                    consumer.Poll(TimeSpan.FromMilliseconds(1000));
                }
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
            var bootstrapServers = config["eventHubsNamespace"];
            var connectionString = config["eventHubsConnectionString"];
            var consumerGroup = config["eventHubsConsumerGroups"];
            var topic = config["eventHub"];

            switch (mode)
            {
                case "produce":
                    Produce(bootstrapServers, connectionString, topic).Wait();
                    break;
                case "consume":
                    Consume(bootstrapServers, connectionString, consumerGroup, topic);
                    break;
                default:
                    PrintUsage();
                    break;
            }
        }
    }
}
