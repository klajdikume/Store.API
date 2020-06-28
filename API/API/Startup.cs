using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Errors;
using API.Extensions;
using API.Helpers;
using API.Middleware;
using AutoMapper;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        //constructor, create new instance of Startup class, inject IConfiguration
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            //sqlite config scoped lifetimed of requested
            services.AddDbContext<StoreContext>(x =>
                x.UseSqlite(_configuration
                            .GetConnectionString("DefaultConnection")));

            services.AddDbContext<AppIdentityDbContext>(x =>
            {
                x.UseSqlite(_configuration.GetConnectionString("IdentityConnection"));
            });

            //add redies
            services.AddSingleton<IConnectionMultiplexer>(c =>
            {
                var configuration = ConfigurationOptions.Parse(_configuration
                    .GetConnectionString("Redis"), true);

                return ConnectionMultiplexer.Connect(configuration);
            });


            //add automapper as a service and specify location where automapper is located, assembly where is automaper class
            services.AddAutoMapper(typeof(MappingProfiles));


            services.AddApplicationServices(); //add extensions from appeservicesextensions
            services.AddIdentityServices(_configuration); // add identity services
            services.AddSwaggerDocumentation();

            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
                });
            });
        }

        //middleware
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage(); //errors in development mode not in production mode //replace withs
            //}

            //replacesd by this below
            app.UseMiddleware<ExceptionMiddleware>();

            //pass line exception redirect to error controller // with apiresponse
            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection(); //redirects http routings to https

            app.UseRouting(); //get controller that gets hit

            //static files
            app.UseStaticFiles();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();

            app.UseAuthorization(); //

            app.UseSwaggerDocumentation();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            }); //map all endpoint to controllers, api knows wheret to get controllers that we get
        }
    }
}
