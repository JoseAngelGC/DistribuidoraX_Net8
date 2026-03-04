using DistribuidoraX.Shared.Dtos.ProductDtos.Interfaces;

namespace DistribuidoraX.Shared.Dtos.ProductDtos
{
    public record ProductBaseWhitoutPriceDto : IProductBaseWhitoutPriceDto
    {
        public int ProductItem { get; init; }
        public string? ProductCode { get; init; }
        public string? ProductName { get; init; }
    }
}
