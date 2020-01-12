using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Processor;

namespace netcore.eventhubs
{
    public class TemperatureProcessor : IPartitionProcessor
    {
        private static int totalEventsCount = 0;

        public static int TotalEventsCount { get => totalEventsCount; }

        private readonly string PartitionId;

        public TemperatureProcessor(string partitionId)
        {
            PartitionId = partitionId;

            Console.WriteLine($"\tPartition '{ PartitionId }': partition processor successfully created.");
        }

        public Task InitializeAsync()
        {
            Console.WriteLine($"\tPartition '{ PartitionId }': partition processor successfully initialized.");
            return Task.CompletedTask;
        }

        public Task CloseAsync(PartitionProcessorCloseReason reason)
        {
            Console.WriteLine($"\tPartition '{ PartitionId }': partition processor successfully closed. Reason: { reason }.");
            return Task.CompletedTask;
        }

        public Task ProcessErrorAsync(Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine($"\tPartition '{ PartitionId }': an unhandled exception was encountered. This was not expected to happen.");
            return Task.CompletedTask;
        }

        public Task ProcessEventsAsync(IEnumerable<EventData> events, CancellationToken cancellationToken)
        {
            int eventsCount = events.Count();

            if (eventsCount > 0)
            {
                Interlocked.Add(ref totalEventsCount, eventsCount);
                Console.WriteLine($"\tPartition '{ PartitionId }': { eventsCount } event(s) received.");
            }

            foreach (var eventData in events)
            {
                var data = Encoding.UTF8.GetString(eventData.Body.ToArray());
                Console.WriteLine($"Message received. Partition: '{PartitionId}', Data: '{data}'");
            }

            return Task.CompletedTask;
        }
    }
}