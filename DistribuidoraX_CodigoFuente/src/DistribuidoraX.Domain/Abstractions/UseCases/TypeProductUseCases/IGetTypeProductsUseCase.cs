using DistribuidoraX.Domain.Entities;

namespace DistribuidoraX.Domain.Abstractions.UseCases.TypeProductUseCases
{
    public interface IGetTypeProductsUseCase
    {
        Task<List<TipoProducto>> GetListAsync();
    }
}
