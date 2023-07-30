using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OutlayApi.Entities.Configurations;

public class DebtConfiguration : EntityBaseConfiguration<Debt>
{
    public override void Configure(EntityTypeBuilder<Debt> builder)
    {
        base.Configure(builder);
        builder.Property(b => b.Cost).HasColumnType("integer").HasColumnType("varchar(8)").IsRequired();
        builder.Property(b => b.SenderId).HasColumnType("integer").IsRequired();
        builder.Property(b => b.ReceiverId).HasColumnType("integer").IsRequired();
        builder.Property(b => b.GivenPersonId).HasColumnType("integer").IsRequired();
    }
}