using IoT.AgentDevice;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Newtonsoft.Json;
using System.Text;


Console.WriteLine("Hello from agent!");

var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appSettings.json").Build();

string DeviceConnectionString = config.GetSection("devices").GetSection("device-01").GetSection("connectionString").Value;

var device = DeviceClient.CreateFromConnectionString(DeviceConnectionString);

await device.OpenAsync();

Console.WriteLine("Device is connected");


var twinProperties = new TwinCollection();

// do not user space period $ unciode. use only letters and -
twinProperties["conntecitonType"] = "wi-fi";
twinProperties["conntecitonStrength"] = "weak";

await device.UpdateReportedPropertiesAsync(twinProperties);


//var telemetry = new Telemetry()
//{
//    Message = "Complex obj ",
//    StatusCode = 1
//};

//var telemertyJson = JsonConvert.SerializeObject(telemetry);

//var telemertyBytes = Encoding.ASCII.GetBytes(telemertyJson);

//var message = new Message(telemertyBytes);

//await device.SendEventAsync(message);

//Console.WriteLine("Message sent from device");