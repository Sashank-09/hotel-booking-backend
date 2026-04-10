using HotelBooking.Application.DTOs.Email;
using HotelBooking.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace HotelBooking.Infrastructure.Services;

public class EmailService(ILogger<EmailService> logger) : IEmailService
{
    public Task SendBookingConfirmationAsync(BookingConfirmationEmailDto dto, CancellationToken ct = default)
    {
        // Replace with real SendGrid/SMTP implementation for production
        logger.LogInformation(
            "Sending booking confirmation to {Email} for booking {ConfirmationNumber}",
            dto.ToEmail, dto.ConfirmationNumber);
        return Task.CompletedTask;
    }

    public Task SendCancellationEmailAsync(CancellationEmailDto dto, CancellationToken ct = default)
    {
        logger.LogInformation(
            "Sending cancellation email to {Email} for booking {ConfirmationNumber}",
            dto.ToEmail, dto.ConfirmationNumber);
        return Task.CompletedTask;
    }

    public Task SendWelcomeEmailAsync(string toEmail, string fullName, CancellationToken ct = default)
    {
        logger.LogInformation("Sending welcome email to {Email}", toEmail);
        return Task.CompletedTask;
    }
}