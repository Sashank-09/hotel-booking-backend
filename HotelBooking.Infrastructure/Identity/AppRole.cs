using Microsoft.AspNetCore.Identity;

namespace HotelBooking.Infrastructure.Identity;

public class AppRole : IdentityRole<Guid>
{
    public AppRole() { }
    public AppRole(string roleName) : base(roleName) { }
}