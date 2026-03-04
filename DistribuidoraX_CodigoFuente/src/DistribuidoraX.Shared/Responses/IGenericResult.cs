
namespace DistribuidoraX.Shared.Responses
{
    public interface IGenericResult<T>
    {
        public string? Message { get; }
        public T? Value { get; }
        public bool IsSuccess { get; }
        public int StatusCode { get; }
        public string? Error { get; }

        IGenericResult<T> Success(string? message, T? value);
        IGenericResult<T> Failure_BadRequest(string? message, T? value, string? error);
        IGenericResult<T> Failure_NotFound(string? message, T? value, string? error);
        IGenericResult<T> Failure_InternalServerError(string? message, T? value, string? error);
    }
}
