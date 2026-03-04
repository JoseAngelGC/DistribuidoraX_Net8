using DistribuidoraX.Domain.Abstractions.Repositories.TypeProductRepositories;
using DistribuidoraX.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

namespace DistribuidoraX.Infraestructure.Repositories.TypeProductRepositories
{
    internal class QueriesTypeProductRepository(IOptions<ConnectionStrings> options) : ITypeProductRepository_StoredProceduresSqlServer
    {
        private readonly ConnectionStrings _connections = options.Value;

        public async Task<List<TipoProducto>> GetListAsync()
        {
            List<TipoProducto> listaTipoProducto = [];
            using (var conexion = new SqlConnection(_connections.CadenaSql))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new("sp_ObtenerListaTipoProducto", conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };

                using var reader = await cmd.ExecuteReaderAsync();
                {
                    while (reader.Read())
                    {
                        listaTipoProducto.Add(new TipoProducto
                        {
                            TipoProductoId = Convert.ToInt32(reader["TipoProductoId"]),
                            Nombre = reader["TipoProductoNombre"].ToString()
                        });
                    }
                }

            }

            return listaTipoProducto;
        }
    }
}
