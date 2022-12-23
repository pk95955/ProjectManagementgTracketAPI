using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using ProjectManagementTracketAPI.DbContexts;
using ProjectManagementTracketAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIGateway
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
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddOcelot();

            // get secret key from app setting 
            var key = Configuration.GetSection("Jwt")["Key"];
            var issuer = Configuration.GetSection("Jwt")["Issuer"];
            var aud1 = Configuration.GetSection("Jwt")["Aud1"];
            var aud2 = Configuration.GetSection("Jwt")["Aud2"];
            IEnumerable<string> audience =new[] { aud1, aud2 };
            services.AddDbContext<ApplicationDbContexts>(options =>
           options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")),
           ServiceLifetime.Scoped
           );
            services.AddMemoryCache();
            services.AddScoped<IUserRepository>(sp =>
                ActivatorUtilities.CreateInstance<UserRepository>(sp, key,issuer, audience)
            );

            //CORS  Policy
            services.AddCors(opt =>
            {
                opt.AddPolicy(name: policyName, builder =>
                {
                    builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
                });
            });
            services.AddControllers();
        //    var authenticationProviderKey = "IdentityApiKey";


        //    services.AddAuthentication(x =>
        //    {
        //        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //    }).AddJwtBearer(authenticationProviderKey,x =>
        //    {
        //        x.RequireHttpsMetadata = false;
        //        x.SaveToken = true;
        //        x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        //        {
        //            ValidateIssuerSigningKey = true,
        //            ValidateIssuer = false,
        //            ValidateAudience = false,
        //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key))
        //        };
        //    });
       }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public  void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseCors(policyName);

            //await app.UseOcelot();
           // app.UseAuthentication();          
            //app.UseAuthorization();
           
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        await context.Response.WriteAsync("Hello World!");
            //    });
            //});
        }
    }
}
