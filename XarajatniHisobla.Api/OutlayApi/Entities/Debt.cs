namespace OutlayApi.Entities;

public class Debt : EntityBase
{
    public int Cost { get; set; }
    public int SenderId { get; set; }
    public int GivenPersonId { get; set; }
    public int ReceiverId { get; set; }
    public EDebtType DebtType { get; set; }
    public int? DeleteSenderId { get; set; }
    public DateTime? LastAt { get; set; }
}

