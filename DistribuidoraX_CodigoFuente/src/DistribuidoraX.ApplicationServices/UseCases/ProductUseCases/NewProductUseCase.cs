using DistribuidoraX.Domain.Abstractions.Repositories.ProductRepositories;
using DistribuidoraX.Domain.Abstractions.UseCases.ProductUseCases;
using DistribuidoraX.Domain.Entities;

namespace DistribuidoraX.ApplicationServices.UseCases.ProductUseCases
{
    internal class NewProductUseCase : INewProductUseCase
    {
        private readonly IProductRepository_StoredProceduresSqlServer _productRepositoryStoredProceduresSqlServer;
        public NewProductUseCase(IProductRepository_StoredProceduresSqlServer productRepositoryStoredProceduresSqlServer)
        {
            _productRepositoryStoredProceduresSqlServer = productRepositoryStoredProceduresSqlServer;
        }
        public async Task<int> SaveItem_StoredProceduresSqlServerAsync(Producto product)
        {
            return await _productRepositoryStoredProceduresSqlServer.SaveItemAsync(product);
        }
    }
}
