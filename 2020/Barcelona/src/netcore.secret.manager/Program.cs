using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace SecretShouldStaySecret
{
    class Program
    {
        private static IConfiguration BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true)
                .AddEnvironmentVariables();

            var devEnvironmentVariable = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");

            var isDevelopment = string.IsNullOrEmpty(devEnvironmentVariable) ||
                                devEnvironmentVariable.ToLower() == "development";

            if (isDevelopment)
            {
                builder.AddUserSecrets<Program>();
            }

            return builder.Build();
        }

        static void Main(string[] args)
        {
            var config = BuildConfiguration();

            Console.WriteLine(config["SUPERSECRET"]);
        }
    }
}
