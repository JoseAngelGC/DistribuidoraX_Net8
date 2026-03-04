using DistribuidoraX.Domain.Entities;

namespace DistribuidoraX.Domain.Abstractions.UseCases.SupplierUseCases
{
    public interface IGetSupplierUseCase
    {
        Task<Proveedor> GetByIdAsync(int id);
    }
}
