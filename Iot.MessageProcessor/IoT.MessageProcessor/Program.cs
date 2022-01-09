using IoT.MessageProcessor;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Newtonsoft.Json;

public class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Hello from the Message Processor Console!");

        var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appSettings.json").Build();

        var hubName = config.GetSection("hubName").Value;

        var hubConnectionString = config.GetSection("hubConnectionString").Value;

        var storageConnectionString = config.GetSection("storageConnectionString").Value;

        var storageContainerName = config.GetSection("storageContainerName").Value;

        var consumerGroupName = PartitionReceiver.DefaultConsumerGroupName;

        var processor = new EventProcessorHost(hubName, consumerGroupName, hubConnectionString,storageConnectionString,storageContainerName);

        await processor.RegisterEventProcessorAsync<LoggingEventProcessor>();

        Console.WriteLine("Event processor started, press enter to exit");

        Console.ReadLine();

        try
        {
            Console.WriteLine("Event processor unregister started, please wait...");

            await processor.UnregisterEventProcessorAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in UnregisterEventProcessorAsync : {ex.Message}");
        }                

        Console.WriteLine("Event processor unregistered, press enter to close the console");

        Console.ReadLine();
    }
}


