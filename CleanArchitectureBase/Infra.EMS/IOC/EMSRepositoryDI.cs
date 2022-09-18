using Core.EMS.Entities;
using Core.Utils.Interfaces;
using Infra.MIS.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.EMS.IOC
{
    public static class EMSRepositoryDI
    {
        public static IServiceCollection InjectEMSPersistence(this IServiceCollection services)
        {
            services.AddScoped<IAsyncRepository<Product>, EMSRepository<Product>>();
            services.AddScoped<IAsyncRepository<User>, EMSRepository<User>>();

            return services;
        }
    }
}
