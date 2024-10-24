using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using protocol.rabbitqm.shared.Models;

namespace protocol.rabbitqm.service.DependencyInject
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
