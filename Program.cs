using Active_Blog_Service.Models;
using Active_Blog_Service_API.Context;
using Active_Blog_Service_API.Repositories;
using Active_Blog_Service_API.Repositories.Contract;
using Active_Blog_Service_API.Services;
using Active_Blog_Service_API.Services.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Abstractions;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Active_Blog_Service_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
            
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(
                   swagger =>
                   {
                       swagger.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                       {
                           Version = "v1",
                           Title = "Active API",
                           Description = "A blog website"
                       });
                       swagger.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
                       {
                           Name = "Authorization",
                           Type = SecuritySchemeType.ApiKey,
                           Scheme = "Bearer",
                           BearerFormat = "JWT",
                           In = ParameterLocation.Header,
                           Description = "Enter 'Bearer' [space] and then your valid token"

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
                                new string[] { }
                            }
                       });
                   }

               );

            var config  = builder.Configuration;
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(config["constr"])
             );
            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();
            var repositoryAssembly = typeof(BlogRepository).Assembly;
            builder.Services.Scan(s => s
             .FromAssemblies(repositoryAssembly)
            .AddClasses(c => c.AssignableTo<IScoppedRepositoryMarker>())
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            );
            var serviceAssembly = typeof(BlogService).Assembly;
            builder.Services.Scan(s => s
            .FromAssemblies(serviceAssembly)
            .AddClasses(c => c.AssignableTo<IScopedServiceMarker>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());
            builder.Services.AddAuthentication(
                options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }
                ).AddJwtBearer(
                        options =>
                        {
                            options.SaveToken = true;
                            options.RequireHttpsMetadata = false;

                            options.TokenValidationParameters = new TokenValidationParameters
                            {
                                ValidateIssuer = true,
                                ValidIssuer = config["JWT:issuer"], // Use builder.Configuration here
                                ValidateAudience = true,
                                ValidAudience = config["JWT:audience"], // Use builder.Configuration here
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                        config["JWT:SecurityKey"]! // Use builder.Configuration here
                    ))
                            };
                        }
                );
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
