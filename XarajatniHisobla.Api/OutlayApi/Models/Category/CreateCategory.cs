namespace OutlayApi.Models;

public class CreateCategory
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Key { get; set; }
    public List<UserCategory>? UserCategories { get; set; }
}
