using Application.Common.Interfaces;
using Application.Options;
using Hangfire;
using Hangfire.PostgreSql;
using Infrastructure.Data;
using Infrastructure.Identity;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        
        // Database
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        // Identity
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        //Hangfire
        services.AddHangfire(config => config
            .UsePostgreSqlStorage(o =>
                o.UseNpgsqlConnection(configuration.GetConnectionString("DefaultConnection"))));
        
        services.AddHangfireServer();
        
        // Options
        services.Configure<EmailOptions>(configuration.GetSection("Email"));
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        
        // Services
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IBackgroundJobService, BackgroundJobService>();
        services.AddScoped<ITokenService, TokenService>();
        
        
        return services;
    }
}
