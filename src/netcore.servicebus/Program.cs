using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;

namespace netcore.kafka
{
    class Program
    {
        private static string currentFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        private static void PrintUsage()
            => Console.WriteLine("Usage: <produce|consume>");

        private static QueueClient queueClient;

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

        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            return Task.CompletedTask;
        }

        static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            // Process the message
            Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");

            // Complete the message so that it is not received again.
            // This can be done only if the queueClient is opened in ReceiveMode.PeekLock mode (which is default).
            await queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        static async Task Produce(string connectionString, string queue)
        {
            try
            {
                queueClient = new QueueClient(connectionString, queue);

                var temperature = new Random();

                for (var i = 0; i < 10; i++)
                {
                    try
                    {
                        // Create a new message to send to the queue
                        var messageBody = $"Temperature is: {temperature.Next(0, 45)}";
                        var message = new Message(Encoding.UTF8.GetBytes(messageBody));

                        // Write the body of the message to the console
                        Console.WriteLine($"Sending message: {messageBody}");

                        // Send the message to the queue
                        await queueClient.SendAsync(message);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("Exception Occurred - {0}", e.Message));
            }
        }

        static void Consume(string connectionString, string queue)
        {
            queueClient = new QueueClient(connectionString, queue);

            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                // Maximum number of Concurrent calls to the callback `ProcessMessagesAsync`, set to 1 for simplicity.
                // Set it according to how many messages the application wants to process in parallel.
                MaxConcurrentCalls = 1,

                // Indicates whether MessagePump should automatically complete the messages after returning from User Callback.
                // False value below indicates the Complete will be handled by the User Callback as seen in `ProcessMessagesAsync`.
                AutoComplete = false
            };

            // Register the function that will process messages
            queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);

            Console.WriteLine("Receiving. Press any key to stop.");
            Console.ReadLine();
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
            var connectionString = config["serviceBusConnectionString"];
            var queue = config["serviceBusQueue"];

            switch (mode)
            {
                case "produce":
                    Produce(connectionString, queue).Wait();
                    break;
                case "consume":
                    Consume(connectionString, queue);
                    break;
                default:
                    PrintUsage();
                    break;
            }
        }
    }
}
