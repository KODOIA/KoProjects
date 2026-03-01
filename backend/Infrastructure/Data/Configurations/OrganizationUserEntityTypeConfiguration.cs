using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain;

namespace Infrastructure.Data.Configurations;

internal class OrganizationUserEntityTypeConfiguration : IEntityTypeConfiguration<OrganizationUser>
{
    public void Configure(EntityTypeBuilder<OrganizationUser> builder)
    {
        builder.ToTable("organization_users", "public");

        builder.HasKey(ou => ou.Id);

        builder.Property(ou => ou.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(ou => ou.OrganizationId)
            .HasColumnName("organization_id")
            .IsRequired();

        builder.Property(ou => ou.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(ou => ou.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(ou => ou.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(ou => ou.Organization)
            .WithMany(o => o.OrganizationUsers)
            .HasForeignKey(ou => ou.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ou => ou.User)
            .WithMany()
            .HasForeignKey(ou => ou.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
