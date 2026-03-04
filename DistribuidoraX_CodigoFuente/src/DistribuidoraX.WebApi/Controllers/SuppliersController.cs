using DistribuidoraX.Shared.Services.SupplierServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DistribuidoraX.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierQueriesService _supplierQueriesService;
        public SuppliersController(ISupplierQueriesService supplierQueriesService)
        {
            _supplierQueriesService = supplierQueriesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetListAsync()
        {
            var response = await _supplierQueriesService.GetListAsync();
            return StatusCode(response.StatusCode, response);
        }
    }
}
