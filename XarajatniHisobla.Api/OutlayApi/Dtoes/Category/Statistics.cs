namespace OutlayApi.Dtoes;

public class Statistics
{
    public int CategoryId { get; set; }
    public bool IsPrivate { get; set; }
    public bool IsAdmin { get; set; }
    public List<UsersResult>? UsersResults { get; set; }
    public int TotalSum { get; set; }
    public int NumberOfYourOutlays { get; set; }
    public int NumberOfOutlays { get; set; }
    public int NumberOfPeople { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public int SpentMoney { get; set; }
    public int ResultMoney { get; set; }
}
