using System;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FoodAPI.IntegrationTest
{
    public class IntegrationTest : MongoRunner
    {
        // HttpClient targets in-memory version of the project that the <Startup> belongs to (FoodAPI)
        protected readonly HttpClient _httpClient;
        private readonly WebApplicationFactory<Startup> _applicationFactory;

        protected IntegrationTest()
        {
            // Setup In-Memory MongoDb first (also creates a new connectionString)
            SetupMongoRunner();

            // Setup the testserver for FoodAPI after database initialization
            _applicationFactory = new WebApplicationFactory<Startup>();
            var appConfiguration = _applicationFactory.Services.GetRequiredService<IConfiguration>();

            // Change conncection string to the one of the Runner
            appConfiguration.GetSection("ConnectionStrings")["MongoConnection"] = Runner.ConnectionString;

            // Create the http client that will call the testserver API
            _httpClient = _applicationFactory.CreateClient();

            // Log a status report of all services
            this.ToString();
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine("\n=========================================");
            builder.AppendLine("==============STATUS REPORT==============");
            builder.AppendLine("=========================================");
            builder.AppendLine("HTTP-CLIENT:");
            builder.AppendLine($"- Host: {_httpClient.BaseAddress.Host}");
            builder.AppendLine($"- Port: {_httpClient.BaseAddress.Port}");
            builder.AppendLine($"////////////////////////////////////////");
            builder.AppendLine("TEST-SERVER:");
            builder.AppendLine($"- Address: {_applicationFactory.Server.BaseAddress.Host}");
            builder.AppendLine($"- Port: {_applicationFactory.Server.BaseAddress.Port}");
            builder.AppendLine($"////////////////////////////////////////");
            builder.AppendLine("MONGO2GO-RUNNER");
            builder.AppendLine($"- Status: {Runner.State}");
            builder.AppendLine($"- ConnectionString: {Runner.ConnectionString}");

            return builder.ToString();
        }
    }
}