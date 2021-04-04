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
            const int fakeFoodItemCount = 1;

            var fakeFoodItem = A.CollectionOfDummy<FoodItem>(fakeFoodItemCount).ToList();
            var fakeFoodRepository = A.Fake<IFoodRepository>();
            var foodItemController = new FoodItemController(fakeFoodRepository);

            A.CallTo(() => fakeFoodRepository.GetAllAsync())
                .Returns(Task.FromResult(fakeFoodItem));

            // Act
            var returnedFoodItems = (await foodItemController.GetAllAsync()).ToList();

            // Assert
            Assert.Equal(fakeFoodItemCount, returnedFoodItems.Count);
        }
    }
}
