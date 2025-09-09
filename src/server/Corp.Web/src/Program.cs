using System.Net.Mime;
using System.Threading.RateLimiting;

using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Net.Http.Headers;

using Hangfire;
using Scalar.AspNetCore;
using Vite.AspNetCore;

using Corp.Data;
using Corp.Identity;
using Corp.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSeq();

builder.Services.AddDatabases(builder.Configuration);
builder.Services.AddServices();
builder.Services.AddTaskServices();
builder.Services.AddForegroundServices();

builder.Services.AddAuthorizationBuilder()
    .AddDefaultPolicy(AppPolicy.RequireUserRole, policy => policy.RequireAuthenticatedUser())
    .AddPolicy(AppPolicy.RequireUserRole, policy => policy.RequireRole(AppRole.User))
    .AddPolicy(AppPolicy.RequireAdminRole, policy => policy.RequireRole(AppRole.Admin));

// https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-9.0&tabs=visual-studio#adddefaultidentity-and-addidentity
//builder.Services.AddDefaultIdentity<IdentityUser>() // UI
builder.Services.AddIdentityApiEndpoints<ApplicationUser>( // API
        options => options.SignIn.RequireConfirmedAccount = true)
    //.AddSignInManager<TenantSignInManager>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<CorpDbContext>();

builder.Services.AddOptions<BearerTokenOptions>(IdentityConstants.BearerScheme).Configure(options =>
{
    // options.BearerTokenExpiration = TimeSpan.FromSeconds(60);
});

builder.Services.Configure<IdentityOptions>(builder.Configuration.GetSection(nameof(IdentityOptions)));

builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseDefaultHangfireStorage(builder.Configuration));

builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => policy
        .AllowAnyMethod()
        .WithOrigins(
            "http://localhost:8383", // app
            "http://localhost:8484", // sys
            "http://localhost:5173") // Vite default port
        .WithHeaders(
            HeaderNames.Authorization,
            HeaderNames.ContentType));
});

var rateOptions = new AppRateLimitOptions();
builder.Configuration.GetSection(AppRateLimitOptions.AppRateLimit).Bind(rateOptions);
const string fixedRatePolicy = "FixedPolicy";

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy(policyName: fixedRatePolicy, partitioner: httpContext =>
    {
        string? userId = httpContext.User.GetNameIdentifierOrDefault();

        return string.IsNullOrEmpty(userId)
            ? RateLimitPartition.GetFixedWindowLimiter("anonymous", _ =>
                new FixedWindowRateLimiterOptions
                {
                    PermitLimit = rateOptions.PermitLimit,
                    Window = TimeSpan.FromSeconds(rateOptions.Window),
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = rateOptions.QueueLimit,
                })
            : RateLimitPartition.GetFixedWindowLimiter(userId, _ =>
                new FixedWindowRateLimiterOptions
                {
                    PermitLimit = rateOptions.PermitLimit,
                    Window = TimeSpan.FromSeconds(rateOptions.Window),
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = 0, // Set to 0 to disable queuing
                    // QueueLimit = rateOptions.QueueLimit,
                });
    });
});

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddControllers();
//builder.Services.AddControllersWithViews()
//    ;

builder.Services.AddProblemDetails();

builder.Services.AddViteServices(options =>
{
    options.Server.Port = 8383;
});

builder.Services.AddTransient<IEmailSender, AuthSendGridSender>();
builder.Services.Configure<AuthSendGridOptions>(builder.Configuration.GetSection(nameof(AuthSendGridOptions)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRouting();

// https://learn.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-9.0
// Usually called before UseStaticFiles.
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseRateLimiter();
//app.MapStaticAssets();

// Proxy to the Vite Development Server.
if (app.Environment.IsDevelopment())
{
    // WebSockets support is required for HMR (hot module reload).
    // Uncomment the following line if your pipeline doesn't contain it.
    app.UseWebSockets();

    // Enable all required features to use the Vite Development Server.
    // Pass true if you want to use the integrated middleware.
    //app.UseViteDevelopmentServer(/* false */);
    app.UseViteDevelopmentServer(true);
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();               // http://localhost:8181/openapi/v1.json
    app.MapScalarApiReference();    // http://localhost:8181/scalar/v1
}

app.MapGroup("/api/auth")
    .WithTags("Auth")
    .MapIdentityApi<ApplicationUser>();

app.MapDefaultControllerRoute()
    //.RequireRateLimiting(fixedRatePolicy)
    .WithStaticAssets();
//app.MapRazorPages();

// Attribute routing (API) endpoints.
app.MapControllers()
    .RequireRateLimiting(fixedRatePolicy);

#pragma warning disable ASP0014
// TODO: Convert to top-level Map calls off of app.
// TODO: Secure with policy.
app.UseEndpoints(endpoints =>
{
    endpoints.MapHangfireDashboard();   // http://localhost:8181/hangfire
});
#pragma warning restore ASP0014

#if DEBUG
app.MapGet(
    "/",
    () =>
        Results.Content("""
            <!doctype html>
            <html lang='en'>
            <title>Corp</title>
            <body>
                <div><a href="http://localhost:8383">App</a></div>
                <div><a href="http://localhost:8484">Admin</a></div>
                <div><a href="http://localhost:8181/hangfire">Hangfire</a></div>
                <div><a href="http://localhost:8181/scalar/v1">Scalar API</a></div>
                <div><a href="http://localhost:8181/openapi/v1.json">OpenAPI JSON</a></div>
            </body>
            </html>
            """,
        MediaTypeNames.Text.Html))
    .ExcludeFromDescription();
#endif

app.Run();

// Required for unit test project.
public partial class Program { }
