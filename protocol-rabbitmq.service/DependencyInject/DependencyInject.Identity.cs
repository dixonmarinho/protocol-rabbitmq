using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using protocol.rabbitmq.shared.Models;

namespace protocol.rabbitmq.service.DependencyInject
{
    public static partial class DependencyInject
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services
                .AddIdentity<ApplicationUser, IdentityRole>()
                .AddDefaultTokenProviders();
            return services;
        }
    }
}
