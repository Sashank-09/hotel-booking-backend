using HotelBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelBooking.Infrastructure.Persistence.Configurations;

public class LoyaltyTransactionConfiguration : IEntityTypeConfiguration<LoyaltyTransaction>
{
    public void Configure(EntityTypeBuilder<LoyaltyTransaction> builder)
    {
        builder.HasKey(l => l.Id);

        builder.Property(l => l.UserId)
            .IsRequired()
            .HasMaxLength(450);

        builder.Property(l => l.Description)
            .HasMaxLength(500);

        builder.HasOne(l => l.Booking)
            .WithMany()
            .HasForeignKey(l => l.BookingId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}