using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FoodAPI.Models;
using MongoDB.Driver;
using Xunit;

namespace FoodAPI.IntegrationTest
{
    public class FoodItemControllerTests : IntegrationTest
    {
        public FoodItemControllerTests()
        {
            var foodItem = new FoodItem()
            {
                Id = Guid.NewGuid(),
                Name = "TestFoodItem",
                Description = "TestDescription"
            };

            Collection.InsertOne(foodItem);
        }

        [Fact]
        public async Task MyFirstIntegrationTest_HelloFromEndpoint()
        {
            var expectedFoodName = "DummyFoodItem";
            var expectedFoodDesc = "This Dummy is deserialized from JSON";
            FoodItem result = null;

            try
            {
                // Get the JSON from the response
                result = await _httpClient.GetFromJsonAsync<FoodItem>(ApiRoutes.FoodItems.TestResponse);

                // Cleanup
                Runner.Dispose();
                _httpClient.Dispose();
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception thrown when executing test MyFirstIntegrationTest_HelloFromEndpoint(): {exception.Message}");
            }

            Assert.NotNull(result);
            Assert.IsType<FoodItem>(result);
            Assert.Equal(result.Name, expectedFoodName);
            Assert.Equal(result.Description, expectedFoodDesc);
        }
    }
}