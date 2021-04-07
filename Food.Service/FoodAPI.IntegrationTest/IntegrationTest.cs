using System;
using System.Net.Http;
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

        protected IntegrationTest()
        {
            // Setup In-Memory MongoDb first (also creates a new connectionString)
            SetupMongoRunner();

            // Setup the testserver for FoodAPI after database initialization
            var appFactory = new WebApplicationFactory<Startup>();
            var appConfiguration = appFactory.Services.GetRequiredService<IConfiguration>();

            appConfiguration.GetSection("ConnectionStrings")["MongoConnection"] = Runner.ConnectionString;

            // Create the http client that will call the testserver API
            _httpClient = appFactory.CreateClient();
            _httpClient.BaseAddress = new UriBuilder()
            {
                Host = "localhost",
                Port = 5001
            }.Uri;
        }
    }
}