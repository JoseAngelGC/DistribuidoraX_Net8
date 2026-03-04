using DistribuidoraX.Domain.Abstractions.Repositories.SupplierRepositories;
using DistribuidoraX.Domain.Abstractions.UseCases.SupplierUseCases;
using DistribuidoraX.Domain.Entities;

namespace DistribuidoraX.ApplicationServices.UseCases.SupplierUseCases
{
    public class GetSupplierListUseCase : IGetSupplierListUseCase
    {
        private readonly ISupplierRepository_StoredProceduresSqlServer _supplierRepositoryStoredProceduresSqlServer;
        public GetSupplierListUseCase(ISupplierRepository_StoredProceduresSqlServer supplierRepositoryStoredProceduresSqlServer)
        {
            _supplierRepositoryStoredProceduresSqlServer = supplierRepositoryStoredProceduresSqlServer;
        }

        public async Task<List<Proveedor>> GetListAsync()
        {
            return await _supplierRepositoryStoredProceduresSqlServer.GetSupplierListAsync();
        }
    }
}
