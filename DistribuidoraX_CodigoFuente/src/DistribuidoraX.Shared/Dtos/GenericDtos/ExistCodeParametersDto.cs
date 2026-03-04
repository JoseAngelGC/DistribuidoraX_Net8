using DistribuidoraX.Shared.Dtos.ProductDtos.Interfaces;

namespace DistribuidoraX.Shared.Dtos.GenericDtos
{
    public record ExistCodeParametersDto : IExistProductCodeParametersDto
    {
        public int ItemId { get; init; }
        public string? CodeValue { get; init; }
    }
}
