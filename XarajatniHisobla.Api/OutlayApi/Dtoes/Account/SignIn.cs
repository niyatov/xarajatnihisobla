using System.ComponentModel.DataAnnotations;

namespace OutlayApi.Dtoes;

public class SignIn
{
    public string? UsernameOrEmail { get; set; }
    public string? Password { get; set; }
}
