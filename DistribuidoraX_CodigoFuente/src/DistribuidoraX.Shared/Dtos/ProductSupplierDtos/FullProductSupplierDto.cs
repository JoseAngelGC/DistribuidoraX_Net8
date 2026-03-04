using DistribuidoraX.Shared.Dtos.ProductDtos;
using DistribuidoraX.Shared.Dtos.ProductSupplierDtos.Interfaces;
using DistribuidoraX.Shared.Dtos.TypeProductDtos;

namespace DistribuidoraX.Shared.Dtos.ProductSupplierDtos
{
    public record FullProductSupplierDto : ProductDto, IFullProductSupplierDto
    {
        public int TypeProductId { get; init; }
        public List<TypeProductDto>? TypeProductList { get; init; }
        public List<SupplierProductDto>? ProductSupplierList { get; init; }
    }
}
