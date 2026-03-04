using DistribuidoraX.Domain.Entities;
using DistribuidoraX.Domain.Objects.GenericObjects;

namespace DistribuidoraX.Domain.Abstractions.UseCases.ProductSupplierUseCases.Hubs
{
    public interface IHubProductSupplierUseCases_StoredProceduresSqlServer
    {
        Task<List<ProductoProveedor>> GetProductSuppliersUseCase_GetListByProductIdAsync(int productId);
        Task<bool> ProductSupplierValidationsUseCase_ExistCodeAsync(ExistCodeParameters productSupplierCodeParameters);
        Task<int> NewProductSupplierUseCase_SaveItemAsync(ProductoProveedor productSupplierEntity);
        Task<int> EditProductSupplierUseCase_UpdateItemAsync(ProductoProveedor productSuplierEntity, bool revertFlag);
        Task<int> DeleteProductSupplierUseCase_DeleteItemByStateAsync(ProductoProveedor productSupplierEntity);
    }
}
