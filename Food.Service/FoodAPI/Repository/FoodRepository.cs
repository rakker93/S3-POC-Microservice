using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodAPI.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using FoodAPI.Services;

namespace FoodAPI.Repository
{
    public class FoodRepository : IFoodRepository
    {
        private readonly MongoClientService _mongoClient;
        private readonly IMongoCollection<FoodItem> _collection;

        public FoodRepository(MongoClientService mongoClient)
        {
            _mongoClient = mongoClient;
            _collection = _mongoClient.CreateNewCollection<FoodItem>("FoodItems");
        }

        public async Task CreateAsync(FoodItem foodItem)
        {
            if (foodItem == null) throw new ArgumentNullException(nameof(foodItem));

            try
            {
                await _collection.InsertOneAsync(foodItem);
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
                return await _collection
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

                return await _collection
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
                await _collection.DeleteOneAsync(filter);
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
                await _collection.ReplaceOneAsync(filter, foodItem, new ReplaceOptions { IsUpsert = true });
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception has been thrown when calling UpdateAsync: {exception.Message}");
            }
        }
    }
}