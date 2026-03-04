using DistribuidoraX.Domain.Entities;

namespace DistribuidoraX.Domain.Abstractions.Repositories.TypeProductRepositories
{
    public interface ITypeProductRepository_StoredProceduresSqlServer
    {
        Task<List<TipoProducto>> GetListAsync();
    }
}
