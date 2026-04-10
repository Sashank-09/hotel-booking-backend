using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Enums;
using HotelBooking.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Persistence.Seeders;

public class DataSeeder(
    AppDbContext context,
    UserManager<AppUser> userManager,
    RoleManager<AppRole> roleManager)
{
    public async Task SeedAsync()
    {
        await SeedRolesAsync();
        await SeedAdminUserAsync();
        await SeedHotelsAsync();
    }

    private async Task SeedRolesAsync()
    {
        string[] roles = ["Admin", "HotelManager", "Guest"];
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new AppRole { Name = role });
        }
    }

    private async Task SeedAdminUserAsync()
    {
        const string adminEmail = "admin@hotelbooking.com";
        if (await userManager.FindByEmailAsync(adminEmail) != null) return;

        var admin = new AppUser
        {
            FirstName = "System",
            LastName = "Admin",
            Email = adminEmail,
            UserName = adminEmail,
            EmailConfirmed = true
        };

        await userManager.CreateAsync(admin, "Admin@12345");
        await userManager.AddToRoleAsync(admin, "Admin");
    }

    private async Task SeedHotelsAsync()
    {
        if (await context.Hotels.AnyAsync()) return;

        var hotels = new List<Hotel>
        {
            new()
            {
                Name = "The Grand Palace",
                Description = "A luxury 5-star hotel in the heart of the city.",
                Location = "123 Main Street",
                City = "New York",
                Country = "USA",
                StarRating = 5,
                PhoneNumber = "+1-212-555-0100",
                Email = "info@grandpalace.com",
                IsActive = true,
                Rooms = new List<Room>
                {
                    new() { RoomNumber = "101", Type = RoomType.Single,    PricePerNight = 150, MaxOccupancy = 1, Description = "Cozy single room", IsAvailable = true },
                    new() { RoomNumber = "201", Type = RoomType.Double,    PricePerNight = 250, MaxOccupancy = 2, Description = "Spacious double room", IsAvailable = true },
                    new() { RoomNumber = "301", Type = RoomType.Suite,     PricePerNight = 500, MaxOccupancy = 4, Description = "Luxury suite", IsAvailable = true },
                    new() { RoomNumber = "401", Type = RoomType.Deluxe,    PricePerNight = 750, MaxOccupancy = 4, Description = "Deluxe room with city view", IsAvailable = true },
                }
            },
            new()
            {
                Name = "Seaside Resort",
                Description = "Beautiful beachfront resort with stunning ocean views.",
                Location = "456 Ocean Drive",
                City = "Miami",
                Country = "USA",
                StarRating = 4,
                PhoneNumber = "+1-305-555-0200",
                Email = "info@seasideresort.com",
                IsActive = true,
                Rooms = new List<Room>
                {
                    new() { RoomNumber = "101", Type = RoomType.Double,    PricePerNight = 200, MaxOccupancy = 2, Description = "Ocean view double room", IsAvailable = true },
                    new() { RoomNumber = "201", Type = RoomType.Suite,     PricePerNight = 450, MaxOccupancy = 3, Description = "Beachfront suite", IsAvailable = true },
                }
            }
        };

        await context.Hotels.AddRangeAsync(hotels);
        await context.SaveChangesAsync();
    }
}