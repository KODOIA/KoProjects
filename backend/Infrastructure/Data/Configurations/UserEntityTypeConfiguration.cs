using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain;


namespace Infrastructure.Data.Configurations;

internal class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users", "public");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(u => u.GivenName)
            .HasColumnName("given_name")
            .HasMaxLength(64);

        builder.Property(u => u.FamilyName)
            .HasColumnName("family_name")
            .HasMaxLength(64);

        builder.Property(u => u.Email)
            .HasColumnName("email")
            .HasMaxLength(64);

        builder.Property(u => u.PreferredUsername)
            .HasColumnName("preferred_username")
            .HasMaxLength(64);

        builder.Property(u => u.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(u => u.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}