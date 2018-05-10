using Serilog;
using System;

namespace HighScoreApi.Configuration
{
    public static class SerilogConfiguration
    {
        public static void Configure(SerilogSettings settings)
        {
            var log = new LoggerConfiguration()
                .WriteTo.RollingFile("log-{Date}.txt")
                .MinimumLevel.Is(Enum.Parse<Serilog.Events.LogEventLevel>(settings.LogLevel))
                .CreateLogger();
        }
    }

    public class SerilogSettings
    {
        public string LogLevel { get; set; }
    }
}
