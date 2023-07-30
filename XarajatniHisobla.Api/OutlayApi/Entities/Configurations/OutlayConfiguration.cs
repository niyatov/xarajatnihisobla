using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OutlayApi.Entities.Configurations;

public class OutlayConfiguration : EntityBaseConfiguration<Outlay>
{
    public override void Configure(EntityTypeBuilder<Outlay> builder)
    {
        base.Configure(builder);
        builder.Property(b => b.Name).HasColumnType("varchar(20)").IsRequired();
        builder.HasOne(c => c.User)
        .WithMany(e => e.Outlays).IsRequired();
        builder.HasOne(c => c.Category)
        .WithMany(e => e.Outlays).IsRequired();
    }
}