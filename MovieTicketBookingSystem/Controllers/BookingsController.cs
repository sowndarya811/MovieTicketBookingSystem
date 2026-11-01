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

        // POST: api/Bookings
        [HttpPost]
        public async Task<IActionResult> CreateBooking(CreateBookingDto dto)
        {
            try
            {
                var booking = await _bookingService.CreateBookingAsync(dto);

                return CreatedAtAction(
                    nameof(GetBookingById),
                    new { id = booking.BookingId },
                    booking);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        // GET: api/Bookings
        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _bookingService.GetAllAsync();

            return Ok(bookings);
        }

        // GET: api/Bookings/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var booking = await _bookingService.GetByIdAsync(id);

            if (booking == null)
                return NotFound("Booking not found.");

            return Ok(booking);
        }

        // PUT: api/Bookings/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(
            int id,
            UpdateBookingDto dto)
        {
            var existingBooking = await _bookingService.GetByIdAsync(id);

            if (existingBooking == null)
                return NotFound("Booking not found.");

            existingBooking.ShowId = dto.ShowId;
            existingBooking.CustomerName = dto.CustomerName;
            existingBooking.CustomerPhone = dto.CustomerPhone;
            existingBooking.Status = dto.Status;

            var result = await _bookingService.UpdateAsync(existingBooking);

            return Ok(result);
        }

        // PUT: api/Bookings/{id}/cancel
        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            try
            {
                var result = await _bookingService.CancelBookingAsync(id);

                if (!result)
                    return NotFound("Booking not found.");

                return Ok(new
                {
                    message = "Booking cancelled successfully."
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        // DELETE: api/Bookings/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var result = await _bookingService.DeleteAsync(id);

            if (!result)
                return NotFound("Booking not found.");

            return NoContent();
        }

        // GET: api/Bookings/show/{showId}
        [HttpGet("show/{showId}")]
        public async Task<IActionResult> GetBookingsByShow(int showId)
        {
            var bookings = await _bookingService.GetByShowIdAsync(showId);

            return Ok(bookings);
        }

        // GET: api/Bookings/{bookingId}/seats
        [HttpGet("{bookingId}/seats")]
        public async Task<IActionResult> GetBookingSeats(int bookingId)
        {
            var booking = await _bookingService.GetByIdAsync(bookingId);

            if (booking == null)
                return NotFound("Booking not found.");

            var seats = await _bookingService.GetBookingSeatsAsync(bookingId);

            return Ok(seats);
        }

        // POST: api/Bookings/{bookingId}/seats
        [HttpPost("{bookingId}/seats")]
        public async Task<IActionResult> AddBookingSeat(
            int bookingId,
            CreateBookingSeatDto dto)
        {
            var booking = await _bookingService.GetByIdAsync(bookingId);

            if (booking == null)
                return NotFound("Booking not found.");

            var bookingSeat = new BookingSeat
            {
                BookingId = bookingId,
                SeatId = dto.SeatId,
                Price = dto.Price
            };

            await _bookingService.AddBookingSeatAsync(bookingSeat);

            return Ok(bookingSeat);
        }
    }
}