using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingSystem.DTOs.Shows;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Services.Interfaces;

namespace MovieTicketBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ShowsController : ControllerBase
    {
        private readonly IShowService _showService;

        public ShowsController(IShowService showService)
        {
            _showService = showService;
        }

        // Admin only
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddShow(CreateShowDto dto)
        {
            var show = new Show
            {
                MovieId = dto.MovieId,
                ScreenId = dto.ScreenId,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                BasePrice = dto.BasePrice,
                IsActive = dto.IsActive
            };

            var result = await _showService.AddAsync(show);

            return StatusCode(201, result);
        }

        // Customer + Admin
        [Authorize(Roles = "Customer,Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllShows()
        {
            var result = await _showService.GetAllAsync();

            return Ok(result);
        }

        // Customer + Admin
        [Authorize(Roles = "Customer,Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetShowById(int id)
        {
            var result = await _showService.GetByIdAsync(id);

            if (result == null)
                return NotFound("Show not found.");

            return Ok(result);
        }

        // Admin only
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShow(
            int id,
            UpdateShowDto dto)
        {
            var show = new Show
            {
                ShowId = id,
                MovieId = dto.MovieId,
                ScreenId = dto.ScreenId,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                BasePrice = dto.BasePrice,
                IsActive = dto.IsActive
            };

            var result = await _showService.UpdateAsync(show);

            if (result == null)
                return NotFound("Show not found.");

            return Ok(result);
        }

        // Admin only
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShow(int id)
        {
            var result = await _showService.DeleteAsync(id);

            if (!result)
                return NotFound("Show not found.");

            return NoContent();
        }

        // Customer + Admin
        [Authorize(Roles = "Customer,Admin")]
        [HttpGet("movie/{movieId}")]
        public async Task<IActionResult> GetShowsByMovieId(
            int movieId)
        {
            var result =
                await _showService.GetByMovieIdAsync(movieId);

            return Ok(result);
        }

        // Customer + Admin
        [Authorize(Roles = "Customer,Admin")]
        [HttpGet("screen/{screenId}")]
        public async Task<IActionResult> GetShowsByScreenId(
            int screenId)
        {
            var result =
                await _showService.GetByScreenIdAsync(screenId);

            return Ok(result);
        }

        // Customer + Admin
        [Authorize(Roles = "Customer,Admin")]
        [HttpGet("{showId}/seats")]
        public async Task<IActionResult> GetShowSeats(int showId)
        {
            var seats =
                await _showService.GetShowSeatsAsync(showId);

            if (seats.Count == 0)
                return NotFound("No seats found for this show.");

            return Ok(seats);
        }

        // Customer + Admin
        [Authorize(Roles = "Customer,Admin")]
        [HttpGet("{showId}/seats/available")]
        public async Task<IActionResult> GetAvailableShowSeats(
            int showId)
        {
            var seats =
                await _showService.GetAvailableShowSeatsAsync(showId);

            return Ok(seats);
        }

        // Customer + Admin
        [Authorize(Roles = "Customer,Admin")]
        [HttpGet("{showId}/seats/booked")]
        public async Task<IActionResult> GetBookedShowSeats(
            int showId)
        {
            var seats =
                await _showService.GetBookedShowSeatsAsync(showId);

            return Ok(seats);
        }
    }
}