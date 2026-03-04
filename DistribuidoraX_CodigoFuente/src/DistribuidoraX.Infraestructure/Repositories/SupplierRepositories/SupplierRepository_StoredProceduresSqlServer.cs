using DistribuidoraX.Domain.Abstractions.Repositories.SupplierRepositories;
using DistribuidoraX.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

namespace DistribuidoraX.Infraestructure.Repositories.SupplierRepositories
{
    public class SupplierRepository_StoredProceduresSqlServer(IOptions<ConnectionStrings> options) : ISupplierRepository_StoredProceduresSqlServer
    {
        private readonly ConnectionStrings connections = options.Value;

        
        public async Task<List<Proveedor>> GetSupplierListAsync()
        {
            List<Proveedor> supplierList = [];
            using (var conexion = new SqlConnection(connections.CadenaSql))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new("sp_ObtenerListaProveedor", conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };

                using var reader = await cmd.ExecuteReaderAsync();
                {
                    while (reader.Read())
                    {
                        supplierList.Add(new Proveedor
                        {
                            ProveedorId = Convert.ToInt32(reader["ProveedorId"]),
                            Nombre = reader["ProveedorNombre"].ToString()
                        });
                    }
                }

            }

            return supplierList;
        }

        public async Task<Proveedor> GetSupplierByIdAsync(int id)
        {
            Proveedor supplierEntity = new();
            using (var conexion = new SqlConnection(connections.CadenaSql))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new("sp_ObtenerProveedorPorId", conexion);
                cmd.Parameters.AddWithValue("@ProveedorId", id);
                cmd.CommandType = CommandType.StoredProcedure;

                using var reader = await cmd.ExecuteReaderAsync();
                {
                    while (reader.Read())
                    {
                        supplierEntity.ProveedorId = Convert.ToInt32(reader["ProveedorId"]);
                        supplierEntity.Nombre = reader["Nombre"].ToString();
                    }
                }

            }

            return supplierEntity;
        }
    }
}
