using CoreWebApi.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace CoreWebApi.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config, IWebHostEnvironment env)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(optinos =>
                   {
                       optinos.TokenValidationParameters = new TokenValidationParameters
                       {
                           ValidateIssuerSigningKey = true,
                           IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                           .GetBytes(config.GetSection("AppSettings").GetSection("Token").Value)),
                           ValidateIssuer = false,
                           ValidateAudience = false
                       };
                   });

            if (env.IsDevelopment())
            {
                services.AddSwaggerGen(swagger =>
                {
                    swagger.DocumentFilter<ApplyDocumentVendorExtensions>();
                    //This is to generate the Default UI of Swagger Documentation  
                    swagger.SwaggerDoc("v2", new OpenApiInfo
                    {
                        Version = "v2",
                        Title = "LMS Web Api",
                        Description = "ASP.NET Core 3.1 Web API"
                    });
                    // To Enable authorization using Swagger (JWT)  
                    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    });
                    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                                new string[] {}
                            }
                        });

                });

            }
            return services;
        }

    }
}
