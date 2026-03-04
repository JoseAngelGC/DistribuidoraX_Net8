using DistribuidoraX.Domain.Abstractions.Repositories.ProductRepositories;
using DistribuidoraX.Domain.Abstractions.UseCases.ProductUseCases;
using DistribuidoraX.Domain.Entities;

namespace DistribuidoraX.ApplicationServices.UseCases.ProductUseCases
{
    internal class DeleteProductUseCase : IDeleteProductUseCase
    {
        private readonly IProductRepository_StoredProceduresSqlServer _productRepository_StoredProceduresSqlServer;
        public DeleteProductUseCase(IProductRepository_StoredProceduresSqlServer productRepository_StoredProceduresSqlServer)
        {
            _productRepository_StoredProceduresSqlServer = productRepository_StoredProceduresSqlServer;
        }

        public async Task<int> DeleteByParams_StoredProceduresSqlServerAsync(Producto product)
        {
            return await _productRepository_StoredProceduresSqlServer.DeleteByParamsAsync(product);
        }
    }
}
