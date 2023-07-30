using System.Net;
using System.Text;

namespace OutlayMudBlazor.Dtoes;

public static class SendTelegramMessage
{
    public static async Task SendTelegram(string? message, IConfiguration configuration)
    {
        var botToken = configuration["BotToken"];
        var chatId = configuration["ChatId"];

        using (var httpClient = new HttpClient())
        {
            var url = $"https://api.telegram.org/bot{botToken}/sendMessage";
            var content = new StringContent($"chat_id={chatId}&text={WebUtility.UrlEncode(message)}", Encoding.UTF8, "application/x-www-form-urlencoded");
            var response = await httpClient.PostAsync(url, content);
        }
    }
}
