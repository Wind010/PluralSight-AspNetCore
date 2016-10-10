using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;


namespace TheWorld
{
    using Models;
    using Services;

    using Newtonsoft.Json.Serialization;
    using AutoMapper;
    using ViewModels;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

    public class Startup
    {
        private IHostingEnvironment _env;
        private IConfigurationRoot _config;

        public Startup(IHostingEnvironment env)
        {
            _env = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(_env.ContentRootPath)
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();

            _config = builder.Build();
        }


        /// <summary>
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> </param>
        public void ConfigureServices(IServiceCollection services)
        {
            // This method is the dependency injection layer.

            services.AddSingleton(_config);

            //_env.IsEnviornment("WhateverServer")
            if (_env.IsDevelopment())
            {
                // We only want the IMailService to be available in the scope of the controller.
                // AddSingleton and AddTransient as other options.
                services.AddScoped<IMailService, DebugMailService>();
            }
            else
            {
                //TODO:  Implement a real mail service.
            }

            services.AddDbContext<WorldContext>();

            // This is Scoped because the initialization could be costly.
            // We could also add a mock repository here for testing.
            services.AddScoped<IWorldRepository, WorldRepository>();

            // Transient because this data gets created every time we need it.
            services.AddTransient<WorldContextSeedData>();

            services.AddIdentity<WorldUser, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
                config.Password.RequiredLength = 8;
                config.Cookies.ApplicationCookie.LoginPath = "/Auth/Login";
            })
            .AddEntityFrameworkStores<WorldContext>();

            services.AddLogging();

            // Dependency injection and thus we need to add MVC.
            services.AddMvc().AddJsonOptions(config =>
            {
                config.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });
        }


        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// This is where the middle-ware is applied.  Order matters.
        /// </summary>
        /// <param name="app"><see cref="IApplicationBuilder"/> </param>
        /// <param name="env"><see cref="IHostingEnvironment"/></param>
        /// <param name="loggerFactory"><see cref="ILoggerFactory"/></param>
        /// <param name="seeder"><see cref="WorldContextSeedData"/></param>
        /// <param name="logFactory"><see cref="ILoggerFactory"/></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, 
            ILoggerFactory loggerFactory, WorldContextSeedData seeder, ILoggerFactory logFactory)
        {
            loggerFactory.AddConsole();

            // Middle-ware
            //app.UseDefaultFiles();  // Should be first when using just static files.

            // For debugging:
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                logFactory.AddDebug(LogLevel.Information);
            }
            else
            {
                logFactory.AddDebug(LogLevel.Error);
            }


            Mapper.Initialize(config =>
            {
                // We create a map from TripViewModels to Trip and Trip to TripViewModels with ReverseMap call.
                // Will also create mappings for collections of these types.
                config.CreateMap<TripViewModels, Trip>().ReverseMap();
            });

            app.UseStaticFiles();

            // Turn on use of ASP.NET Identity
            app.UseIdentity();

            // Typically one of the last items to configure.
            app.UseMvc(config =>
            {
                config.MapRoute(
                    name: "Default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "App", action = "Index" }
                    );
            });

            // Force synchronous call with Wait();
            seeder.EnsureSeedData().Wait();

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});

        }
    }
}
