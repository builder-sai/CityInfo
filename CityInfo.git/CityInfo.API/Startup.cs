using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration.Json;
using CityInfo.API.Services;


namespace CityInfo.API
{
    public class Startup
    {
        public static IConfiguration Configuration;
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings-{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

            ////from now on our app settings are on!
            Configuration = builder.Build();

        }


        // This method gets called by the runtime. Use this method to add services to the container.
        // used to configure services and containers
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddMvcOptions(o => o.OutputFormatters.Add(
                new XmlDataContractSerializerOutputFormatter()));

            //working with old APIs the consumer might expect an uppercase letter at the beginning
            //instead of json formatted casing, for that we look for the ContractResolver
            //and cast it as a default contract.
            //services.AddMvc().AddJsonOptions( obj => 
            //{
            //    //Here we will override the default naming strategy
            //    //by default it's sett to change property name to start with a 
            //    //lowercase letter
            //    //for now we don't want that so we change it
            //    //find the contract resolver in serializerSettings
            //    if (obj.SerializerSettings.ContractResolver != null)
            //    {
            //        //using Newtonsoft.Json.Serialization;
            //        var castedResolver = obj.SerializerSettings.ContractResolver
            //                                as DefaultContractResolver;
            //        //now we have the "casted resulver" now we access the naming strategy
            //        // and set it to null so json wont change the property name
            //        // and instead showing it as it's defind in our class
            //        castedResolver.NamingStrategy = null;
            //    }
            //});

            //AddTransient are created each time the service is requested
            //this livetime works best for lightwight stateless services
            //services.AddTransient

            //Scoped livetime are created once per request
            //services.AddScoped

            //are created once when the service is requested or if you define a service
            //it would be created when the ConfigureSerivce is run and Every subsequent request
            //will use the same instance
            //services.AddSingleton


            //this would send all our emailing via the LocalMailService.
            //if we would want to use the CloudMailService we can use
            //the compile directives telling the compile what to omit and what to add
            //to compile
#if DEBUG
            // if DEBUG would run if the code is compiled in debug else if in release it would run the other one
            services.AddTransient<IMailService, LocalMailService>();
#else
            services.AddTransient<IMailService, CloudMailService>();
#endif


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //keeping it at the default meaning Information level or serious 
            //LogLevel.Information == default
            //this addDebug would add info to the debugger window
            //addConsole would add to the console windwo and so on
            loggerFactory.AddDebug();

            //loggerFactory.AddNlog();
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStatusCodePages();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
        }
    }
}
