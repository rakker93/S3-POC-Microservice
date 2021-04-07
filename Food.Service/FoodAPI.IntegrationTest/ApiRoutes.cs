using System;

namespace FoodAPI.IntegrationTest
{
    public static class ApiRoutes
    {
        public static class FoodItems
        {
            public const string GetAll = "/fooditems/getall";
            public const string GetById = "/fooditems/{replaceThisForId}";
        }
    }
}