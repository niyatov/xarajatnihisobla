using System.ComponentModel.DataAnnotations.Schema;

namespace OutlayApi.Entities;

public class UserCategory
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    [ForeignKey(nameof(CategoryId))]
    public virtual Category Category { get; set; }
    public int UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; }
    public bool IsAdmin { get; set; }
}
