using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodAPI.Models;
using MongoDB.Driver;

namespace FoodAPI.Repository
{
    public class FoodRepository : IFoodRepository
    {
        private const string collectionName = "FoodItems";
        private readonly IMongoCollection<FoodItem> foodItemCollection;

        public FoodRepository()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var mongoDb = mongoClient.GetDatabase("FoodCatalog");
            foodItemCollection = mongoDb.GetCollection<FoodItem>(collectionName);
        }

        public async Task CreateAsync(FoodItem foodItem)
        {
            if (foodItem == null) throw new ArgumentNullException(nameof(foodItem));

            await foodItemCollection.InsertOneAsync(foodItem);
        }

        public async Task<List<FoodItem>> GetAllAsync()
        {
            return await foodItemCollection
                .Find(FilterDefinition<FoodItem>.Empty)
                .ToListAsync();
        }

        public async Task<FoodItem> GetAsync(Guid id)
        {
            var filter = Builders<FoodItem>.Filter.Eq(item => item.Id, id);

            return await foodItemCollection
                .Find(filter)
                .FirstOrDefaultAsync();
        }

        public async Task RemoveAsync(Guid id)
        {
            var filter = Builders<FoodItem>.Filter.Eq(item => item.Id, id);
            await foodItemCollection.DeleteOneAsync(filter);
        }

        public async Task UpdateAsync(FoodItem foodItem)
        {
            if (foodItem == null) throw new ArgumentNullException(nameof(foodItem));

            var filter = Builders<FoodItem>.Filter.Eq(item => item.Id, foodItem.Id);

            // Note: Upsert will create a new item if it cant find an item to update.
            await foodItemCollection.ReplaceOneAsync(filter, foodItem, new ReplaceOptions { IsUpsert = true });
        }
    }
}