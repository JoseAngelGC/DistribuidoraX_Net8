using DistribuidoraX.Domain.Abstractions.Repositories.ProductRepositories;
using DistribuidoraX.Domain.Entities;
using DistribuidoraX.Domain.Objects.GenericObjects;
using DistribuidoraX.Domain.Objects.ProductObjects;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

namespace DistribuidoraX.Infraestructure.Repositories.ProductRepositories
{
    internal class ProductRepository_StoredProceduresSqlServer(IOptions<ConnectionStrings> options) : IProductRepository_StoredProceduresSqlServer
    {
        private readonly ConnectionStrings connections = options.Value;

        public async Task<List<Producto>> GetListBySearchFiltersAsync(ProductFiltersParametersObject searchFilters)
        {
            List<Producto> listaProducto = [];
            using (var conexion = new SqlConnection(connections.CadenaSql))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new("sp_ObtenerListaProductoPorFiltros", conexion);
                cmd.Parameters.AddWithValue("@Clave", searchFilters.ProductCodeFilter);
                cmd.Parameters.AddWithValue("@NombreTipoProducto", searchFilters.TypeProductFilter);
                cmd.CommandType = CommandType.StoredProcedure;

                using var reader = await cmd.ExecuteReaderAsync();
                {
                    while (reader.Read())
                    {
                        listaProducto.Add(new Producto
                        {
                            ProductoId = Convert.ToInt32(reader["ProductoId"]),
                            Nombre = reader["Nombre"].ToString(),
                            ClaveInterna = reader["ClaveProducto"].ToString(),
                            Precio = Convert.ToDecimal(reader["Precio"]),
                            Activo = Convert.ToBoolean(reader["Activo"]),
                        });
                    }
                }

            }

            return listaProducto;
        }

        public async Task<Producto> GetByIdAsync(int productId)
        {
            Producto productoResponse = new();
            using (var conexion = new SqlConnection(connections.CadenaSql))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new("sp_ObtenerProductoPorId", conexion);
                cmd.Parameters.AddWithValue("@ProductoId", productId);
                cmd.CommandType = CommandType.StoredProcedure;

                using var reader = await cmd.ExecuteReaderAsync();
                {
                    while (reader.Read())
                    {
                        productoResponse.ProductoId = Convert.ToInt32(reader["ProductoId"]);
                        productoResponse.Nombre = reader["Nombre"].ToString();
                        productoResponse.ClaveInterna = reader["ClaveProducto"].ToString();
                        productoResponse.Precio = Convert.ToDecimal(reader["Precio"]);
                        productoResponse.Activo = Convert.ToBoolean(reader["Activo"]);
                        productoResponse.TipoProductoId = Convert.ToInt32(reader["TipoProductoId"]);
                    }
                }

            }

            return productoResponse;
        }

        public async Task<bool> ExistCodeAsync(ExistCodeParameters productCodeParameters)
        {
            bool existResponse = false;
            using (var conexion = new SqlConnection(connections.CadenaSql))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new("sp_ExisteClaveProducto", conexion);
                cmd.Parameters.AddWithValue("@ProductoId", productCodeParameters.ItemId);
                cmd.Parameters.AddWithValue("@ClaveProducto", productCodeParameters.CodeValue);
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

        public async Task<int> SaveItemAsync(Producto product)
        {
            int rowIdResponse = -1;
            using (var conexion = new SqlConnection(connections.CadenaSql))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new("sp_AgregarProducto", conexion);
                cmd.Parameters.AddWithValue("@NombreProducto", product.Nombre);
                cmd.Parameters.AddWithValue("@ClaveProducto", product.ClaveInterna);
                cmd.Parameters.AddWithValue("@PrecioProducto", product.Precio);
                cmd.Parameters.AddWithValue("@ActivoProducto", product.Activo);
                cmd.Parameters.AddWithValue("@TipoProductoId", product.TipoProductoId);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter valorRetorno = new()
                {
                    Direction = ParameterDirection.ReturnValue
                };

                cmd.Parameters.Add(valorRetorno);
                await cmd.ExecuteNonQueryAsync();

                rowIdResponse = (int)valorRetorno.Value;
            }

            return rowIdResponse;
        }

        public async Task<int> UpdateItemAsync(Producto product)
        {
            int rowIdResponse = -1;
            using (var conexion = new SqlConnection(connections.CadenaSql))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new("sp_ActualizarProducto", conexion);
                cmd.Parameters.AddWithValue("@ProductoId", product.ProductoId);
                cmd.Parameters.AddWithValue("@TipoProductoId", product.TipoProductoId);
                cmd.Parameters.AddWithValue("@NombreProducto", product.Nombre);
                cmd.Parameters.AddWithValue("@ClaveProducto", product.ClaveInterna);
                cmd.Parameters.AddWithValue("@ActivoProducto", product.Activo);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter valorRetorno = new()
                {
                    Direction = ParameterDirection.ReturnValue
                };

                cmd.Parameters.Add(valorRetorno);
                await cmd.ExecuteNonQueryAsync();

                rowIdResponse = (int)valorRetorno.Value;
            }

            return rowIdResponse;
        }

        public async Task<int> UpdatePriceAsync(Producto product)
        {
            int response = -1;
            using (var conexion = new SqlConnection(connections.CadenaSql))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new("sp_ActualizarProductoPrecio", conexion);
                cmd.Parameters.AddWithValue("@ProductoId", product.ProductoId);
                cmd.Parameters.AddWithValue("@ClaveProducto", product.ClaveInterna);
                cmd.Parameters.AddWithValue("@PrecioProducto", product.Precio);
                cmd.CommandType = CommandType.StoredProcedure;

                response = await cmd.ExecuteNonQueryAsync();
            }

            return response;
        }

        public async Task<int> DeleteByParamsAsync(Producto productEntity)
        {
            int response = 0;
            using (var conexion = new SqlConnection(connections.CadenaSql))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new("sp_EliminarActualizarProducto", conexion);
                cmd.Parameters.AddWithValue("@ProductoId", productEntity.ProductoId);
                cmd.Parameters.AddWithValue("@ClaveProducto", productEntity.ClaveInterna!.TrimEnd());
                cmd.CommandType = CommandType.StoredProcedure;

                response = await cmd.ExecuteNonQueryAsync();
            }

            return response;
        }
        
    }
}
