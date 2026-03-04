using DistribuidoraX.Domain.Abstractions.Repositories.ProductSupplierRepositories;
using DistribuidoraX.Domain.Abstractions.UseCases.ProductSupplierUseCases;
using DistribuidoraX.Domain.Entities;

namespace DistribuidoraX.ApplicationServices.UseCases.ProductSupplierUseCases
{
    public class GetProductSuppliersUseCase : IGetProductSuppliersUseCase
    {
        private readonly IProductSupplierRepository_StoredProceduresSqlServer _productSupplierRepositoryStoredProceduresSqlServer;
        public GetProductSuppliersUseCase(IProductSupplierRepository_StoredProceduresSqlServer productSupplierRepositoryStoredProceduresSqlServer)
        {
            _productSupplierRepositoryStoredProceduresSqlServer = productSupplierRepositoryStoredProceduresSqlServer;
        }
        public async Task<List<ProductoProveedor>> GetListByProductIdAsync(int productId)
        {
            return await _productSupplierRepositoryStoredProceduresSqlServer.GetListByProductIdAsync(productId);
        }
    }
}
