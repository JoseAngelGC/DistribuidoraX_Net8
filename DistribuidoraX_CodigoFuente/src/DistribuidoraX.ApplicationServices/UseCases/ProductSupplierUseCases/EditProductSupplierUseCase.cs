using DistribuidoraX.Domain.Abstractions.Repositories.ProductSupplierRepositories;
using DistribuidoraX.Domain.Abstractions.UseCases.ProductSupplierUseCases;
using DistribuidoraX.Domain.Entities;

namespace DistribuidoraX.ApplicationServices.UseCases.ProductSupplierUseCases
{
    internal class EditProductSupplierUseCase : IEditProductSupplierUseCase
    {
        private readonly IProductSupplierRepository_StoredProceduresSqlServer _productSupplierRepositoryStoredProceduresSqlServer;
        public EditProductSupplierUseCase(IProductSupplierRepository_StoredProceduresSqlServer productSupplierRepositoryStoredProceduresSqlServer)
        {
            _productSupplierRepositoryStoredProceduresSqlServer = productSupplierRepositoryStoredProceduresSqlServer;
        }
        public Task<int> UpdateItem_StoredProceduresSqlServerAsync(ProductoProveedor productSuplierEntity, bool revertFlag)
        {
            return _productSupplierRepositoryStoredProceduresSqlServer.UpdateItemAsync(productSuplierEntity, revertFlag);
        }
    }
}
