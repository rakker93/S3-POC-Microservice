using System;
using System.Collections.Generic;
using FoodAPI.Extensions;
using FoodAPI.Models;
using FoodAPI.Repository;
using FoodAPI.Services;
using FoodAPI.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace FoodAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);

            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "FoodAPI", Version = "v1" }));

            services.AddMongoServiceClient(new MongoSettingsProvider()
            {
                ConnectionString = Configuration.GetConnectionString("MongoConnection"),
                DatabaseName = Configuration.GetSection("DatabaseSettings")["MongoDatabase"]
            });

            services.AddSingleton<IFoodRepository, FoodRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FoodAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
