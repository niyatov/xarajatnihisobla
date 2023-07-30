namespace OutlayApi;

public class SettingOptions
{
    public const string Setting = "Setting";

    public string BotToken { get; set; } = String.Empty;
    public string ChatId { get; set; } = String.Empty;
    public string ConnectionString { get; set; } = String.Empty;
    public string Origins { get; set; } = String.Empty;
}
