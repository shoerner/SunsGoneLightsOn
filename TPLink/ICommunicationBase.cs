
using Newtonsoft.Json;

public class APIBaseRequest
{
    public string method { get; set; }
}

public class APIRequestParams<RequestType> : APIBaseRequest
{
    [JsonProperty("params")]
    public RequestType requestParams { get; set; }
}

public class APIResponse<ResponseType>
{
    public int error_code { get; set; }
    public ResponseType result { get; set; }
}