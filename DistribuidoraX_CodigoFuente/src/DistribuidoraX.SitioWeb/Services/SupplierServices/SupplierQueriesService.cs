using DistribuidoraX.Shared.Dtos.SupplierDtos;
using DistribuidoraX.Shared.Dtos.TypeProductDtos;
using DistribuidoraX.Shared.Responses;
using DistribuidoraX.Shared.Services.SupplierServices;
using DistribuidoraX.SitioWeb.Responses;
using Newtonsoft.Json;

namespace DistribuidoraX.SitioWeb.Services.SupplierServices
{
    public class SupplierQueriesService : ISupplierQueriesService
    {
        private static string? _baseUrl;
        public SupplierQueriesService()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            _baseUrl = builder.GetSection("ApiSettings:baseUrl").Value;
        }

        public async Task<IGenericResult<List<SupplierDto>>> GetListAsync()
        {
            var cliente = new HttpClient
            {
                BaseAddress = new Uri(_baseUrl!)
            };

            var responseApi = await cliente.GetAsync("api/Suppliers");
            var jsonRespuesta = await responseApi.Content.ReadAsStringAsync();
            GenericResult<List<SupplierDto>> resultado = JsonConvert.DeserializeObject<GenericResult<List<SupplierDto>>>(jsonRespuesta)!;

            return resultado;
        }
    }
}
