using DistribuidoraX.Domain.Entities;
using DistribuidoraX.Domain.Objects.GenericObjects;
using DistribuidoraX.Domain.Objects.ProductObjects;

namespace DistribuidoraX.Domain.Abstractions.Repositories.ProductRepositories
{
    public interface IProductRepository_StoredProceduresSqlServer
    {
        Task<List<Producto>> GetListBySearchFiltersAsync(ProductFiltersParametersObject searchFilters);
        Task<Producto> GetByIdAsync(int productoId);
        Task<bool> ExistCodeAsync(ExistCodeParameters  productCodeParameters);
        Task<int> SaveItemAsync(Producto product);
        Task<int> UpdateItemAsync(Producto product);
        Task<int> UpdatePriceAsync(Producto product);
        Task<int> DeleteByParamsAsync(Producto productEntity);
    }
}
