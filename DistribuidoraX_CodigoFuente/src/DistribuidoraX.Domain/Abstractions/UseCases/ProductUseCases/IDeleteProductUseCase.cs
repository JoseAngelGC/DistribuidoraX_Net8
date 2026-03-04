using DistribuidoraX.Domain.Entities;

namespace DistribuidoraX.Domain.Abstractions.UseCases.ProductUseCases
{
    public interface IDeleteProductUseCase
    {
        Task<int> DeleteByParams_StoredProceduresSqlServerAsync(Producto product);
    }
}
