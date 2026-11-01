using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingSystem.DTOs.Seats;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Services.Interfaces;

namespace MovieTicketBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeatsController : ControllerBase
    {
        private readonly ISeatService _seatService;

        public SeatsController(ISeatService seatService)
        {
            _seatService = seatService;
        }

        [HttpPost]
        public async Task<IActionResult> AddSeat(CreateSeatDto dto)
        {
            var seat = new Seat
            {
                ScreenId = dto.ScreenId,
                SeatNumber = dto.SeatNumber,
                SeatType = dto.SeatType,
                IsAvailable = dto.IsAvailable
            };

            var result = await _seatService.AddAsync(seat);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSeats()
        {
            var result = await _seatService.GetAllAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSeatById(int id)
        {
            var result = await _seatService.GetByIdAsync(id);

            if (result == null)
                return NotFound("Seat not found.");

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSeat(
            int id,
            UpdateSeatDto dto)
        {
            var seat = new Seat
            {
                SeatId = id,
                ScreenId = dto.ScreenId,
                SeatNumber = dto.SeatNumber,
                SeatType = dto.SeatType,
                IsAvailable = dto.IsAvailable
            };

            var result = await _seatService.UpdateAsync(seat);

            if (result == null)
                return NotFound("Seat not found.");

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeat(int id)
        {
            var result = await _seatService.DeleteAsync(id);

            if (!result)
                return NotFound("Seat not found.");

            return Ok("Seat deleted successfully.");
        }

        [HttpGet("screen/{screenId}")]
        public async Task<IActionResult> GetSeatsByScreenId(int screenId)
        {
            var result = await _seatService.GetByScreenIdAsync(screenId);

            return Ok(result);
        }
    }
}