using System.Net;

namespace Resume.Application.Utility;

public class BaseResult {
    public string Message { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public bool IsSuccess { get; set; }
}
public class BaseResult<T> {
    public string Message { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public bool IsSuccess { get; set; }
    public T Data { get; set; }
}