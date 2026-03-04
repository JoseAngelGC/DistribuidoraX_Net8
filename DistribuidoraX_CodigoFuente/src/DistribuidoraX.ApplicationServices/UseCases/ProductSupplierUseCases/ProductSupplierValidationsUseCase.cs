using DistribuidoraX.Domain.Abstractions.Repositories.ProductSupplierRepositories;
using DistribuidoraX.Domain.Abstractions.UseCases.ProductSupplierUseCases;
using DistribuidoraX.Domain.Objects.GenericObjects;

namespace DistribuidoraX.ApplicationServices.UseCases.ProductSupplierUseCases
{
    internal class ProductSupplierValidationsUseCase : IProductSupplierValidationsUseCase
    {
        private readonly IProductSupplierRepository_StoredProceduresSqlServer _productSupplierRepositoryStoredProceduresSqlServer;
        public ProductSupplierValidationsUseCase(IProductSupplierRepository_StoredProceduresSqlServer productSupplierRepositoryStoredProceduresSqlServer)
        {
            _productSupplierRepositoryStoredProceduresSqlServer = productSupplierRepositoryStoredProceduresSqlServer;
        }
        public async Task<bool> ExistProductSupplierCode_StoredProceduresSqlServerAsync(ExistCodeParameters productSupplierCodeParameters)
        {
            return await _productSupplierRepositoryStoredProceduresSqlServer.ExistCodeAsync(productSupplierCodeParameters);
        }
    }
}
