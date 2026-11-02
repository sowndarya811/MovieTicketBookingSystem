using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Services.Interfaces;

namespace MovieTicketBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SeatsController : ControllerBase
    {
        private readonly ISeatService _seatService;

        public SeatsController(ISeatService seatService)
        {
            _seatService = seatService;
        }

        // Admin only
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddSeat(Seat seat)
        {
            var result = await _seatService.AddAsync(seat);

            return Ok(result);
        }

        // Customer + Admin
        [Authorize(Roles = "Customer,Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllSeats()
        {
            var result = await _seatService.GetAllAsync();

            return Ok(result);
        }

        // Customer + Admin
        [Authorize(Roles = "Customer,Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSeatById(int id)
        {
            var result = await _seatService.GetByIdAsync(id);

            if (result == null)
                return NotFound("Seat not found.");

            return Ok(result);
        }

        // Customer + Admin
        [Authorize(Roles = "Customer,Admin")]
        [HttpGet("screen/{screenId}")]
        public async Task<IActionResult> GetSeatsByScreenId(
            int screenId)
        {
            var result =
                await _seatService.GetByScreenIdAsync(screenId);

            return Ok(result);
        }

        // Admin only
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSeat(
            int id,
            Seat seat)
        {
            seat.SeatId = id;

            var result =
                await _seatService.UpdateAsync(seat);

            if (result == null)
                return NotFound("Seat not found.");

            return Ok(result);
        }

        // Admin only
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeat(int id)
        {
            var result =
                await _seatService.DeleteAsync(id);

            if (!result)
                return NotFound("Seat not found.");

            return Ok("Seat deleted successfully.");
        }
    }
}