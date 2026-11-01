using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingSystem.DTOs.Shows;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Services.Interfaces;

namespace MovieTicketBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShowsController : ControllerBase
    {
        private readonly IShowService _showService;

        public ShowsController(IShowService showService)
        {
            _showService = showService;
        }

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

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllShows()
        {
            var result = await _showService.GetAllAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetShowById(int id)
        {
            var result = await _showService.GetByIdAsync(id);

            if (result == null)
                return NotFound("Show not found.");

            return Ok(result);
        }

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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShow(int id)
        {
            var result = await _showService.DeleteAsync(id);

            if (!result)
                return NotFound("Show not found.");

            return Ok("Show deleted successfully.");
        }

        [HttpGet("movie/{movieId}")]
        public async Task<IActionResult> GetShowsByMovieId(int movieId)
        {
            var result = await _showService.GetByMovieIdAsync(movieId);

            return Ok(result);
        }

        [HttpGet("screen/{screenId}")]
        public async Task<IActionResult> GetShowsByScreenId(int screenId)
        {
            var result = await _showService.GetByScreenIdAsync(screenId);

            return Ok(result);
        }
        [HttpGet("{showId}/seats")]
        public async Task<IActionResult> GetShowSeats(int showId)
        {
            var seats = await _showService.GetShowSeatsAsync(showId);

            if (seats.Count == 0)
                return NotFound("No seats found for this show.");

            return Ok(seats);
        }

        [HttpGet("{showId}/seats/available")]
        public async Task<IActionResult> GetAvailableShowSeats(int showId)
        {
            var seats = await _showService.GetAvailableShowSeatsAsync(showId);

            return Ok(seats);
        }

        [HttpGet("{showId}/seats/booked")]
        public async Task<IActionResult> GetBookedShowSeats(int showId)
        {
            var seats = await _showService.GetBookedShowSeatsAsync(showId);

            return Ok(seats);
        }
    }
}