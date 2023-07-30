namespace OutlayApi.Exceptions;

public class BadRequestException : Exception
{
    public int ErrorCode { get; set; }
    public BadRequestException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}
