using Core.EMS.Interfaces;
using Core.EMS.Services;
using Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Core.EMS.IOC
{
    public static class EMSServiceDI
    {
        public static IServiceCollection InjectEMSServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJwtAuthenticationManager, JwtAuthenticationManager>();
            return services;

        }
    }
}
