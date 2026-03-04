
using DistribuidoraX.Domain.Entities;

namespace DistribuidoraX.Domain.Abstractions.UseCases.ProductUseCases
{
    public interface IEditProductUseCase
    {
        Task<int> UpdateItem_StoredProceduresSqlServerAsync(Producto product);
        Task<int> UpdatePrice_StoredProceduresSqlServerAsync(int productId);
    }
}
