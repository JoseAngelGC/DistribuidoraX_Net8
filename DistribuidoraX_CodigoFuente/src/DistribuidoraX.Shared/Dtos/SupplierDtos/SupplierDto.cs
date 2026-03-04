using DistribuidoraX.Shared.Dtos.SupplierDtos.Interfaces;

namespace DistribuidoraX.Shared.Dtos.SupplierDtos
{
    public record SupplierDto : ISupplierDto
    {
        public int SupplierItem { get; init; }
        public string? SupplierName { get; init; }
    }
}
