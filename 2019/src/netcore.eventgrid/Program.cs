using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace netcore.servicebus
{
    class Program
    {
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

        static async Task Main(string[] args)
        {
            var config = BuildConfiguration();
            var endPoint = config["eventGridEndPoint"];
            var sas = config["eventGridSAS"];

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("aeg-sas-key", sas);

            // Event must have this fields
            var customEvent = new GridEvent<object>
            {
                Subject = "Event",
                EventType = "allEvents",
                EventTime = DateTime.UtcNow,
                Id = Guid.NewGuid().ToString(),
                Data = 10
            };

            // A List must be sent
            var eventList = new List<GridEvent<object>>() { customEvent };

            string json = JsonConvert.SerializeObject(eventList);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, endPoint)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            HttpResponseMessage response = await httpClient.SendAsync(request);

            Console.WriteLine(response.StatusCode);
        }
    }

    public class GridEvent<T> where T : class
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public string EventType { get; set; }
        public T Data { get; set; }
        public DateTime EventTime { get; set; }
    }
}
