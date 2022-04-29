using System.Text.Json.Serialization;

namespace Keis.Model;

public abstract class KeisResult
{
    [JsonPropertyName("ok")]
    public bool Ok { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("errors")]
    public IEnumerable<string> Errors { get; set; }

    [JsonPropertyName("ts")]
    public long Ts { get; set; } = DateTimeOffset.Now.ToUnixTimeMilliseconds();
}

public class SingleResult<T> : KeisResult
{
    [JsonPropertyName("data")]
    public T Data { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = "single";
}

public class ListResult<T> : KeisResult
{
    [JsonPropertyName("data")]
    public IEnumerable<T> Data { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = "list";
}

public class AuthResult : KeisResult
{
    public string Fullname { get; set; }
    public string Token { get; set; }
    public string[] Permissions { get; set; }
}

public class ErrorResult : KeisResult
{ }