using System;
using System.Text;
using FoodAPI.Models;
using Mongo2Go;
using MongoDB.Driver;

namespace FoodAPI.IntegrationTest
{
    public class MongoRunner : IDisposable
    {
        internal static MongoDbRunner Runner;
        internal static IMongoCollection<FoodItem> Collection;
        public string RunnerConnectionString => Runner.ConnectionString;

        internal static void SetupMongoRunner()
        {
            Runner = MongoDbRunner.Start();

            var mongoClient = new MongoClient(Runner.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase("IntegrationTest");
            Collection = mongoDatabase.GetCollection<FoodItem>("FoodItemCollection");

            var builder = new StringBuilder();
            builder.AppendLine("=========================================");
            builder.AppendLine("==============STATUS REPORT==============");
            builder.AppendLine("=========================================");
            builder.AppendLine("MongoClient:");
            builder.AppendLine($"- Host: {mongoClient.Settings.Server.Host}");
            builder.AppendLine($"- Port: {mongoClient.Settings.Server.Port}");
            builder.AppendLine($"////////////////////////////////////////");
            builder.AppendLine("Runner:");
            builder.AppendLine($"- Status: {Runner.State}");
            builder.AppendLine($"- ConnectionString: {Runner.ConnectionString}");

            Console.WriteLine(builder.ToString());
        }

        public void Dispose()
        {
            Runner.Dispose();
        }
    }
}