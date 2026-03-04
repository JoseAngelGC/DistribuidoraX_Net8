using DistribuidoraX.Domain.Abstractions.Repositories.ProductSupplierRepositories;
using DistribuidoraX.Domain.Abstractions.UseCases.ProductSupplierUseCases;
using DistribuidoraX.Domain.Entities;

namespace DistribuidoraX.ApplicationServices.UseCases.ProductSupplierUseCases
{
    internal class NewProductSupplierUseCase : INewProductSupplierUseCase
    {
        private readonly IProductSupplierRepository_StoredProceduresSqlServer _productSupplierRepositoryStoredProceduresSqlServer;
        public NewProductSupplierUseCase(IProductSupplierRepository_StoredProceduresSqlServer productSupplierRepositoryStoredProceduresSqlServer)
        {
            _productSupplierRepositoryStoredProceduresSqlServer = productSupplierRepositoryStoredProceduresSqlServer;
        }
        public Task<int> SaveItem_StoredProceduresSqlServerAsync(ProductoProveedor productSupplierEntity)
        {
            return _productSupplierRepositoryStoredProceduresSqlServer.SaveItemAsync(productSupplierEntity);
        }
    }
}
