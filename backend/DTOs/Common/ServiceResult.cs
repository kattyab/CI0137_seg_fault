namespace Backend.Api.DTOs.Common;

public class ServiceResult<T>
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public T? Data { get; set; }

    public static ServiceResult<T> Ok(T data)
    {
        return new ServiceResult<T>
        {
            Success = true,
            Data = data
        };
    }

    public static ServiceResult<T> Fail(string error, T? data = default)
    {
        return new ServiceResult<T>
        {
            Success = false,
            Error = error,
            Data = data
        };
    }
}
