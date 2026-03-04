using DistribuidoraX.Domain.Abstractions.Repositories.ProductRepositories;
using DistribuidoraX.Domain.Abstractions.UseCases.ProductUseCases;
using DistribuidoraX.Domain.Objects.GenericObjects;

namespace DistribuidoraX.ApplicationServices.UseCases.ProductUseCases
{
    internal class ProductValidationsUseCase : IProductValidationsUseCase
    {
        private readonly IProductRepository_StoredProceduresSqlServer _productRepositoryStoredProceduresSqlServer;
        public ProductValidationsUseCase(IProductRepository_StoredProceduresSqlServer productRepositoryStoredProceduresSqlServer)
        {
            _productRepositoryStoredProceduresSqlServer = productRepositoryStoredProceduresSqlServer;
        }
        public async Task<bool> ExistProductCode_StoredProceduresSqlServerAsync(ExistCodeParameters productCodeParameters)
        {
            return await _productRepositoryStoredProceduresSqlServer.ExistCodeAsync(productCodeParameters);
        }
    }
}
