using System.Net.Http.Json;
using System.Text.Json;

using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity.Data;

namespace Corp.Web.Test;

public class ApiClient(HttpClient client)
{
    private readonly HttpClient _client = client;
    private readonly JsonSerializerOptions _options = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    //public Task<HttpResult<AccessTokenResponse, ProblemDetails>> PostLoginAsync(LoginRequest model, CancellationToken cancellationToken = default)
    //    => _client.PostModelAsync<LoginRequest, AccessTokenResponse, ProblemDetails>("/api/auth/login", model, headers: null, _options, cancellationToken);
}
