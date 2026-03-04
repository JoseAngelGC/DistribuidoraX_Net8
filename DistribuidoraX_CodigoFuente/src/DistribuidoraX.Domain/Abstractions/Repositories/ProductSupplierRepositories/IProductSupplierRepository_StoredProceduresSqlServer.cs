using DistribuidoraX.Domain.Entities;
using DistribuidoraX.Domain.Objects.GenericObjects;

namespace DistribuidoraX.Domain.Abstractions.Repositories.ProductSupplierRepositories
{
    public interface IProductSupplierRepository_StoredProceduresSqlServer
    {
        Task<List<ProductoProveedor>> GetListByProductIdAsync(int productId);
        Task<List<decimal>> GetCostListByProductIdAsync(int productId);
        Task<bool> ExistCodeAsync(ExistCodeParameters productCodeParameters);
        Task<int> SaveItemAsync(ProductoProveedor productSupplierEntity);
        Task<int> UpdateItemAsync(ProductoProveedor productSupplierEntity, bool revertFlag);
        Task<int> DeleteItemByIdAsync(int productSupplierId);
        Task<int> DeleteItemByDeletedStateUpdatingAsync(ProductoProveedor productSupplierEntity);
    }
}
