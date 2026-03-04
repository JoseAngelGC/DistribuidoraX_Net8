using DistribuidoraX.Domain.Entities;

namespace DistribuidoraX.Domain.Abstractions.Mappers.ProductSupplierMappers.Objects
{
    public interface IProductSupplierMappingListsObject
    {
       public List<ProductoProveedor> AddProductSupplierList { get; }
       public List<ProductoProveedor> UpdateProductSupplierList { get; }
       public List<ProductoProveedor> DeleteProductSupplierList { get; }
       public bool IncoherentMapping { get; } 
    }
}
