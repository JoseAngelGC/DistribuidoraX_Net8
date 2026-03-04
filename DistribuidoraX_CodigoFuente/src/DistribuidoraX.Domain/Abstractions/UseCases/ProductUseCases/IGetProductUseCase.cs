using DistribuidoraX.Domain.Entities;

namespace DistribuidoraX.Domain.Abstractions.UseCases.ProductUseCases
{
    public interface IGetProductUseCase
    {
        Task<Producto> GetItemById_StoredProceduresSqlServerAsyncAsync(int productId);
    }
}
