using System;
using System.ComponentModel.DataAnnotations;

namespace FoodAPI.DTO
{
    public class FoodItemDtos
    {
        public record FoodItemDto(Guid id, string name, string description, decimal price);
        public record CreateFoodItemDto([Required] string name, string description, [Range(0, 100)] decimal price);
        public record UpdateFoodItemDto([Required] string name, string description, [Range(0, 100)] decimal price);
    }
}