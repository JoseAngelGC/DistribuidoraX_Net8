using DistribuidoraX.Shared.Dtos.GenericDtos;
using DistribuidoraX.Shared.Services.ProductSupplierServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DistribuidoraX.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductSuppliersController : ControllerBase
    {
        private readonly IProductSupplierQueriesService _productSupplierQueriesService;
        public ProductSuppliersController(IProductSupplierQueriesService productSupplierQueriesService)
        {
            _productSupplierQueriesService = productSupplierQueriesService;
        }

        [HttpGet("Product/{id}")]
        public async Task<IActionResult> GetByProductIdAsync(int id)
        {
            var response = await _productSupplierQueriesService.GetListByProductIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("ExistCode")]
        public async Task<IActionResult> AddProductAsync([FromBody] ExistCodeParametersDto productSupplierCodeParametersDto)
        {
            var response = await _productSupplierQueriesService.ExistProductSupplierCodeAsync(productSupplierCodeParametersDto);
            return StatusCode(response.StatusCode, response);
        }
    }
}
