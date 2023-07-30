namespace OutlayApi.Exceptions;

public class NotFoundException : Exception
{
    public int ErrorCode { get; set; }
    public NotFoundException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}

public class NotFoundException<T> : NotFoundException
{
    public NotFoundException(int errorCode) : base($"Bunday obyekt {typeof(T).Name} topilmadi.", errorCode)
    { }
}