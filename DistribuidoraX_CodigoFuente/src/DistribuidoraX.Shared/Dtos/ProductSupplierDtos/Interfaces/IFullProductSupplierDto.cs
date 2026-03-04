using DistribuidoraX.Shared.Dtos.TypeProductDtos;

namespace DistribuidoraX.Shared.Dtos.ProductSupplierDtos.Interfaces
{
    public interface IFullProductSupplierDto
    {
        public int TypeProductId { get; }
        public List<TypeProductDto>? TypeProductList { get; }
        public List<SupplierProductDto>? ProductSupplierList { get; }
    }
}
