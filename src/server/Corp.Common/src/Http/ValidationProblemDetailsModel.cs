using System.Text.Json.Serialization;

namespace Corp.Http;

/// <summary>
/// A validation ProblemDetailsModel.
/// </summary>
public class ValidationProblemDetailsModel : ProblemDetailsModel
{
    /// <summary>
    /// Contains validation related errors from the extensions.
    /// </summary>
    [JsonPropertyName("errors")]
    public IDictionary<string, string[]>? Errors { get; set; }
}
