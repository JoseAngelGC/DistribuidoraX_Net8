using DistribuidoraX.Shared.Dtos.TypeProductDtos;
using DistribuidoraX.Shared.Responses;

namespace DistribuidoraX.Shared.Services.TypeProductServices
{
    public interface ITypeProductQueriesService
    {
        Task<IGenericResult<List<TypeProductDto>>> GetListAsync();
    }
}
