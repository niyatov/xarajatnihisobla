using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OutlayApi.Entities.Configurations;

public class CategoryConfiguration : EntityBaseConfiguration<Category>
{
    public override void Configure(EntityTypeBuilder<Category> builder)
    {
        base.Configure(builder);
        builder.Property(b => b.Name).IsUnicode().HasColumnType("varchar(20)").IsRequired();
        builder.Property(b => b.Key).HasColumnType("varchar(30)");
        builder.HasMany(c => c.UserCategories)
        .WithOne(e => e.Category).IsRequired();
    }
}