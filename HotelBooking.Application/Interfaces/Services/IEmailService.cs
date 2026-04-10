using HotelBooking.Application.DTOs.Email;

namespace HotelBooking.Application.Interfaces.Services;

public interface IEmailService
{
    Task SendBookingConfirmationAsync(BookingConfirmationEmailDto dto, CancellationToken ct = default);
    Task SendCancellationEmailAsync(CancellationEmailDto dto, CancellationToken ct = default);
    Task SendWelcomeEmailAsync(string toEmail, string fullName, CancellationToken ct = default);
}