using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Interfaces;
using DAL.Models.Internal;
using DAL.Models.Mongo;
using DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace BusDaemon
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
                    services.Configure<MongoDbSettings>(
                        hostContext.Configuration.GetSection(nameof(MongoDbSettings)));
                    services.AddSingleton<IMongoDbSettings>(sp =>
                        sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);
                    services.AddScoped(typeof(IMongoRepository<Stock>), typeof(MongoRepository<Stock>));
                    services.AddHostedService<Worker>();
                    
                });
    }
}