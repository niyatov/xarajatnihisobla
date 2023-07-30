using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OutlayApi.Data;
using OutlayApi.Entities;

namespace OutlayApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddAppDbContext(this IServiceCollection collection, IOptions<SettingOptions> options)
    {
        var connectionString = options.Value.ConnectionString;
        collection.AddDbContext<AppDbContext>(options =>
        {
            options.UseLazyLoadingProxies().UseSqlServer(connectionString)
                .UseSnakeCaseNamingConvention();
        });
    }

    public static void AddIdentityManagers(this IServiceCollection collection)
    {
        collection.AddIdentity<User, UserRole>(options =>
        {
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;
        }).AddEntityFrameworkStores<AppDbContext>();
    }

    public static void AddCorsPolicy(this IServiceCollection collection, IOptions<SettingOptions> options)
    {
        var origins = options.Value.Origins;

        collection.AddCors(options =>
        {
            options.AddDefaultPolicy(cors =>
            {
                cors.WithOrigins(origins.Split(","))
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

    }
}