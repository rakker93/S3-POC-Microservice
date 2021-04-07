using System;
using FoodAPI.Settings;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace FoodAPI.Services
{
    public class MongoClientService
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoClient _client;
        private readonly MongoSettingsProvider _settings;

        public MongoClientService(MongoSettingsProvider settings)
        {
            _settings = settings;

            try
            {
                BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));

                _client = new MongoClient(_settings.ConnectionString);
                _database = _client.GetDatabase(_settings.DatabaseName);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception thrown calling Constructor MongoClientService(): {exception.Message}");
            }
        }

        public IMongoCollection<T> CreateNewCollection<T>(string collectionName = null)
        {
            try
            {
                var name = (collectionName == null ? "Col" + Guid.NewGuid().ToString() : collectionName);
                return _database.GetCollection<T>(name);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception thrown calling CreateNewCollection(): {exception.Message}");
            }

            return null;
        }

        public IMongoCollection<T> GetMongoCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }
    }
}