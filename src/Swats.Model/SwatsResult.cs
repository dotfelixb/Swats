using System.Text.Json.Serialization;

namespace Swats.Model;

public abstract class SwatsResult
{
    [JsonPropertyName("ok")]
    public bool Ok { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("type")]
    public string Type { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("errors")]
    public IEnumerable<string> Errors { get; set; }
    [JsonPropertyName("ts")]
    public long Ts { get; set; } = DateTimeOffset.Now.ToUnixTimeMilliseconds();
}

public class SingleResult<T> : SwatsResult
{
    [JsonPropertyName("data")]
    public T Data { get; set; }
}

public class ListResult<T> : SwatsResult
{
    [JsonPropertyName("data")]
    public IEnumerable<T> Data { get; set; }
}

public class AuthResult : SwatsResult
{
    public string Fullname { get; set; }
    public string Token { get; set; }
    public string[] Permissions { get; set; }
}

public class ErrorResult : SwatsResult { }