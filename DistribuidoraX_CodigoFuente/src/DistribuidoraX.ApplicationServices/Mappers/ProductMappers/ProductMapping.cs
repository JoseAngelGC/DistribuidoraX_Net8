using DistribuidoraX.Domain.Entities;
using DistribuidoraX.Shared.Dtos.ProductDtos;

namespace DistribuidoraX.ApplicationServices.Mappers.ProductMappers
{
    internal static class ProductMapping
    {
        public static Producto ProductoEntityMapper(ProductFullDataDto productFullDataDto)
        {
            return new Producto
            {
                ProductoId = productFullDataDto.ProductItem,
                Nombre = productFullDataDto.ProductName,
                ClaveInterna = productFullDataDto.ProductCode,
                Activo = productFullDataDto.ProductActive,
                TipoProductoId = productFullDataDto.TypeProductItem
            };
        }

    }
}
