namespace DistribuidoraX.Shared.Dtos.ProductDtos.Interfaces
{
    public interface IProductBaseDto
    {
        public int ProductItem { get; }
        public string? ProductCode { get; }
        public string? ProductName { get; }
        public decimal ProductPrice { get; }
    }
}
