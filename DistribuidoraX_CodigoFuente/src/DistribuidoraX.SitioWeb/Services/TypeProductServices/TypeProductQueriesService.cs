using DistribuidoraX.Shared.Dtos.TypeProductDtos;
using DistribuidoraX.Shared.Responses;
using DistribuidoraX.Shared.Services.TypeProductServices;
using DistribuidoraX.SitioWeb.Responses;
using Newtonsoft.Json;

namespace DistribuidoraX.SitioWeb.Services.TypeProductServices
{
    public class TypeProductQueriesService : ITypeProductQueriesService
    {
        private static string? _baseUrl;
        public TypeProductQueriesService()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            _baseUrl = builder.GetSection("ApiSettings:baseUrl").Value;
        }

        public async Task<IGenericResult<List<TypeProductDto>>> GetListAsync()
        {
            var cliente = new HttpClient
            {
                BaseAddress = new Uri(_baseUrl!)
            };

            var responseApi = await cliente.GetAsync("api/TypeProducts");
            var jsonRespuesta = await responseApi.Content.ReadAsStringAsync();
            GenericResult<List<TypeProductDto>> resultado = JsonConvert.DeserializeObject<GenericResult<List<TypeProductDto>>>(jsonRespuesta)!;

            return resultado;
        }
    }
}
