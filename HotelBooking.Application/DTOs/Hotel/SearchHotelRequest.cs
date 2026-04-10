namespace HotelBooking.Application.DTOs.Hotel;

public class SearchHotelRequest
{
    public string? Location { get; set; }
    public string? City { get; set; }
    public DateTime? CheckInDate { get; set; }
    public DateTime? CheckOutDate { get; set; }
    public int? Guests { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int? StarRating { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}