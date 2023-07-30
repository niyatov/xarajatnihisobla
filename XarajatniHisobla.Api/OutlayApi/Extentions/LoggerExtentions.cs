using Microsoft.Extensions.Options;
using Serilog;
using TelegramSink;

namespace OutlayApi.Extentions;

public static class LoggerExtensions
{
    public static void SerilogConfig(this WebApplicationBuilder builder, IOptions<SettingOptions> options)
    {
        var botToken = options.Value.BotToken;
        var chatId = options.Value.ChatId;

        var logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.TeleSink(botToken, chatId, minimumLevel: Serilog.Events.LogEventLevel.Error)
            .CreateLogger();

        builder.Logging.AddSerilog(logger);
    }
}