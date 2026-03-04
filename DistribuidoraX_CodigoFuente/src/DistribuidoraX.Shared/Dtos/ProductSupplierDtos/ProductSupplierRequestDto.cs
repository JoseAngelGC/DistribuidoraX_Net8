using DistribuidoraX.Shared.Dtos.ProductDtos;
using DistribuidoraX.Shared.Dtos.ProductSupplierDtos.Interfaces;

namespace DistribuidoraX.Shared.Dtos.ProductSupplierDtos
{
    public record ProductSupplierRequestDto : IProductSupplierRequestDto
    {
        public ProductFullDataDto? ProductDataDto { get; init; }
        public List<SupplierProductDto>? ProductSupplierListDto { get; init; }
    }
}
