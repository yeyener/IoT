using IoT.Common;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoT.MessageProcessor
{
    internal class LoggingEventProcessor : IEventProcessor
    {
        public Task OpenAsync(PartitionContext context)
        {
            Console.WriteLine($"LoggingEventProcessor OpenAsync has been called. Partition : {context.PartitionId}");

            return Task.CompletedTask;
        }

        public Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            Console.WriteLine($"LoggingEventProcessor CloseAsync has been called. Partition : {context.PartitionId}");

            return Task.CompletedTask;
        }


        public Task ProcessErrorAsync(PartitionContext context, Exception error)
        {
            Console.WriteLine($"LoggingEventProcessor ProcessErrorAsync has been called. Partition : {context.PartitionId}");

            return Task.CompletedTask;
        }

        public Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            Console.WriteLine($"Events received on partition : {context.PartitionId}");

            foreach (var eventData in messages)
            {
                var payload = Encoding.ASCII.GetString(eventData.Body.Array,
                    eventData.Body.Offset,
                    eventData.Body.Count);

                var deviceId = eventData.SystemProperties["iothub-connection-device-id"];


                Console.WriteLine($"Message received on partition {context.PartitionId} device Id : { deviceId} payload : {payload} ");

                var telemetry = JsonConvert.DeserializeObject<Telemetry>(payload);

                if (telemetry != null && telemetry.Status == StatusType.Emergency)
                {
                    Console.WriteLine("Emergency!");

                    SendHelp(telemetry.Latitude, telemetry.Longitude);
                }
            }
            

            return context.CheckpointAsync();
        }

        private void SendHelp(decimal latitude, decimal longitude)
        {
            Console.WriteLine($"Sending help to latitude: {latitude} longitude: {longitude}");
        }
    }
}
