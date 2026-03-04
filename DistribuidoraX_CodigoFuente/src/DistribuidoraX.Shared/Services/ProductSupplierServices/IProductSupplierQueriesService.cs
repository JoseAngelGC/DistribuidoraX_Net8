using DistribuidoraX.Shared.Dtos.GenericDtos;
using DistribuidoraX.Shared.Dtos.ProductSupplierDtos;
using DistribuidoraX.Shared.Responses;

namespace DistribuidoraX.Shared.Services.ProductSupplierServices
{
    public interface IProductSupplierQueriesService
    {
        Task<IGenericResult<FullProductSupplierDto>> GetListByProductIdAsync(int productId);
        Task<IGenericResult<bool>> ExistProductSupplierCodeAsync(ExistCodeParametersDto productSupplierCodeParametersDto);
    }
}
