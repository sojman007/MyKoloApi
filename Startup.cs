using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MyKoloApi.Controllers;
using MyKoloApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyKoloApi
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
            services.AddDbContextPool<AppDbContext>(options => {
                options.UseSqlServer(Configuration["DatabaseConfig:MyKoloLocalDb:ConnectionString"]);
            });

            services.AddScoped<IClaimsManager, ClaimsManager>();
            services.AddHttpContextAccessor();

            services.AddAuthentication(Options => {
                // Scheme...
                // Cookie authentication scheme and Bearer Authentication scheme
                // both schemes make use of Tokens for passing authentication data back and forth
                Options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions => 
            {
                bearerOptions.SaveToken = true;
                bearerOptions.RequireHttpsMetadata = true;
                bearerOptions.TokenValidationParameters = new TokenValidationParameters() { 
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    ValidAudience =Configuration["JWT:ValidAudience"],
                    ValidateIssuer = true,
                    ValidateAudience=true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:SigningSecret"]))
                };
            });


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
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
