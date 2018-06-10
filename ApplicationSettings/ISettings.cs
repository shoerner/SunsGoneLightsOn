using Newtonsoft.Json;

public class ISettings
{
    [JsonProperty("clockSkew")]
    public string ClockSkew { get; set; }

    [JsonProperty("cloudUserName")]
    public string CloudUserName { get; set; }

    [JsonProperty("cloudPassword")]
    public string CloudPassword { get; set; }

    [JsonProperty("terminalUUID")]
    public string TerminalUUID { get; set; }

    [JsonProperty("localLatitude")]
    public double LocalLatitude { get; set; }

    [JsonProperty("localLongitude")]
    public double LocalLongitude { get; set; }
}