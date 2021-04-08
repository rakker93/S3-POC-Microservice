using System;
using System.Text;
using FoodAPI.Models;
using Mongo2Go;
using MongoDB.Driver;

namespace FoodAPI.IntegrationTest
{
    public class MongoRunner
    {
        internal static MongoDbRunner Runner;
        internal static IMongoCollection<FoodItem> Collection;
        public string RunnerConnectionString => Runner.ConnectionString;

        public string RunnerStatus => Runner.State.ToString();

        internal static void SetupMongoRunner()
        {
            Runner = MongoDbRunner.Start(singleNodeReplSet: true);

            var mongoClient = new MongoClient(Runner.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase("IntegrationTest");
            Collection = mongoDatabase.GetCollection<FoodItem>("FoodItemCollection");
        }
    }
}