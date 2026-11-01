using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingSystem.DTOs.Bookings;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Services.Interfaces;

namespace MovieTicketBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // CREATE BOOKING
        [HttpPost]
        public async Task<IActionResult> CreateBooking(
            CreateBookingDto dto)
        {
            try
            {
                var booking =
                    await _bookingService.CreateBookingAsync(dto);

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

        // GET ALL BOOKINGS
        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings =
                await _bookingService.GetAllAsync();

            return Ok(bookings);
        }

        // GET BOOKING BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var booking =
                await _bookingService.GetByIdAsync(id);

            if (booking == null)
                return NotFound("Booking not found.");

            return Ok(booking);
        }

        // UPDATE BOOKING
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

        // DELETE BOOKING
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var result =
                await _bookingService.DeleteAsync(id);

            if (!result)
                return NotFound("Booking not found.");

            return NoContent();
        }

        // GET BOOKINGS BY SHOW
        [HttpGet("show/{showId}")]
        public async Task<IActionResult> GetBookingsByShowId(
            int showId)
        {
            var bookings =
                await _bookingService.GetByShowIdAsync(showId);

            return Ok(bookings);
        }

        // GET BOOKING SEATS
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

        // CANCEL BOOKING
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