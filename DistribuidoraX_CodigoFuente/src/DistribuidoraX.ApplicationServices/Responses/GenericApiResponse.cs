using DistribuidoraX.Domain.Abstractions.Responses;

namespace DistribuidoraX.ApplicationServices.Responses
{
    public class GenericApiResponse<T> : IGenericApiResponse<T>
    {
        public string? Mensaje { get; set; }
        public T? Result { get; set; }
        public bool Exitoso { get; set; }
        public int Estado { get; set; }
    }
}
