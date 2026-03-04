
namespace DistribuidoraX.Shared.Dtos.ProductDtos
{
    public record SearchProductsFiltersBaseDto
    {
        public string? ProductCodeFilter { get; init; }
        public string? TypeProductFilter { get; init; }
    }
}
