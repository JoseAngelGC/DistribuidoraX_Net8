using DistribuidoraX.Domain.Entities;
using DistribuidoraX.Domain.Objects.GenericObjects;

namespace DistribuidoraX.Domain.Abstractions.UseCases.ProductUseCases.Hubs
{
    public interface IHubProductUseCases_StoredProceduresSqlServer
    {
        Task<Producto> GetProductUseCase_GetItemByIdAsync(int productId);
        Task<bool> ProductValidationsUseCase_ExistCodeAsync(ExistCodeParameters productCodeParameters);
        Task<int> NewProductUseCase_SaveItemAsync(Producto product);
        Task<int> EditProductUseCase_UpdateItemAsync(Producto product);
        Task<int> EditProductSupplierUseCase_UpdatePriceAsync(int productId);
        Task<int> DeleteProductUseCase_DeleteByParamsAsync(Producto product);
    }
}
