using Microsoft.AspNetCore.Authorization;
using MovieTicketBookingSystem.Models;
using System.Security.Claims;

namespace MovieTicketBookingSystem.Authorization
{
    public class BookingAuthorizationHandler
        : AuthorizationHandler<BookingAuthorizationRequirement, Booking>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            BookingAuthorizationRequirement requirement,
            Booking booking)
        {
            var userIdValue = context.User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (context.User.IsInRole("Admin"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            if (int.TryParse(userIdValue, out int userId))
            {
                if (booking.UserId == userId)
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}