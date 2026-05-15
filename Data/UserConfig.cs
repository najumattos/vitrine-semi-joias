using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using VitrineSemiJoias.Models;
using VitrineSemiJoias.Enums;

namespace VitrineSemiJoias.Data;

public class UserConfig : IEntityTypeConfiguration<UserModel>
{
    public void Configure(EntityTypeBuilder<UserModel> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(u => u.Email)              
               .IsRequired()
               .HasMaxLength(100);

        builder.HasIndex(u => u.Email)                                                   
                    .IsUnique();

        builder.Property(u => u.PasswordHash)
               .IsRequired()
               .HasMaxLength(255);

        builder.Property(u => u.Profile)
               .HasConversion<string>()
               .HasMaxLength(50)
               .HasDefaultValue(ProfileEnum.Client);
    }
}
