using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodAPI.Extensions;
using FoodAPI.Models;
using FoodAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using static FoodAPI.DTO.FoodItemDtos;

namespace FoodAPI.Controllers
{
    [ApiController]
    [Route("/fooditems")]
    public class FoodItemController : ControllerBase
    {
        private readonly IFoodRepository _foodRepository;

        public FoodItemController(IFoodRepository foodRepository)
        {
            this._foodRepository = foodRepository;
        }

        [HttpGet("hello")]
        public ActionResult<string> SayHello()
        {
            var foodItem = new FoodItem()
            {
                Id = Guid.NewGuid(),
                Name = "DummyFoodItem",
                Description = "This Dummy is deserialized from JSON"
            };

            return Ok(foodItem);
        }

        [HttpGet("getall")]
        public async Task<ActionResult<IEnumerable<FoodItemDto>>> GetAllAsync()
        {
            var foodItems = (await _foodRepository
                .GetAllAsync())
                .Select(record => record
                .ConvertToDto());

            return Ok(foodItems);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FoodItemDto>> GetByIdAsync(Guid id)
        {
            var foodItem = await _foodRepository.GetByIdAsync(id);

            if (foodItem == null) return NotFound();

            return Ok(foodItem.ConvertToDto());
        }

        [HttpPost]
        public async Task<ActionResult<FoodItemDto>> CreateAsync(CreateFoodItemDto createFoodItemDto)
        {
            var foodItem = new FoodItem
            {
                Name = createFoodItemDto.name,
                Description = createFoodItemDto.description,
                Price = createFoodItemDto.price
            };

            await _foodRepository.CreateAsync(foodItem);

            // Creates a 201 response that returns the created item.
            return CreatedAtAction(nameof(GetByIdAsync), new { id = foodItem.Id }, foodItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, UpdateFoodItemDto updateFoodItemDto)
        {
            var existingItem = await _foodRepository.GetByIdAsync(id);

            if (existingItem == null)
            {
                return NotFound();
            }

            // Mapping new values
            existingItem.Name = updateFoodItemDto.name;
            existingItem.Description = updateFoodItemDto.description;
            existingItem.Price = updateFoodItemDto.price;

            await _foodRepository.UpdateAsync(existingItem);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingItem = await _foodRepository.GetByIdAsync(id);

            if (existingItem == null)
            {
                return NotFound();
            }

            await _foodRepository.RemoveAsync(existingItem.Id);

            return NoContent();
        }
    }
}