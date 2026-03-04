using DistribuidoraX.Domain.Abstractions.Repositories.ProductSupplierRepositories;
using DistribuidoraX.Domain.Entities;
using DistribuidoraX.Domain.Objects.GenericObjects;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

namespace DistribuidoraX.Infraestructure.Repositories.ProductSupplierRepositories
{
    public class ProductSupplierRepository_StoredProceduresSqlServer(IOptions<ConnectionStrings> options) : IProductSupplierRepository_StoredProceduresSqlServer
    {
        private readonly ConnectionStrings connections = options.Value;

        public async Task<List<ProductoProveedor>> GetListByProductIdAsync(int productId)
        {
            List<ProductoProveedor> ProductSupplierListResponse = [];
            using (var conexion = new SqlConnection(connections.CadenaSql))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new("sp_ObtenerListaProductoProveedorPorProductoId", conexion);
                cmd.Parameters.AddWithValue("@ProductoId", productId);
                cmd.CommandType = CommandType.StoredProcedure;

                using var reader = await cmd.ExecuteReaderAsync();
                {
                    while (reader.Read())
                    {
                        ProductSupplierListResponse.Add(new ProductoProveedor
                        {
                            ProductoProveedorId = Convert.ToInt32(reader["ProductoProveedorId"]),
                            ClaveProductoProveedor = reader["ClaveProductoProveedor"].ToString(),
                            Costo = Convert.ToDecimal(reader["Costo"]),
                            ProveedorId = Convert.ToInt32(reader["ProveedorId"]),
                            ProductoId = Convert.ToInt32(reader["ProveedorId"]),
                            EstadoEliminado = Convert.ToBoolean(reader["EstadoEliminado"]),
                            FechaActualizado = Convert.ToDateTime(reader["FechaActualizado"])
                        });
                    }
                }

            }

            return ProductSupplierListResponse;
        }

        public async Task<List<decimal>> GetCostListByProductIdAsync(int productId)
        {
            List<decimal> CostsListResponse = [];
            using (var conexion = new SqlConnection(connections.CadenaSql))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new("sp_ObtenerListaCostoProductoProveedorPorProductoId", conexion);
                cmd.Parameters.AddWithValue("@ProductoId", productId);
                cmd.CommandType = CommandType.StoredProcedure;

                using var reader = await cmd.ExecuteReaderAsync();
                {
                    while (reader.Read())
                    {
                        CostsListResponse.Add(Convert.ToDecimal(reader["Costo"]));
                    }
                }

            }

            return CostsListResponse;
        }

        public async Task<bool> ExistCodeAsync(ExistCodeParameters productSupplierCodeParameters)
        {
            bool existResponse = false;
            using (var conexion = new SqlConnection(connections.CadenaSql))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new("sp_ExisteClaveProductoProveedor", conexion);
                cmd.Parameters.AddWithValue("@ProductoProveedorId", productSupplierCodeParameters.ItemId);
                cmd.Parameters.AddWithValue("@ClaveProductoProveedor", productSupplierCodeParameters.CodeValue);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter valorRetorno = new()
                {
                    Direction = ParameterDirection.ReturnValue
                };

                cmd.Parameters.Add(valorRetorno);
                await cmd.ExecuteNonQueryAsync();

                var valueResponse = (int)valorRetorno.Value;
                existResponse = valueResponse != 0;
            }

            return existResponse;
        }

        public async Task<int> SaveItemAsync(ProductoProveedor productSupplierEntity)
        {
            int rowIdResponse = -1;
            using (var conexion = new SqlConnection(connections.CadenaSql))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new("sp_AgregarProductoProveedor", conexion);
                cmd.Parameters.AddWithValue("@ProductoId", productSupplierEntity.ProductoId);
                cmd.Parameters.AddWithValue("@ProveedorId", productSupplierEntity.ProveedorId);
                cmd.Parameters.AddWithValue("@ClaveProveedorProveedor", productSupplierEntity.ClaveProductoProveedor);
                cmd.Parameters.AddWithValue("@CostoProducto", productSupplierEntity.Costo);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter returnValue = new()
                {
                    Direction = ParameterDirection.ReturnValue
                };

                cmd.Parameters.Add(returnValue);
                await cmd.ExecuteNonQueryAsync();

                rowIdResponse = (int)returnValue.Value;
            }

            return rowIdResponse;
        }

        public async Task<int> UpdateItemAsync(ProductoProveedor productSupplierEntity, bool revertFlag)
        {
            int rowIdResponse = -1;
            using (var conexion = new SqlConnection(connections.CadenaSql))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new("sp_ActualizarProductoProveedor", conexion);
                cmd.Parameters.AddWithValue("@ProductoProveedorId", productSupplierEntity.ProductoProveedorId);
                cmd.Parameters.AddWithValue("@ProveedorId", productSupplierEntity.ProveedorId);
                cmd.Parameters.AddWithValue("@ProductoId", productSupplierEntity.ProductoId);
                cmd.Parameters.AddWithValue("@ClaveProveedorProveedor", productSupplierEntity.ClaveProductoProveedor);
                cmd.Parameters.AddWithValue("@CostoProducto", productSupplierEntity.Costo);
                cmd.Parameters.AddWithValue("@EstadoEliminado", productSupplierEntity.EstadoEliminado);
                cmd.Parameters.AddWithValue("@FechaActualizado", productSupplierEntity.FechaActualizado);
                cmd.Parameters.AddWithValue("@BanderaRetorno", revertFlag);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter returnValue = new()
                {
                    Direction = ParameterDirection.ReturnValue
                };

                cmd.Parameters.Add(returnValue);
                await cmd.ExecuteNonQueryAsync();

                rowIdResponse = (int)returnValue.Value;
            }

            return rowIdResponse;
        }

        public async Task<int> DeleteItemByIdAsync(int productSupplierId)
        {
            int rowsAffected = -1;
            using (var conexion = new SqlConnection(connections.CadenaSql))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new("sp_EliminarProductoProveedor", conexion);
                cmd.Parameters.AddWithValue("@ProductoProveedorId", productSupplierId);
                cmd.CommandType = CommandType.StoredProcedure;

                rowsAffected = await cmd.ExecuteNonQueryAsync();
            }

            return rowsAffected;
        }

        public async Task<int> DeleteItemByDeletedStateUpdatingAsync(ProductoProveedor productSupplierEntity)
        {
            int rowIdResponse = -1;
            using (var conexion = new SqlConnection(connections.CadenaSql))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new("sp_EliminarActualizarProductoProveedor", conexion);
                cmd.Parameters.AddWithValue("@ProductoProveedorId", productSupplierEntity.ProductoProveedorId);
                cmd.Parameters.AddWithValue("@Estadoliminado", productSupplierEntity.EstadoEliminado);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter returnValue = new()
                {
                    Direction = ParameterDirection.ReturnValue
                };

                cmd.Parameters.Add(returnValue);
                await cmd.ExecuteNonQueryAsync();

                rowIdResponse = (int)returnValue.Value;
            }

            return rowIdResponse;
        }

        
    }
}
