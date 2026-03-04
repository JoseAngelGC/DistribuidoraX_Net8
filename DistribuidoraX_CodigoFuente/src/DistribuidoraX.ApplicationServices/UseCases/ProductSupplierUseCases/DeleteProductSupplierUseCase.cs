using DistribuidoraX.Domain.Abstractions.Repositories.ProductSupplierRepositories;
using DistribuidoraX.Domain.Abstractions.UseCases.ProductSupplierUseCases;
using DistribuidoraX.Domain.Entities;

namespace DistribuidoraX.ApplicationServices.UseCases.ProductSupplierUseCases
{
    internal class DeleteProductSupplierUseCase : IDeleteProductSupplierUseCase
    {
        private readonly IProductSupplierRepository_StoredProceduresSqlServer _productSupplierRepositoryStoredProceduresSqlServer;
        public DeleteProductSupplierUseCase(IProductSupplierRepository_StoredProceduresSqlServer productSupplierRepositoryStoredProceduresSqlServer)
        {
            _productSupplierRepositoryStoredProceduresSqlServer = productSupplierRepositoryStoredProceduresSqlServer;
        }
        public Task<int> DeleteItemByDeletedStateUpdating_StoredProceduresSqlServerAsync(ProductoProveedor productSupplierEntity)
        {
            return _productSupplierRepositoryStoredProceduresSqlServer.DeleteItemByDeletedStateUpdatingAsync(productSupplierEntity);
        }
    }
}
