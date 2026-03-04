
using DistribuidoraX.Shared.Responses;
using System.Net;

namespace DistribuidoraX.SitioWeb.Responses
{
    public class GenericResult<T> : IGenericResult<T>
    {
        public string? Message { get; set; }
        public T? Value { get; set; }
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string? Error { get; set; }

        public GenericResult() { }

        private GenericResult(string? message, T? value, bool isSuccess, int statusCode, string? error)
        {
            Message = message;
            Value = value;
            IsSuccess = isSuccess;
            StatusCode = statusCode;
            Error = error;
        }

        public IGenericResult<T> Success(string? message, T? value) => new GenericResult<T>(message, value, true, (int)HttpStatusCode.OK, null);
        public IGenericResult<T> Failure_BadRequest(string? message, T? value, string? error) => new GenericResult<T>(message, value, false, (int)HttpStatusCode.BadRequest, error);
        public IGenericResult<T> Failure_NotFound(string? message, T? value, string? error) => new GenericResult<T>(message, value, false, (int)HttpStatusCode.NotFound, error);
        public IGenericResult<T> Failure_InternalServerError(string? message, T? value, string? error) => new GenericResult<T>(message, value, false, (int)HttpStatusCode.InternalServerError, error);
    }
}
