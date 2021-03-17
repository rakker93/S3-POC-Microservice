using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodAPI.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;

namespace FoodAPI.Repository
{
    public class FoodRepository : IFoodRepository
    {
        private readonly IMongoCollection<FoodItem> foodItemCollection;
        private readonly IConfiguration _configuration;

        public FoodRepository(IConfiguration configuration)
        {
            this._configuration = configuration;

            try
            {
                // TODO: Simplify code and use Dependency Injection to get the Mongo Instance instead.
                BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));

                var mongoClient = new MongoClient(_configuration.GetConnectionString("MongoConnection"));
                var mongoDb = mongoClient.GetDatabase(_configuration.GetSection("DatabaseSettings")["MongoDatabase"]);
                foodItemCollection = mongoDb.GetCollection<FoodItem>(_configuration.GetSection("DatabaseSettings")["MongoCollection"]);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Problem connecting or initializing MongoClient instance: {exception.Message}");
            }
        }

        public async Task CreateAsync(FoodItem foodItem)
        {
            if (foodItem == null) throw new ArgumentNullException(nameof(foodItem));

            try
            {
                await foodItemCollection.InsertOneAsync(foodItem);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception has been thrown when calling CreateAsync: {exception.Message}");
            }
        }

        public async Task<List<FoodItem>> GetAllAsync()
        {
            try
            {
                return await foodItemCollection
                    .Find(FilterDefinition<FoodItem>.Empty)
                    .ToListAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception has been thrown when calling GetAllAsync: {exception.Message}");
                return null;
            }
        }

        public async Task<FoodItem> GetByIdAsync(Guid? id)
        {
            if (id == Guid.Empty || !id.HasValue) throw new ArgumentNullException(nameof(id));

            try
            {
                var filter = Builders<FoodItem>.Filter.Eq(item => item.Id, id);

                return await foodItemCollection
                    .Find(filter)
                    .FirstOrDefaultAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception has been thrown when calling GetAsync: {exception.Message}");
                return null;
            }

        }

        public async Task RemoveAsync(Guid? id)
        {
            if (id == Guid.Empty || !id.HasValue) throw new ArgumentNullException(nameof(id));

            try
            {
                var filter = Builders<FoodItem>.Filter.Eq(item => item.Id, id);
                await foodItemCollection.DeleteOneAsync(filter);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception has been thrown when calling RemoveAsync: {exception.Message}");
            }
        }

        public async Task UpdateAsync(FoodItem foodItem)
        {
            if (foodItem == null) throw new ArgumentNullException(nameof(foodItem));

            try
            {
                var filter = Builders<FoodItem>.Filter.Eq(item => item.Id, foodItem.Id);

                // Note: Upsert will create a new item if it cant find an item to update.
                await foodItemCollection.ReplaceOneAsync(filter, foodItem, new ReplaceOptions { IsUpsert = true });
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception has been thrown when calling UpdateAsync: {exception.Message}");
            }
        }
    }
}