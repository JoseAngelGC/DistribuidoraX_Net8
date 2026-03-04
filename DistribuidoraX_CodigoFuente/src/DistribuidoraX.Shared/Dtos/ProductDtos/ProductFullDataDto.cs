using DistribuidoraX.Shared.Dtos.ProductDtos.Interfaces;

namespace DistribuidoraX.Shared.Dtos.ProductDtos
{
    public record ProductFullDataDto : ProductDto, IProductFullDataDto
    {
        public int TypeProductItem { get; init; }
    }
}
