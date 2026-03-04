using DistribuidoraX.Shared.Dtos.ProductSupplierDtos.Interfaces;
using DistribuidoraX.Shared.Dtos.SupplierDtos;

namespace DistribuidoraX.Shared.Dtos.ProductSupplierDtos
{
    public record SupplierProductDto : SupplierDto, ISupplierProductDto
    {
        public int SupplierProductItem { get; init; }
        public string? SupplierProductCode { get; init; }
        public decimal SupplierProductCost { get; init; }
    }
}
