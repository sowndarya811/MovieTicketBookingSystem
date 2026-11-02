using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingSystem.DTOs.Bookings;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Services.Interfaces;

namespace MovieTicketBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // Customer + Admin
        [Authorize(Roles = "Customer,Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateBooking(
            CreateBookingDto dto)
        {
            try
            {
                var booking =
                    await _bookingService.CreateBookingAsync(dto);

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

        // Admin only
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings =
                await _bookingService.GetAllAsync();

            return Ok(bookings);
        }

        // Customer + Admin
        [Authorize(Roles = "Customer,Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var booking =
                await _bookingService.GetByIdAsync(id);

            if (booking == null)
                return NotFound("Booking not found.");

            return Ok(booking);
        }

        // Admin only
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

        // Admin only
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

        // Admin only
        [Authorize(Roles = "Admin")]
        [HttpGet("show/{showId}")]
        public async Task<IActionResult> GetBookingsByShowId(
            int showId)
        {
            var bookings =
                await _bookingService.GetByShowIdAsync(showId);

            return Ok(bookings);
        }

        // Customer + Admin
        [Authorize(Roles = "Customer,Admin")]
        [HttpGet("{bookingId}/seats")]
        public async Task<IActionResult> GetBookingSeats(
            int bookingId)
        {
            var booking =
                await _bookingService.GetByIdAsync(bookingId);

            if (booking == null)
                return NotFound("Booking not found.");

            var seats =
                await _bookingService.GetBookingSeatsAsync(
                    bookingId);

            return Ok(seats);
        }

        // Customer + Admin
        [Authorize(Roles = "Customer,Admin")]
        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            try
            {
                var booking =
                    await _bookingService.CancelBookingAsync(id);

                if (booking == null)
                    return NotFound("Booking not found.");

                return Ok(booking);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }
    }
}