using DistribuidoraX.Domain.Entities;

namespace DistribuidoraX.Domain.Abstractions.UseCases.ProductUseCases
{
    public interface INewProductUseCase
    {
        Task<int> SaveItem_StoredProceduresSqlServerAsync(Producto product);
    }
}
