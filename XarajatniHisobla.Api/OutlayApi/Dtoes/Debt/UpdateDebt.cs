using System.ComponentModel.DataAnnotations;

namespace OutlayApi.Dtoes;

public class UpdateDebt
{
    public int? Cost { get; set; }
    public string? Description { get; set; }
    public string ReceiverName { get; set; }
    public string GivenPersonName { get; set; }
    public DateTime? LastAt { get; set; }
}
