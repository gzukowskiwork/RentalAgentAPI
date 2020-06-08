using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SimpleFakeRent.Extensions;
using Newtonsoft.Json;
using NLog;
using System.IO;
using InvoiceGeneratorService.Utility;
using Microsoft.AspNetCore.HttpOverrides;
using EmailService;
using Microsoft.AspNetCore.Http.Features;

namespace SimpleFakeRent
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // This line allows to drop DinkToPdf files in SimpleFakeRent instead of bin/Debug/netappcore3.0 location (difference in working linux and windows)
            //services.ConfigureCustomAssembly(); // to be deleted at the ond of work on InvoiceGenerator
            services.ConfigureCors();
            services.ConfigureIISIntegration();
            services.ConfigureMySqlContext(Configuration);
            services.ConfigureRepositoryWrapper();
            services.AddAutoMapper(typeof(Startup));
            services.MvcOptions();
            services.ImportSwagger();
            services.ConfigureLoggerService();
            services.ConfigurePdfCreator();
            services.ConfigureEmailService(Configuration);
            services.IdentityOptions(Configuration);
            services.AddControllers();

            services.IdentityServiceDI();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.Use(async(context, next) =>
                {
                    await next();
                    if(context.Response.StatusCode==404 && !Path.HasExtension(context.Request.Path.Value))
                    {
                        context.Request.Path = "/index.html";
                        await next();
                    }
                    });
                app.UseHsts();
            }


            app.UseSwagger();

            app.UseHttpsRedirection();
           
            app.UseStaticFiles();

            app.UseCors("CorsPolicy");

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });
         
            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.SwaggerConfiguration();
           
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });



        }
    }
}
