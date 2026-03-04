using DistribuidoraX.Domain.Entities;

namespace DistribuidoraX.Domain.Abstractions.UseCases.SupplierUseCases
{
    public interface IGetSupplierListUseCase
    {
        Task<List<Proveedor>> GetListAsync();
    }
}
