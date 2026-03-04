using DistribuidoraX.Domain.Abstractions.Repositories.SupplierRepositories;
using DistribuidoraX.Domain.Abstractions.UseCases.SupplierUseCases;
using DistribuidoraX.Domain.Entities;

namespace DistribuidoraX.ApplicationServices.UseCases.SupplierUseCases
{
    public class GetSupplierUseCase : IGetSupplierUseCase
    {
        private readonly ISupplierRepository_StoredProceduresSqlServer _supplierRepositoryStoredProceduresSqlServer;
        public GetSupplierUseCase(ISupplierRepository_StoredProceduresSqlServer supplierRepositoryStoredProceduresSqlServer)
        {
            _supplierRepositoryStoredProceduresSqlServer = supplierRepositoryStoredProceduresSqlServer;
        }

        public async Task<Proveedor> GetByIdAsync(int id)
        {
            return await _supplierRepositoryStoredProceduresSqlServer.GetSupplierByIdAsync(id);
        }
    }
}
