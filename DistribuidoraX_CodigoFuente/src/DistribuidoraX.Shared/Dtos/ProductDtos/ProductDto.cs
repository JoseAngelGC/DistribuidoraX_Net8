using DistribuidoraX.Shared.Dtos.ProductDtos.Interfaces;

namespace DistribuidoraX.Shared.Dtos.ProductDtos
{
    public record ProductDto :ProductBaseWhitoutPriceDto, IProductDto
    {
        public bool ProductActive { get; init; }
    }
}
