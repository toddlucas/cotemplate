using System.Text.Json;
using System.Text.Json.Nodes;

namespace Corp.Web.Test;

public class WebApplicationFactoryFixture
{
    public WebApplicationFactory<Program> Factory { get; set; } = new();
    public WebApplicationFactoryFixture()
    {
        // The app runs in: Upscale.Web/src but
        // the test runs in: Corp.Web/test/bin/Debug/net9.0,
        // so we need a different relative reference.
        Environment.SetEnvironmentVariable("ConnectionStrings:SqliteCorpDbContextConnection", "Data Source=../../../../../Corp.Web/src/myproject.db");
        Environment.SetEnvironmentVariable("ConnectionStrings:SqliteMyOtherDbContextConnection", "Data Source=../../../../../Corp.Web/src/myother.db");
        Environment.SetEnvironmentVariable("ConnectionStrings:SqliteDefaultHangfireConnection", "../../../../../Corp.Background/src/background.db");
    }
}

[CollectionDefinition(Name)]
public class WebApplicationFactoryCollection : ICollectionFixture<WebApplicationFactoryFixture>
{
    public const string Name = "WebApplicationFactory Collection";
}
