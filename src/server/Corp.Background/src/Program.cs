using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Hangfire;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
// builder.Services.AddHostedService<HangfireWorker>();

builder.Logging.AddSeq();

builder.Services.AddDatabases(builder.Configuration);
builder.Services.AddServices();
//builder.Services.AddServices(builder.Configuration);
builder.Services.AddTaskServices();
builder.Services.AddBackgroundServices();

builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseDefaultHangfireStorage(builder.Configuration));

builder.Services.AddHangfireServer();

IHost host = builder.Build();

host.Run();
