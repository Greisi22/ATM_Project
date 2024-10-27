using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Services.SignUp;
using CleanArchitecture.Application.Services.Users;
using CleanArchitecture.Infrastructure.Persistence;
using CleanArchitecture.Infrastructure.Persistence.Interceptors;
using CleanArchitecture.Infrastructure.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    private const string DefaultConnectionStringKey = "DefaultConnection";
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();


      
        services.AddDbContext<ApplicationDbContext>(options =>
       options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
           builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
       .UseSnakeCaseNamingConvention());

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<ApplicationDbContextInitialiser>();
        services.AddTransient<IDateTime, DateTimeService>();
        services.AddTransient<IUserService, UserService>();
        

        return services;
    }
}
