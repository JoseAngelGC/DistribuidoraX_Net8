using DistribuidoraX.Domain.Entities;

namespace DistribuidoraX.Domain.Abstractions.Repositories.SupplierRepositories
{
    public interface ISupplierRepository_StoredProceduresSqlServer
    {
        Task<List<Proveedor>> GetSupplierListAsync();
        Task<Proveedor> GetSupplierByIdAsync(int id);
    }
}
