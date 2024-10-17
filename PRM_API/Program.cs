using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PRM_API.Extensions;
using PRM_API.Middlewares;
using PRM_API.Models;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // C?u h�nh c�c d?ch v?
        builder.Services.AddInfrastructure(builder.Configuration);

        builder.Services.AddCors(option =>
           option.AddPolicy("CORS", builder =>
               builder.AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetIsOriginAllowed((host) => true)));

        var app = builder.Build();

        app.Lifetime.ApplicationStarted.Register(async () =>
        {
           // Database Initialiser 
            await app.InitialiseDatabaseAsync();
        });
        
         if (app.Environment.IsDevelopment())
         {
             var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");
             Console.WriteLine($"MSSQL_DbConnection Program: {connectionString}");
             await using (var scope = app.Services.CreateAsyncScope())
             {
                 var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                 await dbContext.Database.MigrateAsync();
             }

             app.UseSwagger();
             app.UseSwaggerUI();
         }

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseCors("CORS");

        app.UseHttpsRedirection();

        app.UseMiddleware<StatusCodeMiddleware>();
        app.UseMiddleware<ExceptionMiddleware>();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}