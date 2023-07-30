namespace OutlayApi.Dtoes;

public class Outlay
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime? LastUpdateAt { get; set; }
    public int Cost { get; set; }
    public string Username { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsOwner { get; set; }
}
