namespace OutlayApi.Models;

public class Debts
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public int Cost { get; set; }
    public string SenderName { get; set; }
    public string ReceiverName { get; set; }
    public string GivenPersonName { get; set; }
    public string? DeleteSenderName { get; set; }
    public EDebtType DebtType { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime? LastAt { get; set; }
}
