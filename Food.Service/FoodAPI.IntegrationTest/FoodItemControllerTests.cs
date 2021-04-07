using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;
using static FoodAPI.DTO.FoodItemDtos;

namespace FoodAPI.IntegrationTest
{
    public class FoodItemControllerTests : IntegrationTest
    {
        [Fact]
        public async Task MyFirstIntegrationTest_GetAllAsync()
        {
            var response = await _httpClient.GetAsync(ApiRoutes.FoodItems.GetAll);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var foodItems = (JsonConvert.DeserializeObject<IEnumerable<FoodItemDto>>(await response.Content.ReadAsStringAsync())).ToList();
            foodItems.Should().HaveCount(1);

            var justTesting = false;
            Assert.False(justTesting);
        }
    }
}