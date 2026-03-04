using DistribuidoraX.Shared.Dtos.ProductDtos;

namespace DistribuidoraX.Shared.Dtos.ProductSupplierDtos.Interfaces
{
    public interface IProductSupplierRequestDto
    {
        public ProductFullDataDto? ProductDataDto { get; }
        public List<SupplierProductDto>? ProductSupplierListDto { get; }
    }
}
