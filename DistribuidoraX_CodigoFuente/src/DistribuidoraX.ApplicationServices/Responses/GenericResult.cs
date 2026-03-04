using DistribuidoraX.Shared.Responses;
using System.Net;

namespace DistribuidoraX.ApplicationServices.Responses
{
    internal class GenericResult<T> : IGenericResult<T>
    {
        public string? Message { get; private set; }
        public T? Value { get; private set; }
        public bool IsSuccess { get; private set; }
        public int StatusCode { get; private set; }
        public string? Error { get; private set; }

        public GenericResult(){ }

        private GenericResult(string? message, T? value, bool isSuccess, int statusCode, string? error)
        {
            Message = message;
            Value = value;
            IsSuccess = isSuccess;
            StatusCode = statusCode;
            Error = error;
        }

        public IGenericResult<T> Success(string? message, T? value) => new GenericResult<T>(message,value,true, (int)HttpStatusCode.OK,null);
        public IGenericResult<T> Failure_BadRequest(string? message, T? value, string? error) => new GenericResult<T>(message,value,false, (int)HttpStatusCode.BadRequest,error);
        public IGenericResult<T> Failure_NotFound(string? message, T? value, string? error) => new GenericResult<T>(message, value, false, (int)HttpStatusCode.NotFound, error);
        public IGenericResult<T> Failure_InternalServerError(string? message, T? value, string? error) => new GenericResult<T>(message, value, false, (int)HttpStatusCode.InternalServerError, error);
    }
}
