
using LocationTracking.Core.Interfaces;
using LocationTracking.Core.Repositories;
using LocationTracking.Core.Services;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Reflection;

namespace LocationTracking
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSingleton<IMongoClient>(sp =>
            {
                var clientSettings = MongoClientSettings.FromConnectionString("mongodb://root:root@mongodb:27017/LocationTrackingDB?authMechanism=SCRAM-SHA-1;authSource=admin");
                clientSettings.LinqProvider = LinqProvider.V3;
                return new MongoClient(clientSettings);
            });

            builder.Services.AddScoped(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                var databaseName = "LocationTrackingDB";
                return client.GetDatabase(databaseName);
            });

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ILocationRepository, LocationRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ILocationService, LocationService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Location Tracking", Version = "v1" });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
