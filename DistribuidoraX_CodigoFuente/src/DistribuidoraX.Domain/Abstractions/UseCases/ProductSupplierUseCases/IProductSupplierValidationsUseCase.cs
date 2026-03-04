using DistribuidoraX.Domain.Objects.GenericObjects;

namespace DistribuidoraX.Domain.Abstractions.UseCases.ProductSupplierUseCases
{
    public interface IProductSupplierValidationsUseCase
    {
        Task<bool> ExistProductSupplierCode_StoredProceduresSqlServerAsync(ExistCodeParameters productSupplierCodeParameters);
    }
}
