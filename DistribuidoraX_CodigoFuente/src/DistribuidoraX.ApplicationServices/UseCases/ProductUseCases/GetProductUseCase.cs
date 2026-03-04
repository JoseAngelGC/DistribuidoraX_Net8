using DistribuidoraX.Domain.Abstractions.Repositories.ProductRepositories;
using DistribuidoraX.Domain.Abstractions.UseCases.ProductUseCases;
using DistribuidoraX.Domain.Entities;

namespace DistribuidoraX.ApplicationServices.UseCases.ProductUseCases
{
    internal class GetProductUseCase : IGetProductUseCase
    {
        private readonly IProductRepository_StoredProceduresSqlServer _productRepository_StoredProceduresSqlServer;
        public GetProductUseCase(IProductRepository_StoredProceduresSqlServer productRepository_StoredProceduresSqlServer)
        {
            _productRepository_StoredProceduresSqlServer = productRepository_StoredProceduresSqlServer;
        }
        public async Task<Producto> GetItemById_StoredProceduresSqlServerAsyncAsync(int productId)
        {
            return await _productRepository_StoredProceduresSqlServer.GetByIdAsync(productId);
        }
    }
}
