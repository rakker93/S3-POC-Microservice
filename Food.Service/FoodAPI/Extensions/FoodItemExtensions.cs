using FoodAPI.Models;
using static FoodAPI.DTO.FoodItemDtos;

namespace FoodAPI.Extensions
{
    public static class FoodItemExtensions
    {
        public static FoodItemDto ConvertToDto(this FoodItem item)
        {
            return new FoodItemDto(item.Id, item.Name, item.Description, item.Price);
        }
    }
}