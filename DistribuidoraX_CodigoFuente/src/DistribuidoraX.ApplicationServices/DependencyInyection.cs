using DistribuidoraX.ApplicationServices.Responses;
using DistribuidoraX.ApplicationServices.Services.ProductServices;
using DistribuidoraX.ApplicationServices.Services.ProductSupplierServices;
using DistribuidoraX.ApplicationServices.Services.SupplierServices;
using DistribuidoraX.ApplicationServices.Services.TypeProductServices;
using DistribuidoraX.ApplicationServices.UseCases.ProductSupplierUseCases;
using DistribuidoraX.ApplicationServices.UseCases.ProductSupplierUseCases.Hubs;
using DistribuidoraX.ApplicationServices.UseCases.ProductUseCases;
using DistribuidoraX.ApplicationServices.UseCases.ProductUseCases.Hubs;
using DistribuidoraX.ApplicationServices.UseCases.SupplierUseCases;
using DistribuidoraX.ApplicationServices.UseCases.TransactionsRevertUseCases;
using DistribuidoraX.ApplicationServices.UseCases.TypeProductUseCases;
using DistribuidoraX.Domain.Abstractions.UseCases.ProductSupplierUseCases;
using DistribuidoraX.Domain.Abstractions.UseCases.ProductSupplierUseCases.Hubs;
using DistribuidoraX.Domain.Abstractions.UseCases.ProductUseCases;
using DistribuidoraX.Domain.Abstractions.UseCases.ProductUseCases.Hubs;
using DistribuidoraX.Domain.Abstractions.UseCases.SupplierUseCases;
using DistribuidoraX.Domain.Abstractions.UseCases.TransactionsRevertUseCases;
using DistribuidoraX.Domain.Abstractions.UseCases.TypeProductUseCases;
using DistribuidoraX.Shared.Responses;
using DistribuidoraX.Shared.Services.ProductServices;
using DistribuidoraX.Shared.Services.ProductSupplierServices;
using DistribuidoraX.Shared.Services.SupplierServices;
using DistribuidoraX.Shared.Services.TypeProductServices;
using DistribuidoraX.Shared.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DistribuidoraX.ApplicationServices
{
    public static class DependencyInyection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<ProductFullDataDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<SupplierProductDtoValidator>();

            //Responses services
            services.AddScoped(typeof(IGenericResult<>), typeof(GenericResult<>));

            //Product services
            services.AddScoped<IProductQueriesService, ProductQueriesService>();
            services.AddScoped<IProductCommandsService, ProductCommandsService>();

            //TypeProduct services
            services.AddScoped<ITypeProductQueriesService, TypeProductQueriesService>();

            //Supplier services
            services.AddScoped<ISupplierQueriesService, SupplierQueriesService>();

            //ProductSupplier services
            services.AddScoped<IProductSupplierQueriesService, ProductSupplierQueriesService>();

            //Product Use Cases
            services.AddScoped<IGetProductListUseCase, GetProductListUseCase>();
            services.AddScoped<IGetProductUseCase, GetProductUseCase>();
            services.AddScoped<IDeleteProductUseCase, DeleteProductUseCase>();
            services.AddScoped<INewProductUseCase, NewProductUseCase>();
            services.AddScoped<IEditProductUseCase, EditProductUseCase>();
            services.AddScoped<IProductValidationsUseCase, ProductValidationsUseCase>();
            services.AddScoped<ICalculateProductPriceUseCase, CalculateProductPriceUseCase>();

            //TypeProduct Use Cases
            services.AddScoped<IGetTypeProductsUseCase, GetTypeProductsUseCase>();

            //Supplier Use Cases
            services.AddScoped<IGetSupplierListUseCase, GetSupplierListUseCase>();
            services.AddScoped<IGetSupplierUseCase, GetSupplierUseCase>();

            //ProductSupplier Use Cases
            services.AddScoped<IGetProductSuppliersUseCase, GetProductSuppliersUseCase>();
            services.AddScoped<INewProductSupplierUseCase, NewProductSupplierUseCase>();
            services.AddScoped<IEditProductSupplierUseCase, EditProductSupplierUseCase>();
            services.AddScoped<IDeleteProductSupplierUseCase, DeleteProductSupplierUseCase>();
            services.AddScoped<IProductSupplierValidationsUseCase, ProductSupplierValidationsUseCase>();

            //Hubs Use Cases
            services.AddScoped<IHubTransactionsRevertUseCase, HubTransactionsRevertUseCase>();
            services.AddScoped<IHubProductSupplierUseCases_StoredProceduresSqlServer, HubProductSupplierUseCases_StoredProceduresSqlServer>();
            services.AddScoped<IHubProductUseCases_StoredProceduresSqlServer, HubProductUseCases_StoredProceduresSqlServer>();

            return services;
        }

    }
}
