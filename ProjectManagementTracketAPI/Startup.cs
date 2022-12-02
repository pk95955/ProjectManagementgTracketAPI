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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagementTracketAPI.ExceptionHandler;

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
            ServiceLifetime.Singleton
            );
            var key = Configuration.GetSection("SecretKey").Value;
            services.AddSingleton<ILog, LogNLog>();
            IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
            services.AddSingleton(mapper);
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            string rabbitMQName = Configuration.GetSection("RabbitMQName").Value; 
            string rabbitMQconnection = Configuration.GetSection("RabbitMQConnectionString").Value;
            services.AddSingleton<IMemberRepository>(sp =>
               ActivatorUtilities.CreateInstance<MemberRepository>(sp, rabbitMQName, rabbitMQconnection)
           );
                  
            services.AddSingleton<IUserRepository>(sp =>
                ActivatorUtilities.CreateInstance<UserRepository>(sp, key)
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

            // get secret key from app setting         
           
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key))
                };
            }); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
        }
    }
}
