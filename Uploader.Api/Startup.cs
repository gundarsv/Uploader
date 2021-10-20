using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using Uploader.Api.Filters;
using Uploader.Core.Clients;
using Uploader.Core.Configurations;
using Uploader.Core.Repositories;
using Uploader.Core.Repositories.Interfaces;
using Uploader.Core.Services;
using Uploader.Core.Services.Interfaces;
using Uploader.Infrastructure.Data.Contexts;

namespace Uploader.Api
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
            services.AddDbContext<UploaderContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"))
                .LogTo(Console.WriteLine));

            services.AddScoped<ISettingsService, SettingsService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped(typeof(IAsyncRepository<>), typeof(AsyncRepository<>));
            services.AddScoped<IAsyncSettingsRepository, AsyncSettingsRepository>();
            services.AddSingleton<IStorageAccountConfiguration, StorageAccountConfiguration>();
            services.AddSingleton<AzureStorageBlobClient>();

            services.AddControllers();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Uploader.Api", Version = "v1" });

                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri("https://localhost:7281/connect/authorize"),
                            TokenUrl = new Uri("https://localhost:7281/connect/token"),
                            Scopes = new Dictionary<string, string> {
                                { "uploader.api", "uploader.api" },
                                { "profile", "profile" },
                                { "openid", "openid" }
                            }
                        }
                    }
                });
                options.OperationFilter<AuthorizeCheckOperationFilter>();
            });

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://localhost:7281";

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("uploader.api.policy", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "uploader.api");
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UploaderContext uploaderContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Uploader Api v1");

                    c.OAuthClientId("uploader.api.swaggerui");
                    c.OAuthAppName("uploader Api Swagger UI");
                    c.OAuthUsePkce();
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                    .RequireAuthorization("uploader.api.policy");
            });

            uploaderContext.Database.Migrate();
        }
    }
}
