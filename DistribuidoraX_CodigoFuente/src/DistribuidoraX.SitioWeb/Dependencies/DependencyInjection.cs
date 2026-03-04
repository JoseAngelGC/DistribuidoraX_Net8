using DistribuidoraX.Shared.Services.ProductServices;
using DistribuidoraX.Shared.Services.ProductSupplierServices;
using DistribuidoraX.Shared.Services.SupplierServices;
using DistribuidoraX.Shared.Services.TypeProductServices;
using DistribuidoraX.Shared.Validators;
using DistribuidoraX.SitioWeb.Services.ProductServices;
using DistribuidoraX.SitioWeb.Services.ProductSupplierServices;
using DistribuidoraX.SitioWeb.Services.SupplierServices;
using DistribuidoraX.SitioWeb.Services.TypeProductServices;
using FluentValidation;

namespace DistribuidoraX.SitioWeb.Dependencies
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<ProductFullDataDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<SupplierProductDtoValidator>();

            services.AddScoped<ITypeProductQueriesService, TypeProductQueriesService>();
            services.AddScoped<IProductQueriesService, ProductQueriesService>();
            services.AddScoped<IProductCommandsService, ProductCommandsService>();
            services.AddScoped<IProductSupplierQueriesService, ProductSupplierQueriesService>();
            services.AddScoped<ISupplierQueriesService, SupplierQueriesService>();
            return services;
        }
    }
}
