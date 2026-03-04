using DistribuidoraX.Domain.Abstractions.Mappers.ProductSupplierMappers.Objects;
using DistribuidoraX.Domain.Entities;

namespace DistribuidoraX.ApplicationServices.Mappers.ProductSupplierMappers.Objects
{
    internal class ProductSupplierMappingListsObject : IProductSupplierMappingListsObject
    {
        public List<ProductoProveedor> AddProductSupplierList { get; set; } = [];
        public List<ProductoProveedor> UpdateProductSupplierList { get; set; } = [];
        public List<ProductoProveedor> DeleteProductSupplierList { get; set; } = [];
        public bool IncoherentMapping { get; set; } = false;
    }
}
