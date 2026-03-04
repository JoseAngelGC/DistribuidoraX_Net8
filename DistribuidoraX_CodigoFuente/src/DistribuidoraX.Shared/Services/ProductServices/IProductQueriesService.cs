using DistribuidoraX.Shared.Dtos.GenericDtos;
using DistribuidoraX.Shared.Dtos.ProductDtos;
using DistribuidoraX.Shared.Responses;

namespace DistribuidoraX.Shared.Services.ProductServices
{
    public interface IProductQueriesService
    {
        Task<IGenericResult<List<ProductBaseDto>>> GetListByFiltersAsync(SearchProductsFiltersBaseDto filters);
        Task<IGenericResult<ProductBaseDto>> ProductByIdAsync(int id);
        Task<IGenericResult<bool>> ExistProductCodeAsync(ExistCodeParametersDto productCodeParametersDto);
    }
}
