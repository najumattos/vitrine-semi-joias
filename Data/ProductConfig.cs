using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VitrineSemiJoias.Models;

namespace VitrineSemiJoias.Data;

public class ProductConfig : IEntityTypeConfiguration<ProductModel>
{
    public void Configure(EntityTypeBuilder<ProductModel> builder)
    {
        builder.ToTable("Products");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Title)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(p => p.Description)
               .HasMaxLength(500);

        builder.Property(p => p.Price)
               .IsRequired()
               .HasColumnType("decimal(18,2)");


        builder.Property(p => p.ImageUrl)
               .IsRequired()
               .HasMaxLength(255);

        builder.Property(p => p.StockQuantity)
               .HasDefaultValue(1);
        
        builder.Property(p => p.CategoryEnum)
               .IsRequired()
               .HasConversion<string>()
               .HasMaxLength(50);

        builder.Property(p => p.IsAvailable)            
               .HasConversion<bool>()
               .HasDefaultValue(true);
    }
}
