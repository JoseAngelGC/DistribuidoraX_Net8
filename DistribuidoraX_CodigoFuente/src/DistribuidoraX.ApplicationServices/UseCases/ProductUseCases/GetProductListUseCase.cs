using DistribuidoraX.Domain.Abstractions.Repositories.ProductRepositories;
using DistribuidoraX.Domain.Abstractions.UseCases.ProductUseCases;
using DistribuidoraX.Domain.Entities;
using DistribuidoraX.Domain.Objects.ProductObjects;

namespace DistribuidoraX.ApplicationServices.UseCases.ProductUseCases
{
    internal class GetProductListUseCase : IGetProductListUseCase
    {
        private readonly IProductRepository_StoredProceduresSqlServer _productRepository_StoredProceduresSqlServer;
        public GetProductListUseCase(IProductRepository_StoredProceduresSqlServer productRepository_StoredProceduresSqlServer)
        {
            _productRepository_StoredProceduresSqlServer = productRepository_StoredProceduresSqlServer;
        }

        public async Task<List<Producto>> GetListByFiltersAsync(ProductFiltersParametersObject searchFilters)
        {
            return await _productRepository_StoredProceduresSqlServer.GetListBySearchFiltersAsync(searchFilters);
        }
    }
}
