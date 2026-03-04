using DistribuidoraX.Shared.Dtos.SupplierDtos;
using DistribuidoraX.Shared.Responses;

namespace DistribuidoraX.Shared.Services.SupplierServices
{
    public interface ISupplierQueriesService
    {
        Task<IGenericResult<List<SupplierDto>>> GetListAsync();
    }
}
