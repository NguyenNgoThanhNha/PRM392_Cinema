﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PRM_API.Data;
using PRM_API.Mappers;
using PRM_API.Middlewares;
using PRM_API.Models;
using PRM_API.Repositories;
using PRM_API.Services;
using PRM_API.Services.Impl;

namespace PRM_API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ExceptionMiddleware>();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ApplicationMapper());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);


            services.AddAuthorization();


            services.AddDbContext<ApplicationDbContext>(opt =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnectionString");
                Console.WriteLine($"MSSQL_DbConnection DbConnect: {connectionString}");
                opt.UseSqlServer(connectionString);
            });

            services.AddScoped(typeof(IRepository<,>), typeof(GenericRepository<,>));
            services.AddScoped<DatabaseInitializer>();
            services.AddScoped<UserService>();
            services.AddScoped<BookingService>();
            services.AddScoped<SeatService>();
            services.AddScoped<IShowtimeService, ShowtimeService>();
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<IFABService,FABService>();
           

            return services;
        }
    }
    public static class DatabaseInitialiserExtension
    {
        public static async Task InitialiseDatabaseAsync(this WebApplication app)
        {
            // Create IServiceScope to resolve service scope
            using var scope = app.Services.CreateScope();
            var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();

            await initializer.InitializeAsync();

            // Try to seeding data
            await initializer.SeedAsync();
        }
    }
}
