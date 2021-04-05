using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeItEasy;
using FoodAPI.Controllers;
using FoodAPI.Models;
using FoodAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using static FoodAPI.DTO.FoodItemDtos;

namespace FoodAPI.UnitTest
{
    public class FoodItemControllerTest
    {
        [Fact]
        public async Task GetAllAsync_Returns_AllFoodItemsPresent()
        {
            // Arrange
            const int fakeFoodItemCount = 1;

            var fakeFoodItem = A.CollectionOfDummy<FoodItem>(fakeFoodItemCount).ToList();
            var fakeFoodRepository = A.Fake<IFoodRepository>();
            var foodItemController = new FoodItemController(fakeFoodRepository);

            A.CallTo(() => fakeFoodRepository.GetAllAsync())
                .Returns(Task.FromResult(fakeFoodItem));

            // Act
            var actionResult = (await foodItemController.GetAllAsync()).Result as OkObjectResult;
            var returnedFoodItems = (actionResult.Value as IEnumerable<FoodItemDto>).ToList();

            // Assert
            Assert.Equal(fakeFoodItemCount, returnedFoodItems.Count);
        }

        [Fact]
        public async Task GetByIdAsync_Returns_OneFoodItem_WithSameGuid()
        {
            // Arrange
            // Setup fake Fooditem with a set GUID
            var fakeFoodItem = A.Fake<FoodItem>(options => options
                .ConfigureFake(fakeItem => fakeItem.Id = Guid.NewGuid()));

            // Fake the FoodRepository and pass it to controller
            var fakeFoodRepository = A.Fake<IFoodRepository>();
            var foodItemController = new FoodItemController(fakeFoodRepository);

            // Configures a call to fake repository. Checks if passed-in GUID will match fake FoodItem created earlier
            A.CallTo(() => fakeFoodRepository.GetByIdAsync(A<Guid>.That.Matches(input => input == fakeFoodItem.Id)))
                .Returns(Task.FromResult(fakeFoodItem));

            // Act
            // Call controller method by passing in required GUID parameter. Returns ActionResult with status OK
            // Then cast the value passed with OK result to the actual type of FoodItemDto
            var actionResult = (await foodItemController.GetByIdAsync(fakeFoodItem.Id)).Result as OkObjectResult;
            var returnedFoodItem = actionResult.Value as FoodItemDto;

            // Assert
            // Checking if an object is returned and if the GUID's still match
            Assert.NotNull(returnedFoodItem);
            Assert.Equal(fakeFoodItem.Id, returnedFoodItem.id);
        }

        // [Fact]
        // public async Task GetByIdAsync_Returns_OneFoodItem()
        // {
        //     // Arrange
        //     var fakeFoodItem = A.Dummy<FoodItem>();
        //     var fakeFoodRepository = A.Fake<IFoodRepository>();
        //     var foodItemController = new FoodItemController(fakeFoodRepository);
        //     var fakeGuid = Guid.NewGuid();

        //     A.CallTo(() => fakeFoodRepository.GetByIdAsync(fakeGuid))
        //         .WhenArgumentsMatch((Guid insertedFakeGuid) => insertedFakeGuid != null && insertedFakeGuid != Guid.Empty ? true : false)
        //         .Returns(Task.FromResult(fakeFoodItem));

        //     // Act
        //     var actionResult = (await foodItemController.GetByIdAsync(fakeGuid)).Result as OkObjectResult;
        //     var returnedFoodItem = actionResult.Value as FoodItemDto;

        //     // Assert
        //     Assert.NotNull(returnedFoodItem);
        // }
    }
}
