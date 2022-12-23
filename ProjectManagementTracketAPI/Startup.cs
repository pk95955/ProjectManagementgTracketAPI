using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProjectManagementTracketAPI.DbContexts;
using ProjectManagementTracketAPI.Repository;
using ProjectManagementTracketAPI.ExceptionHnadler;
using System;
using System.Text;
using ProjectManagementTracketAPI.ExceptionHandler;
using System.Collections.Generic;

namespace ProjectManagementTracketAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private readonly string policyName = "CorsPolicy";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           
            services.AddDbContext<ApplicationDbContexts>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")),
            ServiceLifetime.Scoped
            );
            //var key = Configuration.GetSection("SecretKey").Value;
            var key = Configuration.GetSection("Jwt")["Key"];
            var issuer = Configuration.GetSection("Jwt")["Issuer"];
            var aud1 = Configuration.GetSection("Jwt")["Aud"];
            var aud2 = Configuration.GetSection("Jwt")["Aud2"];
            IEnumerable<string> audience = new[] { aud1, aud2 };
            services.AddSingleton<ILog, LogNLog>();
            IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
            services.AddSingleton(mapper);
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            string rabbitMQName = Configuration.GetSection("RabbitMQName").Value; 
            string rabbitMQconnection = Configuration.GetSection("RabbitMQConnectionString").Value;
            services.AddScoped<IMemberRepository>(sp =>
               ActivatorUtilities.CreateInstance<MemberRepository>(sp, rabbitMQName, rabbitMQconnection)
           );
            services.AddMemoryCache();
           
            //services.AddScoped<IUserRepository>(sp =>
             //   ActivatorUtilities.CreateInstance<UserRepository>(sp, key)
            //);
            services.AddScoped<IUserRepository>(sp =>
              ActivatorUtilities.CreateInstance<UserRepository>(sp, key, issuer, audience)
          );
            // You can use the implementation factory delegate when adding your service.
            //services.AddSingleton<ISecurityCache>(sp =>
            //    new SecurityCache(AppId, sp.GetService<IService1>(), sp.GetService<IService2>())
            //);


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Project Management Tracker API", Version = "v1" });
                // Include 'SecurityScheme' to use JWT Authentication
                // this is used for allowing enter jwt token for authrised through swagger url.
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,   

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
 
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
            });
            //CORS  Policy
            services.AddCors(opt =>
            {
                opt.AddPolicy(name: policyName, builder =>
                {
                    builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
                });
            });
            services.AddControllers();

            services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor,
               Microsoft.AspNetCore.Http.HttpContextAccessor>();

            // get secret key from app setting
            //var authenticationProviderKey = "IdentityApiKeyProjectTracker";

            services.AddAuthentication(
                x =>
            {
                //x.DefaultAuthenticateScheme = "Bearer";
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
               x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }
        ).AddJwtBearer(x =>
            {
                 x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = Configuration.GetSection("Jwt")["Issuer"],
                    ValidAudience= Configuration.GetSection("Jwt")["Aud"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key))
                };
            });
           
        }

        //This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILog logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors(policyName);
            app.UseAuthentication();
            app.UseAuthorization();
            app.ConfigureExceptionHandler(logger);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Project Management Tracket v1");
            });
            System.Web.HttpContext.Configure(app.ApplicationServices.
                GetRequiredService<Microsoft.AspNetCore.Http.IHttpContextAccessor>());
        }
    }
}
