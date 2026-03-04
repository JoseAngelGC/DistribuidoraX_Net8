using DistribuidoraX.Shared.Dtos.TypeProductDtos.Interfaces;

namespace DistribuidoraX.Shared.Dtos.TypeProductDtos
{
    public record TypeProductDto : ITypeProductDto
    {
        public int TypeProductItem { get; init; } = 0;
        public string? TypeProductName { get; init; } = string.Empty;
    }
}
