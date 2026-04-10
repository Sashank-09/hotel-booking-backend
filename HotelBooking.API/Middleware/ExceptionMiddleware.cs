using System.Net;
using System.Text.Json;
using HotelBooking.Application.DTOs.Common;
using HotelBooking.Domain.Exceptions;

namespace HotelBooking.API.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var (statusCode, message) = ex switch
        {
            NotFoundException e => (HttpStatusCode.NotFound, e.Message),
            ConflictException e => (HttpStatusCode.Conflict, e.Message),
            ForbiddenException e => (HttpStatusCode.Forbidden, e.Message),
            ValidationException e => (HttpStatusCode.BadRequest, e.Message),
            DomainException e => (HttpStatusCode.BadRequest, e.Message),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.")
        };

        var response = ApiResponse<object>.Fail(message);
        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        return context.Response.WriteAsync(json);
    }
}