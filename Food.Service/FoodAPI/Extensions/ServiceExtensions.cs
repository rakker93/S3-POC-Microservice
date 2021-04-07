using FoodAPI.Models;
using FoodAPI.Services;
using FoodAPI.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace FoodAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddMongoServiceClient(this IServiceCollection services, MongoSettingsProvider settings)
        {
            return services.AddSingleton(new MongoClientService(settings));
        }
    }
}