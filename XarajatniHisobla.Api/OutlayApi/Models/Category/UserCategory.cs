namespace OutlayApi.Models;

public class UserCategory
{
    public int CategoryId { get; set; }
    public int UserId { get; set; }
    public bool IsAdmin { get; set; }
}
