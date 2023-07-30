using Microsoft.AspNetCore.Identity;

namespace OutlayApi.Entities;

public class User : IdentityUser<int>
{
    public Byte[]? Bytes { get; set; }
    public virtual List<UserCategory>? UserCategories { get; set; }
    public virtual List<Outlay>? Outlays { get; set; }
}