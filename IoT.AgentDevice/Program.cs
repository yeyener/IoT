using IoT.Common;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Newtonsoft.Json;
using System.Text;


public class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Hello from agent!");

        var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appSettings.json").Build();

        string DeviceConnectionString = config.GetSection("devices").GetSection("device-01").GetSection("connectionString").Value;

        var device = DeviceClient.CreateFromConnectionString(DeviceConnectionString);

        await device.OpenAsync();

        Console.WriteLine("Device is connected");

        await UpdateTwin(device);

        var quitRequested = false;

        var random = new Random();

        while (!quitRequested)
        {
            Console.Write("Action?");

            var input = Console.ReadKey().KeyChar;

            Console.WriteLine();

            var status = StatusType.NotSpecified;

            var latitude = random.Next(0,100);

            var longitute = random.Next(0,100);

            switch (Char.ToLower(input))
            {
                case 'q':
                    quitRequested = true;
                    break;
                case 'h':
                    status = StatusType.Happy;
                    break;
                case 'u':
                    status = StatusType.Unhappy;
                    break;
                case 'e':
                    status = StatusType.Emergency;
                    break;
            }

            var telemetry = new Telemetry()
            {
                Status = status,
                Latitude = latitude,
                Longitude = longitute,
            };

            var payload = JsonConvert.SerializeObject(telemetry); 

            var message = new Message(Encoding.ASCII.GetBytes(payload));

            await device.SendEventAsync(message);

            Console.WriteLine("Message sent");
        }

    }



    static async Task UpdateTwin(DeviceClient device)
    {
        var twinProperties = new TwinCollection();

        // do not user space period $ unciode. use only letters and -
        twinProperties["conntecitonType"] = "wi-fi";
        twinProperties["conntecitonStrength"] = "weak";

        await device.UpdateReportedPropertiesAsync(twinProperties);
    }
}

