using DistribuidoraX.Domain.Abstractions.Repositories.ProductRepositories;
using DistribuidoraX.Domain.Abstractions.Repositories.ProductSupplierRepositories;
using DistribuidoraX.Domain.Abstractions.Repositories.SupplierRepositories;
using DistribuidoraX.Domain.Abstractions.Repositories.TypeProductRepositories;
using DistribuidoraX.Infraestructure.Repositories.ProductRepositories;
using DistribuidoraX.Infraestructure.Repositories.ProductSupplierRepositories;
using DistribuidoraX.Infraestructure.Repositories.SupplierRepositories;
using DistribuidoraX.Infraestructure.Repositories.TypeProductRepositories;
using Microsoft.Extensions.DependencyInjection;

namespace DistribuidoraX.Infraestructure
{
    public static class DependencyInyection
    {
        public static IServiceCollection AddInfraestructure(this IServiceCollection services)
        {
            //Product repository services
            services.AddSingleton<IProductRepository_StoredProceduresSqlServer,ProductRepository_StoredProceduresSqlServer>();

            //TypeProduct repository services
            services.AddSingleton<ITypeProductRepository_StoredProceduresSqlServer,QueriesTypeProductRepository>();

            //Supplier repository services
            services.AddSingleton<ISupplierRepository_StoredProceduresSqlServer, SupplierRepository_StoredProceduresSqlServer>();

            //ProductSupplier repository services
            services.AddSingleton<IProductSupplierRepository_StoredProceduresSqlServer, ProductSupplierRepository_StoredProceduresSqlServer>();

            return services;
        }
    }
}
