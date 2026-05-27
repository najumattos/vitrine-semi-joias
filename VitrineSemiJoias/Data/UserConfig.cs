using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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

        var passwordHasher = new PasswordHasher<UserModel>();

        var adminUser = new UserModel
        {
            Id = 1,
            Name = "Camila Reis",
            UserName = "camila@admin.com", 
            Email = "camila@admin.com",
            NormalizedUserName = "CAMILA@ADMIN.COM",
            NormalizedEmail = "CAMILA@ADMIN.COM",   
            EmailConfirmed = true,
            Profile = ProfileEnum.Admin, // 🌟 Garante que o usuário inicial nasça como Administrador
            SecurityStamp = "937c4a6c-0d92-4783-927c-43320f57172f" 
        };
        adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "123456");
        builder.HasData(adminUser);
    }
}