using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Middlewares;
using Core.Helpers;
using Core.Interfaces;
using Core.Services;
using DAL.Interfaces;
using DAL.Models.Internal;
using DAL.Models.Mongo;
using DAL.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Task = DAL.Models.Mongo.Task;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHttpContextAccessor();
            services.AddSingleton<IUriHelper>(o =>
            {
                var accessor = o.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext?.Request;
                var uri = string.Concat(request?.Scheme, "://", request?.Host.ToUriComponent());
                return new UriHelper(uri);
            });
            
            # region mongo
            services.Configure<MongoDbSettings>(
                Configuration.GetSection(nameof(MongoDbSettings)));
            services.AddSingleton<IMongoDbSettings>(sp =>
                sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);
            # endregion
            
            #region services
            
            services.AddScoped<IStockService, StockService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IStoreService, StoreService>();
            
            services.AddScoped(typeof(IMongoRepository<Stock>), typeof(MongoRepository<Stock>));
            services.AddScoped(typeof(IMongoRepository<Task>), typeof(MongoRepository<Task>));
            services.AddScoped(typeof(IMongoRepository<Store>), typeof(MongoRepository<Store>));
            
            #endregion
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "Api", Version = "v1"}); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}