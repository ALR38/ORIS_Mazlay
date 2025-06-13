using Application.Common.Interfaces;
using Application.Interfaces;
using Application.Services;
using Infrastructure.Hubs;
using Infrastructure.Mongo;
using Infrastructure.Services;
using Infrastructure.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IAuthService = Application.Services.IAuthService;

namespace Infrastructure;

/// <summary>Регистрация всех инфраструктурных сервисов.</summary>
public static class DependencyInjection
{
    public static IServiceCollection RegisterInfrastructure(
        this IServiceCollection services, IConfiguration cfg)
    {
        // Mongo
        services.AddSingleton<MongoDbContext>();

        // Доменные сервисы
        services.AddScoped<IProductService , ProductService>();
        services.AddScoped<ICartService    , CartService>();
        services.AddScoped<IWishlistService, WishlistService>();
        services.AddScoped<IOrderService   , OrderService>();
        services.AddScoped<IAuthService    , AuthService>();

        // Текущий пользователь
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        // SignalR + сервис уведомлений
        services.AddSignalR();
        services.AddScoped<IOrderNotificationService, OrderNotificationService>();

        return services;
    }
}