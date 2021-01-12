using System;
using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ITradeLogService, TradeLogService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

            if (env.IsDevelopment())
            {
                services.AddDbContext<DataContext>(options =>
             {
                 options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
             });
            }

            else
            {
                services.AddDbContext<DataContext>(options =>
           {
               options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
           });
            }

            return services;
        }
    }
}