using DistribuidoraX.ApplicationServices.Mappers.ProductSupplierMappers.Objects;
using DistribuidoraX.Domain.Abstractions.Mappers.ProductSupplierMappers.Objects;
using DistribuidoraX.Domain.Entities;
using DistribuidoraX.Shared.Dtos.ProductSupplierDtos;

namespace DistribuidoraX.ApplicationServices.Mappers.ProductSupplierMappers
{
    internal static class ProductSupplierMapping
    {
        public static IProductSupplierMappingListsObject ProductSupplierMappingLists(
            List<SupplierProductDto> productSupplierListDto, 
            List<ProductoProveedor> productSupplierListBackup,
            int productId)
        {
            List<ProductoProveedor> newProductSupplierList = [];
            List<ProductoProveedor> editProductSupplierList = [];
            List<ProductoProveedor> deleteProductSupplierList = [];

            if (productSupplierListDto.Count > 0)
            {
                foreach (var productSupplier in productSupplierListDto)
                {
                    if (productSupplier.SupplierProductItem == 0)
                    {
                        newProductSupplierList.Add(new ProductoProveedor
                        {
                            ProductoProveedorId = productSupplier.SupplierProductItem,
                            ClaveProductoProveedor = productSupplier.SupplierProductCode,
                            Costo = productSupplier.SupplierProductCost,
                            ProveedorId = productSupplier.SupplierItem,
                            ProductoId = productId,
                            FechaAlta = DateTime.Now
                        });
                    }

                    if (productSupplier.SupplierProductItem > 0 && productSupplierListBackup.Count > 0)
                    {
                        var productSupplierBackup = productSupplierListBackup.Find(p => p.ProductoProveedorId == productSupplier.SupplierProductItem);
                        if (productSupplierBackup != null)
                        {
                            
                            editProductSupplierList.Add(new ProductoProveedor
                            {
                                ProductoProveedorId = productSupplier.SupplierProductItem,
                                ClaveProductoProveedor = productSupplier.SupplierProductCode,
                                Costo = productSupplier.SupplierProductCost,
                                ProveedorId = productSupplier.SupplierItem,
                                ProductoId = productId,
                                FechaActualizado = DateTime.Now,
                                EstadoEliminado = productSupplierBackup.EstadoEliminado,
                            });
                        }
                        else
                        {
                            return new ProductSupplierMappingListsObject
                            {
                                AddProductSupplierList = newProductSupplierList,
                                UpdateProductSupplierList = editProductSupplierList,
                                DeleteProductSupplierList = deleteProductSupplierList,
                                IncoherentMapping = true
                            };
                        }
                    }

                    if (productSupplier.SupplierProductItem > 0 && productSupplierListBackup.Count == 0)
                    {
                        return new ProductSupplierMappingListsObject
                        {
                            AddProductSupplierList = newProductSupplierList,
                            UpdateProductSupplierList = editProductSupplierList,
                            DeleteProductSupplierList = deleteProductSupplierList,
                            IncoherentMapping = true
                        };
                    }

                }

                if (productSupplierListBackup.Count > productSupplierListDto.Count)
                {
                    deleteProductSupplierList = productSupplierListBackup;
                    foreach (var productSupplier in productSupplierListDto)
                    {
                        deleteProductSupplierList.RemoveAll(p => p.ProductoProveedorId == productSupplier.SupplierProductItem);
                    }

                    foreach (var deleteProductSupplier in deleteProductSupplierList)
                    {
                        deleteProductSupplier.EstadoEliminado = true;
                    }
                }
            }
            else
            {
                foreach (var productSuplierBackup in productSupplierListBackup)
                {
                    deleteProductSupplierList.Add(new ProductoProveedor
                    {
                        ProductoProveedorId = productSuplierBackup.ProductoProveedorId,
                        ClaveProductoProveedor = productSuplierBackup.ClaveProductoProveedor,
                        Costo = productSuplierBackup.Costo,
                        ProveedorId = productSuplierBackup.ProveedorId,
                        ProductoId = productSuplierBackup.ProductoId,
                        FechaActualizado = DateTime.Now,
                        EstadoEliminado = true
                    });
                }
            }



             return new ProductSupplierMappingListsObject
                        {
                            AddProductSupplierList = newProductSupplierList,
                            UpdateProductSupplierList = editProductSupplierList,
                            DeleteProductSupplierList = deleteProductSupplierList,
                            IncoherentMapping = false
                        };

        }
    }
}
