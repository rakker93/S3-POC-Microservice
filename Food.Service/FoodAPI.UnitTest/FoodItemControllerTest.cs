using System.Linq;
using System.Threading.Tasks;
using FakeItEasy;
using FoodAPI.Controllers;
using FoodAPI.Models;
using FoodAPI.Repository;
using Xunit;

namespace FoodAPI.UnitTest
{
    public class FoodItemControllerTest
    {
        [Fact]
        public async Task GetAllAsync_Returns_AllRecipesPresent()
        {
            // Arrange
            var fakeFoodItemCount = 1;
            var fakeFoodItem = A.CollectionOfDummy<FoodItem>(fakeFoodItemCount).ToList();

            var foodRepository = A.Fake<IFoodRepository>();
            var controller = new FoodItemController(foodRepository);
            A.CallTo(() => foodRepository.GetAllAsync()).Returns(Task.FromResult(fakeFoodItem));

            // Act
            var returnedFoodItems = await controller.GetAllAsync();
            var listItems = returnedFoodItems.ToList();

            // Assert
            Assert.Equal(fakeFoodItemCount, listItems.Count);
        }
    }
}
