using DistribuidoraX.Domain.Entities;

namespace DistribuidoraX.Domain.Abstractions.UseCases.ProductSupplierUseCases
{
    public interface IEditProductSupplierUseCase
    {
        Task<int> UpdateItem_StoredProceduresSqlServerAsync(ProductoProveedor productSuplierEntity, bool revertFlag);
    }
}
