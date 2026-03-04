namespace DistribuidoraX.Domain.Objects.GenericObjects
{
    public record ExistCodeParameters
    {
        public int ItemId { get; init; }
        public string? CodeValue { get; init; }
    }
}
