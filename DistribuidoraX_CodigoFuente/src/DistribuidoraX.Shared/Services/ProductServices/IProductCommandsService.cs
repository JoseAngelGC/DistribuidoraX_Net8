using DistribuidoraX.Shared.Dtos.ProductSupplierDtos;
using DistribuidoraX.Shared.Responses;

namespace DistribuidoraX.Shared.Services.ProductServices
{
    public interface IProductCommandsService
    {
        Task<IGenericResult<bool>> DeleteProductoAsync(int productItem);
        Task<IGenericResult<bool>> SaveProductAsync(ProductSupplierRequestDto productSupplierRequestDto);
        Task<IGenericResult<bool>> UpdateProductAsync(ProductSupplierRequestDto productSupplierRequestDto, int productItem);
    }
}
