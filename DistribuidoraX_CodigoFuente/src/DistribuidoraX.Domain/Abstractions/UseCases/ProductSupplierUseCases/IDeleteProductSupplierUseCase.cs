using DistribuidoraX.Domain.Entities;

namespace DistribuidoraX.Domain.Abstractions.UseCases.ProductSupplierUseCases
{
    public interface IDeleteProductSupplierUseCase
    {
        Task<int> DeleteItemByDeletedStateUpdating_StoredProceduresSqlServerAsync(ProductoProveedor productSupplierEntity);
    }
}
