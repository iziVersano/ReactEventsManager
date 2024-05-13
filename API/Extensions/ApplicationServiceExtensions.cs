using Application.Activities;
using Application.Core;
using Application.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Photos;
using Infrastructure.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using System;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddDbContext<DataContext>(options =>
            {
                var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                string connStr = GetConnectionString(env, config);
                options.UseNpgsql(connStr);
            });

            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyMethod().AllowAnyHeader().AllowCredentials().WithOrigins("http://localhost:3000");
                });
            });

            services.AddMediatR(typeof(List.Handler));
            services.AddAutoMapper(typeof(MappingProfiles).Assembly);
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<Create>();
            services.AddHttpContextAccessor();
            services.AddScoped<IUserAccessor, UserAccessor>();
            services.AddScoped<IPhotoAccessor, PhotoAccessor>();
            services.Configure<CloudinarySettings>(config.GetSection("Cloudinary"));
            services.AddSignalR();

            return services;
        }

        private static string GetConnectionString(string environment, IConfiguration config)
        {
            string connStr;

            if (environment == "Development")
            {
            connStr = config.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connStr))
                throw new InvalidOperationException("Development environment is missing the DefaultConnection string.");
            }
            else
            {
            // Attempt to fetch a direct connection string set for production
            connStr = Environment.GetEnvironmentVariable("DATABASE_URL")
                        ?? config["ConnectionStrings:DefaultConnection"];

            if (string.IsNullOrEmpty(connStr))
                throw new InvalidOperationException("Production environment is missing the DATABASE_URL or DefaultConnection string.");
            }

            // Further parsing or validation of connStr could be added here if needed
            return connStr;
        }

    }
}
