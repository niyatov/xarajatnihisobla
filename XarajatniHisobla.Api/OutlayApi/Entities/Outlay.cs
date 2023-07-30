using System.ComponentModel.DataAnnotations.Schema;

namespace OutlayApi.Entities;

public class Outlay : EntityBase
{
    public string Name { get; set; }
    public int? Cost { get; set; }
    public DateTime? LastUpdateAt { get; set; }
    public int UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; }
    public int CategoryId { get; set; }
    [ForeignKey(nameof(CategoryId))]
    public virtual Category Category { get; set; }
}
