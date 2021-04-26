using System;
using ApplicationCore.Entities;
using ApplicationCore.Interceptors;
using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using BestlaArquitectureApplicationCore.Extentions;
using Castle.DynamicProxy;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ThingsboardBestlaApi.Middlewares;

namespace ThingsboardBestlaApi
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

            services.AddDbContext<CatalogContext>(c =>
                c.UseSqlServer(Configuration.GetConnectionString("CatalogConnection")));

            services.AddProxiedScoped<IUserService, UserService>();
            services.AddSingleton(new ProxyGenerator());
            services.AddAllEntityAsyncRepositoryScoped<IAggregateRoot>(typeof(EfRepository<>));

            
            services.AddScoped<AsyncInterceptorBase, LoggingAsyncInterceptor>();
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            AddSwagger(services);
        }

        private void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                var groupName = "v1";

                options.SwaggerDoc(groupName, new OpenApiInfo
                {
                    Title = $"Notifications {groupName}",
                    Version = groupName,
                    Description = "Notifications API",
                    Contact = new OpenApiContact
                    {
                        Name = "Bestla Company",
                        Email = string.Empty,
                        Url = new Uri("https://foo.com/"),
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.ConfigureExceptionHandler();

            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Notifications API V1");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
