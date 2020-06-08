using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Entities;
using Repository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using LoggerService;
using Contracts;
using DinkToPdf.Contracts;
using DinkToPdf;
using InvoiceGeneratorService.Utility;
using System.Collections.Generic;
using Identity.Services;
using EmailService;
using Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Entities.Models;

namespace SimpleFakeRent.Extensions
{
    public static class ServiceExtensions
    {

        /// <summary>
        /// Method provides CORS policy for project
        /// More info 
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()//allows to connect from eny source, can be changed for something like .WithOrigins("server name") for request from specified source
                    .AllowAnyMethod()//can be replaced with WithMethods("POST", "GET") for just some HTTP methods
                    .AllowAnyHeader());//can be replaced with WithHeaders("accept") for allow only specified methods
            });
        }

        /// <summary>
        /// Method for configuring IIS options
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options =>
            {
                //stays empty
            });
        }

        /// <summary>
        /// Dependancy injection for Logger manager
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        /// <summary>
        /// MySql connection method
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration">to be added from Startup.cs</param>
        public static void ConfigureMySqlContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["connection:connectionString"];
            services.AddDbContext<RepositoryContext>(o => o.UseMySql(connectionString));
        }

        //public static void ConfigureFlatService(this IServiceCollection services)
        //{
        //    services.AddTransient<IFlatRepository, FlatRepository>();
        //}

        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        }

        public static void MvcOptions(this IServiceCollection services)
        {
            //fixes error of System.Text.Json.JsonException: A possible object cycle was detected which 
            //is not supported. This can either be due to a cycle or if the object depth is larger than the maximum allowed depth of 32.
            services.AddMvc(option => option.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
        }

        public static void IdentityOptions(this IServiceCollection services, IConfiguration configuration)
        {
            JwtSettings jwtSettings = new JwtSettings();
            configuration.Bind(nameof(jwtSettings), jwtSettings);
            services.AddSingleton(jwtSettings);

            services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                
            })
               .AddEntityFrameworkStores<RepositoryContext>()
               .AddDefaultTokenProviders();
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.Audience = "SimpleFakeRent";
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                    ValidateIssuer = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = true
                };
            });


            services.AddAuthorization(options =>
            {
                options.AddPolicy("Landlord",
                    builder => builder.RequireClaim("landlord.view", "true"));
                options.AddPolicy("Tenant",
                    builder => builder.RequireClaim("tenant.view", "true"));
                options.AddPolicy("Landlord,Tenant",
                    builder => builder.RequireAssertion(
                        context => context.User.HasClaim(claim =>
                          claim.Type == "landlord.view"
                          || claim.Type == "tenant.view"))
                    );
            });

            //reset token lifetime
            services.Configure<DataProtectionTokenProviderOptions>(opt =>
                opt.TokenLifespan = TimeSpan.FromHours(2));
        }

        public static void IdentityServiceDI(this IServiceCollection services)
        {
            services.AddScoped<IIdentityService, IdentityService>();
        }

        public static void ImportSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "FlatMate",
                    Version = "wersja beta",
                    Description = "API obsługujace baze danych dla strony web oraz aplikacji mobilnej",
                    Contact= new OpenApiContact
                    {
                        Name = "Grzegorz Żukowski",
                        Url= new Uri("https://www.facebook.com/grzes.zukowski")
                    },
                    License= new OpenApiLicense
                    {
                        Name= "API jest własnością PJATK Gdańsk",
                        Url = new Uri("https://gdansk.pjwstk.edu.pl/")
                    }
                });

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[0] }
                };
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] " +
                                    "and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement() {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }});



            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            

        }

        /// <summary>
        /// Adding DinkToPdf converter to Services
        /// </summary>
        public static void ConfigurePdfCreator(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
        }

        /// <summary>
        /// Add EmailService to Services, set mailing [from] details from appsetting.json - section 'EmailConfiguration" {}'
        /// </summary>
        /// <param name="services">collection of all services from Startup.cs</param>
        /// <param name="configuration">parameter passed from Startup.cs (appsetting.json insight)</param>
        public static void ConfigureEmailService(this IServiceCollection services, IConfiguration configuration)
        {
            var emailConfiguration = configuration.GetSection("EmailConfiguration").Get<EmailConfig>();
            services.AddSingleton(emailConfiguration);
            services.AddScoped<IEmailEmmiter, EmailEmmiter>();
        }

        /// <summary>
        /// Generate custom assembly load context to make it available to get  libwkhtmltox.dll AND .dylib AND .so files from /simpleFakeRent/.
        /// // Will probably be DELETED
        /// </summary>
        public static void ConfigureCustomAssembly(this IServiceCollection services)
        {

            var context = new CustomAssemblyLoadContext();
            context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "libwkhtmltox.dll"));
            /*
            #if DEBUG
                context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "libwkhtmltox.dll"));
            #else
                context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "libwkhtmltox.so"));
            #endif
            */
        }

    }
}
