namespace EventApp.App.Responses;

public interface IStatusResponse
{
    public int Code { get; set; }
    public string Message { get; set; }
}