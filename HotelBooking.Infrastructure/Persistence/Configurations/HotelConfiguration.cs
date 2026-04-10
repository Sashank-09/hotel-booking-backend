using HotelBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelBooking.Infrastructure.Persistence.Configurations;

public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        builder.HasKey(h => h.Id);

        builder.Property(h => h.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(h => h.Description)
            .HasMaxLength(2000);

        builder.Property(h => h.Location)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(h => h.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(h => h.Country)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(h => h.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(h => h.Email)
            .HasMaxLength(200);

        builder.HasQueryFilter(h => !h.IsDeleted);

        builder.HasMany(h => h.Rooms)
            .WithOne(r => r.Hotel)
            .HasForeignKey(r => r.HotelId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(h => h.Reviews)
            .WithOne(r => r.Hotel)
            .HasForeignKey(r => r.HotelId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(h => h.Promotions)
            .WithOne(p => p.Hotel)
            .HasForeignKey(p => p.HotelId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}