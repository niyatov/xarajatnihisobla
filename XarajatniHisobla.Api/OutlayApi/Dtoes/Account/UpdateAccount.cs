using System.ComponentModel.DataAnnotations;

namespace OutlayApi.Dtoes;

public class UpdateAccount
{
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }
}
