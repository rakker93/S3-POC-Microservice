using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodAPI.Models;

namespace FoodAPI.Repository
{
    public interface IFoodRepository
    {
        /// <summary>
        /// Adds a FoodItem to the database asynchronously.
        /// </summary>
        Task CreateAsync(FoodItem foodItem);

        /// <summary>
        /// Gets all FoodItem from the database asynchronously.
        /// </summary>
        Task<List<FoodItem>> GetAllAsync();

        /// <summary>
        /// Gets a FoodItem from the database by id asynchronously.
        /// </summary>
        Task<FoodItem> GetByIdAsync(Guid? id);

        /// <summary>
        /// Removes a FoodItem from the database by id asynchronously.
        /// </summary>
        Task RemoveAsync(Guid? id);

        /// <summary>
        /// Updates a FoodItem in the database asynchronously.
        /// </summary>
        Task UpdateAsync(FoodItem foodItem);
    }
}