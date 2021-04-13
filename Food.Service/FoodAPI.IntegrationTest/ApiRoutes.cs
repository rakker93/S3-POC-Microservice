using System;

namespace FoodAPI.IntegrationTest
{
    public static class ApiRoutes
    {
        public static class FoodItems
        {
            public const string TestResponse = "http://localhost:80/fooditems/integrationtest";
            public const string GetAll = "http://localhost:80/fooditems/getall";
        }
    }
}