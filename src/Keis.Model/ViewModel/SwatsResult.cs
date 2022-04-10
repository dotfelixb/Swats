using System.Text.Json.Serialization;

namespace Keis.Model.ViewModel;

public abstract class KeisResult
{
    [JsonPropertyName("ok")] public bool Ok { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("errors")]
    public IEnumerable<string> Errors { get; set; }

    [JsonPropertyName("ts")] public long Ts { get; set; } = DateTimeOffset.Now.ToUnixTimeMilliseconds();
}

public class ErrorResult : KeisResult
{
}