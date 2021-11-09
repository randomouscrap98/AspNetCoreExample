using System;
using System.Collections.Generic;
using System.Drawing;
using AspExample.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspExample
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
            //You'd normally load this from a configuration, the "configuration" field in startup, actually.
            services.AddSingleton(new ToyboxControllerConfig()
            {
                RandomNames = new List<string>()
                {
                    "Stuffikins",
                    "Flufferfu",
                    "Mr. Stuffing",
                    "My Little Pony",
                    "Shrek",
                    "PinkyDink",
                    "Cutie",
                    "Lulu"
                }
            });

            //Note: this connection string, you might want to get it from the configuration instead.
            //There are lots of examples of this online.
            services.AddDbContext<MyDbContext>(options =>
                options.UseSqlite("Data Source=mydatabase.db;"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
