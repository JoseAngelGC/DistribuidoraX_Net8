using DistribuidoraX.Domain.Abstractions.Repositories.TypeProductRepositories;
using DistribuidoraX.Domain.Abstractions.UseCases.TypeProductUseCases;
using DistribuidoraX.Domain.Entities;

namespace DistribuidoraX.ApplicationServices.UseCases.TypeProductUseCases
{
    public class GetTypeProductsUseCase : IGetTypeProductsUseCase
    {
        private readonly ITypeProductRepository_StoredProceduresSqlServer _queriesTypeProductRepository;
        public GetTypeProductsUseCase(ITypeProductRepository_StoredProceduresSqlServer queriesTypeProductRepository)
        {
            _queriesTypeProductRepository = queriesTypeProductRepository;
        }

        public async Task<List<TipoProducto>> GetListAsync()
        {
            return await _queriesTypeProductRepository.GetListAsync();
        }
    }
}
