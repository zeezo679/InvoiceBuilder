using Domain.Entities.Auth;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations.Auth;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(refreshToken => refreshToken.Id);
    
        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(refreshToken => refreshToken.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
    
        builder.Property(refreshToken => refreshToken.UserId).IsRequired();
        builder.Property(refreshToken => refreshToken.Token).IsRequired();
        builder.Property(refreshToken => refreshToken.Expires).IsRequired();
        builder.Property(refreshToken => refreshToken.IsRevoked).IsRequired();


        builder.HasIndex(refreshToken => refreshToken.Token).IsUnique();
        builder.HasIndex(refreshToken => refreshToken.UserId);
    }
}