using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.EMS.IOC;
using Infra.EMS.Data.Models;
using Infra.EMS.IOC;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Core.EMS.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace WEBAPI
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
            services.AddCors();
            services.AddAuthentication("OAuth")
            .AddJwtBearer("OAuth", config =>
            {
                var secretBytes = Encoding.UTF8.GetBytes(Constants.Secret);
                var secretKey = new SymmetricSecurityKey(secretBytes);

                config.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = Constants.Issuer,
                    ValidAudience = Constants.Audiance,
                    IssuerSigningKey = secretKey
                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ERP API", Version = "V1" });

                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };
                c.AddSecurityDefinition("Bearer", securitySchema);

                var securityRequirement = new OpenApiSecurityRequirement();
                securityRequirement.Add(securitySchema, new[] { "Bearer" });
                c.AddSecurityRequirement(securityRequirement);
            });

            services.AddDbContext<EMSDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("EMSConnectionString"), sqlServerOptions => sqlServerOptions.CommandTimeout(360000)));
            services.InjectEMSServices();
            services.InjectEMSPersistence();
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
            app.UseCors();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("v1/swagger.json", "EMS API"); });
        }
    }
}
