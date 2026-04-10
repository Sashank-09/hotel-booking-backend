using HotelBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelBooking.Infrastructure.Persistence.Configurations;

public class HotelOwnerConfiguration : IEntityTypeConfiguration<HotelOwnerProfile>
{
    public void Configure(EntityTypeBuilder<HotelOwnerProfile> builder)
    {
        builder.HasKey(h => h.Id);

        builder.Property(h => h.OwnerId)
            .IsRequired()
            .HasMaxLength(450);

        builder.Property(h => h.BusinessName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(h => h.BusinessEmail)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(h => h.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(h => h.TaxId)
            .HasMaxLength(50);

        builder.HasQueryFilter(h => !h.IsDeleted);
    }
}