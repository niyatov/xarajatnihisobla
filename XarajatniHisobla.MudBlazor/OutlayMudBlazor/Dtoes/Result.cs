namespace OutlayMudBlazor.Dtoes;


public class Result
{
    public string? ErrorMessage { get; set; } //for users
    public string? ErrorDetails { get; set; }
    public bool IsNotFound { get; set; }
    public bool IsSuccess { get; set; }
    public Result(bool isSuccess)
    {
        IsSuccess = isSuccess;
    }
}


public class Result<T> : Result
{
    public T? Data { get; set; }
    public Result(bool isSuccess) : base(isSuccess) { }
}
