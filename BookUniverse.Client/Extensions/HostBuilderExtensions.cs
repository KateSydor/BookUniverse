namespace BookUniverse.Client.Extensions
{
    using Microsoft.Extensions.Hosting;
    using Serilog;
    using Serilog.Events;
    using Serilog.Formatting.Json;

    public static class HostBuilderExtensions
    {
        public static IHostBuilder ConfigureSerilog(this IHostBuilder builder)
        {
            builder.UseSerilog((host, loggerConfiguration) =>
            {
                loggerConfiguration
                    .WriteTo.File(new JsonFormatter(), "logs-.json", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 5)
                    .WriteTo.Debug()
                    .MinimumLevel.Information()
                    .MinimumLevel.Override("BookUniverse", LogEventLevel.Debug)
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .MinimumLevel.Override("System", LogEventLevel.Warning)
                    .Enrich.FromLogContext();
            });

            return builder;
        }
    }
}
