using HotelBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelBooking.Infrastructure.Persistence.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.TotalPrice)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(b => b.ConfirmationNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(b => b.ConfirmationNumber)
            .IsUnique();

        builder.Property(b => b.SpecialRequests)
            .HasMaxLength(1000);

        builder.HasQueryFilter(b => !b.IsDeleted);
    }
}