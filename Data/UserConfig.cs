using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using VitrineSemiJoias.Models;
using VitrineSemiJoias.Enums;

namespace VitrineSemiJoias.Data;

public class UserConfig : IEntityTypeConfiguration<UserModel>
{
    public void Configure(EntityTypeBuilder<UserModel> builder)
    {      
        builder.Property(u => u.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(u => u.Profile)
               .HasConversion<string>()
               .HasMaxLength(50)
               .HasDefaultValue(ProfileEnum.Client);
    }
}
