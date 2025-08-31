using Microsoft.Net.Http.Headers;

namespace Corp.Web.Test;

internal static class ApiClientLoginExtensions
{
    public static Dictionary<string, string> CreateBearerHeaders(this ApiClient client, string token)
        => new Dictionary<string, string> { [HeaderNames.Authorization] = $"Bearer {token}" };
}
