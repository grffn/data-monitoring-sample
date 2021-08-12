using AutoMapper;
using DataMonitoring.Core;
using DataMonitoring.Core.Models;
using DataMonitoring.Storage.MongoDb;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DataMonitoring.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<DataRetrievalService>();

                    services.Configure<MongoSettings>(hostContext.Configuration.GetSection(nameof(MongoSettings)))
                        .PostConfigure<MongoSettings>(options => options.ConnectionString = hostContext.Configuration.GetConnectionString("Default"));
                    services.AddSingleton(serviceProvider => serviceProvider.GetRequiredService<IOptions<MongoSettings>>().Value);
                    services.AddScoped<IMongoClient>(serviceProvider => new MongoClient(serviceProvider.GetRequiredService<MongoSettings>().ConnectionString));
                    services.AddScoped(serviceProvider =>
                    {
                        var settings = serviceProvider.GetRequiredService<MongoSettings>();
                        var client = serviceProvider.GetRequiredService<IMongoClient>();
                        return client.GetDatabase(settings.DatabaseName);
                    });
                    services.AddScoped(serviceProvider =>
                    {
                        var client = serviceProvider.GetRequiredService<IMongoDatabase>();
                        return client.GetCollection<AssetPosition>(nameof(AssetPosition));
                    });
                    services.AddScoped<IPositionService, MongoDbPositionService>();

                    services.AddSingleton<IExternalDataSource, MockExternalDataSource>();
                    services.AddScoped<IDataProcessor, DataProcessor>();

                    services.AddScoped<IPositionService, MongoDbPositionService>();

                    services.AddAutoMapper(typeof(Program));
                });
    }
}
