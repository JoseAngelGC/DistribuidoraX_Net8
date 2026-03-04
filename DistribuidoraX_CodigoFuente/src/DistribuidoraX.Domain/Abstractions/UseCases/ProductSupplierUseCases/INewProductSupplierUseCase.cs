using DistribuidoraX.Domain.Entities;

namespace DistribuidoraX.Domain.Abstractions.UseCases.ProductSupplierUseCases
{
    public interface INewProductSupplierUseCase
    {
        Task<int> SaveItem_StoredProceduresSqlServerAsync(ProductoProveedor productSupplierEntity);
    }
}
