namespace HotelBooking.Domain.Exceptions;

public class NotFoundException(string name, object key)
    : DomainException($"{name} with id '{key}' was not found.");