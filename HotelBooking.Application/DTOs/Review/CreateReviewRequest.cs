namespace HotelBooking.Application.DTOs.Review;

public class CreateReviewRequest
{
    public Guid HotelId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
}