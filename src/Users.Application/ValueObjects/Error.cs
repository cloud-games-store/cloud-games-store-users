using System.Net;

namespace Users.Application.ValueObjects;

public record Error
{
    public HttpStatusCode StatusCode { get; private set; }
    public string ErrorMessage { get; private set; }

    public Error(HttpStatusCode statusCode, string errors)
    {
        StatusCode = statusCode;
        ErrorMessage = errors;
    }

    public static Error BadRequest(string message) => new Error(HttpStatusCode.BadRequest, message);
    public static Error Unauthorized(string message) => new Error(HttpStatusCode.Unauthorized, message);
    public static Error InternalServerError(string message) => new Error(HttpStatusCode.InternalServerError, message);
}