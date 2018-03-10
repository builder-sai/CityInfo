using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

namespace CityInfo.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            
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
