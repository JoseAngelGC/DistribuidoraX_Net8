using DistribuidoraX.Domain.Entities;
using DistribuidoraX.Domain.Objects.ProductObjects;

namespace DistribuidoraX.Domain.Abstractions.UseCases.ProductUseCases
{
    public interface IGetProductListUseCase
    {
        Task<List<Producto>> GetListByFiltersAsync(ProductFiltersParametersObject searchFilters);
    }
}
