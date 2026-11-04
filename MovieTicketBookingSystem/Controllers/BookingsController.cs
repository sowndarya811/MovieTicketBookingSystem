
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingSystem.DTOs.Bookings;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Services.Interfaces;
using MovieTicketBookingSystem.Authorization;

namespace MovieTicketBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IAuthorizationService _authorizationService;

        public BookingsController(
            IBookingService bookingService,
            IAuthorizationService authorizationService)
        {
            _bookingService = bookingService;
            _authorizationService = authorizationService;
        }

        // Customer + Admin: Create Booking
        [Authorize(Roles = "Customer,Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateBooking(
            CreateBookingDto dto)
        {
            try
            {
                var userIdValue = User.FindFirst(
                    System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdValue, out int userId))
                    return Unauthorized();

                var booking =
                    await _bookingService.CreateBookingAsync(
                        dto, userId);

                if (booking == null)
                    return BadRequest("Booking could not be created.");

                return CreatedAtAction(
                    nameof(GetBookingById),
                    new { id = booking.BookingId },
                    booking);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        // Admin only: Get All Bookings
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings =
                await _bookingService.GetAllAsync();

            return Ok(bookings);
        }

        // Customer only: Get Own Bookings
        [Authorize(Roles = "Customer")]
        [HttpGet("my-bookings")]
        public async Task<IActionResult> GetMyBookings()
        {
            var userIdValue = User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdValue, out int userId))
                return Unauthorized();

            var bookings =
                await _bookingService.GetByUserIdAsync(userId);

            return Ok(bookings);
        }

        // Customer + Admin: Get Booking By ID
        [Authorize(Roles = "Customer,Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var booking =
                await _bookingService.GetByIdAsync(id);

            if (booking == null)
                return NotFound("Booking not found.");

            var authorizationResult =
                await _authorizationService.AuthorizeAsync(
                    User,
                    booking,
                    new BookingAuthorizationRequirement());

            if (!authorizationResult.Succeeded)
                return Forbid();

            return Ok(booking);
        }

        // Admin only: Update Booking
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(
            int id,
            UpdateBookingDto dto)
        {
            var booking = new Booking
            {
                BookingId = id,
                ShowId = dto.ShowId,
                CustomerName = dto.CustomerName,
                CustomerPhone = dto.CustomerPhone,
                Status = dto.Status
            };

            var result =
                await _bookingService.UpdateAsync(booking);

            if (result == null)
                return NotFound("Booking not found.");

            return Ok(result);
        }

        // Admin only: Delete Booking
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var result =
                await _bookingService.DeleteAsync(id);

            if (!result)
                return NotFound("Booking not found.");

            return NoContent();
        }

        // Admin only: Get Bookings By Show ID
        [Authorize(Roles = "Admin")]
        [HttpGet("show/{showId}")]
        public async Task<IActionResult> GetBookingsByShowId(
            int showId)
        {
            var bookings =
                await _bookingService.GetByShowIdAsync(showId);

            return Ok(bookings);
        }

        // Customer + Admin: Get Booking Seats
        [Authorize(Roles = "Customer,Admin")]
        [HttpGet("{bookingId}/seats")]
        public async Task<IActionResult> GetBookingSeats(
            int bookingId)
        {
            var booking =
                await _bookingService.GetByIdAsync(bookingId);

            if (booking == null)
                return NotFound("Booking not found.");

            var authorizationResult =
                await _authorizationService.AuthorizeAsync(
                    User,
                    booking,
                    new BookingAuthorizationRequirement());

            if (!authorizationResult.Succeeded)
                return Forbid();

            var seats =
                await _bookingService.GetBookingSeatsAsync(
                    bookingId);

            return Ok(seats);
        }

        // Customer + Admin: Cancel Booking
        [Authorize(Roles = "Customer,Admin")]
        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            try
            {
                var booking =
                    await _bookingService.GetByIdAsync(id);

                if (booking == null)
                    return NotFound("Booking not found.");

                var authorizationResult =
                    await _authorizationService.AuthorizeAsync(
                        User,
                        booking,
                        new BookingAuthorizationRequirement());

                if (!authorizationResult.Succeeded)
                    return Forbid();

                var result =
                    await _bookingService.CancelBookingAsync(id);

                if (result == null)
                    return NotFound("Booking not found.");

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }
    }
}
