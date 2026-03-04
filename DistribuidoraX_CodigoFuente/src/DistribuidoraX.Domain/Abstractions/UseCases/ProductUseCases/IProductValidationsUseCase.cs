
using DistribuidoraX.Domain.Objects.GenericObjects;

namespace DistribuidoraX.Domain.Abstractions.UseCases.ProductUseCases
{
    public interface IProductValidationsUseCase
    {
        Task<bool> ExistProductCode_StoredProceduresSqlServerAsync(ExistCodeParameters productCodeParameters);
    }
}
