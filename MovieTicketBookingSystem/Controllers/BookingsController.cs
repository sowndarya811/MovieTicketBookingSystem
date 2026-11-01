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

        [HttpPost]
        public async Task<IActionResult> AddBooking(CreateBookingDto dto)
        {
            var booking = new Booking
            {
                ShowId = dto.ShowId,
                CustomerName = dto.CustomerName,
                CustomerPhone = dto.CustomerPhone,
                BookingDate = dto.BookingDate,
                TotalAmount = dto.TotalAmount,
                Status = dto.Status
            };

            var result = await _bookingService.AddAsync(booking);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            var result = await _bookingService.GetAllAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var result = await _bookingService.GetByIdAsync(id);

            if (result == null)
                return NotFound("Booking not found.");

            return Ok(result);
        }

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
                BookingDate = dto.BookingDate,
                TotalAmount = dto.TotalAmount,
                Status = dto.Status
            };

            var result = await _bookingService.UpdateAsync(booking);

            if (result == null)
                return NotFound("Booking not found.");

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var result = await _bookingService.DeleteAsync(id);

            if (!result)
                return NotFound("Booking not found.");

            return Ok("Booking deleted successfully.");
        }

        [HttpGet("show/{showId}")]
        public async Task<IActionResult> GetBookingsByShowId(int showId)
        {
            var result =
                await _bookingService.GetByShowIdAsync(showId);

            return Ok(result);
        }

        [HttpGet("{bookingId}/seats")]
        public async Task<IActionResult> GetBookingSeats(int bookingId)
        {
            var result =
                await _bookingService.GetBookingSeatsAsync(bookingId);

            return Ok(result);
        }

        [HttpPost("{bookingId}/seats")]
        public async Task<IActionResult> AddBookingSeat(
            int bookingId,
            CreateBookingSeatDto dto)
        {
            var bookingSeat = new BookingSeat
            {
                BookingId = bookingId,
                SeatId = dto.SeatId,
                Price = dto.Price
            };

            await _bookingService.AddBookingSeatAsync(bookingSeat);

            return Ok("Seat added to booking successfully.");
        }
    }
}