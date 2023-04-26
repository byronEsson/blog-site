namespace BlogAPI.Services;

public class ServiceResponse<T>
{
    public T? Data { get; set; }
    public bool WasSuccessful { get; set; } = true;
    public string Message { get; set; } = string.Empty;
}
