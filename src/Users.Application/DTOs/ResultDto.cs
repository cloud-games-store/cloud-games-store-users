using Users.Application.ValueObjects;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Users.Application.DTOs;

public class ResultDto
{
    public bool Success { get; init; }
    public ValueObjects.Error? Error { get; init; }
    public string? Message { get; init; }

    public static ResultDto Ok(string? message = null) => new ResultDto { Success = true, Message = message };
    public static ResultDto Fail(ValueObjects.Error? error) => new ResultDto { Success = false, Error = error };
}

public class ResultDto<T>
{
    public bool Success { get; init; }
    public T? Data { get; init; }
    public ValueObjects.Error? Error { get; init; }
    public string? Message { get; init; }

    public static ResultDto<T> Ok(T data, string? message = null) => new ResultDto<T> { Success = true, Data = data, Message = message };
    public static ResultDto<T> Fail(ValueObjects.Error error) => new ResultDto<T> { Success = false, Error = error };
}
