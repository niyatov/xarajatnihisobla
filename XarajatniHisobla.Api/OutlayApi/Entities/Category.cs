namespace OutlayApi.Entities;

public class Category : EntityBase
{
    public string Name { get; set; }
    public string? Key { get; set; }
    public DateTime? LastUpdateAt { get; set; }
    public bool IsPrivate { get; set; }
    public virtual List<Outlay>? Outlays { get; set; }
    public virtual List<UserCategory>? UserCategories { get; set; }
}
