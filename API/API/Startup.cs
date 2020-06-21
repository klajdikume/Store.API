using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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

            //how long going to be lived
            //http req create instance
            //Transient is instantiented on single method short lifetime
            //Singelton repo on start of ap, never destroyed until is shuted down
            //scoped on http req is alive
            services.AddScoped<IProductRepository, ProductRepository>();

        }

        //middleware
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection(); //redirects http routings to https

            app.UseRouting(); //get controller that gets hit

            app.UseAuthorization(); //

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            }); //map all endpoint to controllers, api knows wheret to get controllers that we get
        }
    }
}
