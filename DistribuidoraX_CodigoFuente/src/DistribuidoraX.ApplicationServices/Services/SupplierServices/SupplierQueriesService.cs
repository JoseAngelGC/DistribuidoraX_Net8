using DistribuidoraX.Domain.Abstractions.UseCases.SupplierUseCases;
using DistribuidoraX.Shared.Dtos.SupplierDtos;
using DistribuidoraX.Shared.Responses;
using DistribuidoraX.Shared.Services.SupplierServices;
using DistribuidoraX.Shared.Utilities;

namespace DistribuidoraX.ApplicationServices.Services.SupplierServices
{
    public class SupplierQueriesService : ISupplierQueriesService
    {
        private readonly IGetSupplierListUseCase _getSuppliersUseCase;
        private readonly IGenericResult<List<SupplierDto>> _genericResultSupplierList;
        public SupplierQueriesService(IGetSupplierListUseCase getSuppliersUseCase, IGenericResult<List<SupplierDto>> genericResultSupplierList)
        {
            _getSuppliersUseCase = getSuppliersUseCase;
            _genericResultSupplierList = genericResultSupplierList;
        }
        public async Task<IGenericResult<List<SupplierDto>>> GetListAsync()
        {
            try
            {
                var supplierListResponse = await _getSuppliersUseCase.GetListAsync();
                List<SupplierDto> supplierListDto = [];
                if (supplierListResponse.Count > 0)
                {
                    foreach (var supplierItem in supplierListResponse)
                    {
                        supplierListDto.Add(new SupplierDto
                        {
                            SupplierItem = supplierItem.ProveedorId,
                            SupplierName = supplierItem.Nombre
                        });
                    }
                }


                if (supplierListDto.Count == 0)
                {
                    return _genericResultSupplierList.Success(ReplyMessages.EMPTY_QUERY, supplierListDto);
                }

                return _genericResultSupplierList.Success(ReplyMessages.SUCCESSFULL_QUERY, supplierListDto);
            }
            catch (Exception)
            {
                return _genericResultSupplierList.Failure_InternalServerError(ReplyMessages.FAILED_OPERATION, null, null);
            }
        }
    }
}
