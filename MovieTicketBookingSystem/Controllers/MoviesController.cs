using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingSystem.DTOs.Movies;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Services.Interfaces;

namespace MovieTicketBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpPost]
        public async Task<IActionResult> AddMovie(CreateMovieDto dto)
        {
            var movie = new Movie
            {
                Title = dto.Title,
                Description = dto.Description,
                DurationMinutes = dto.DurationMinutes,
                Language = dto.Language,
                Genre = dto.Genre,
                ReleaseDate = dto.ReleaseDate
            };

            var result = await _movieService.AddMovieAsync(movie);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMovies(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? title = null)
        {
            var result = await _movieService.GetAllMoviesAsync(
                pageNumber,
                pageSize,
                title);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieById(int id)
        {
            var result = await _movieService.GetMovieByIdAsync(id);

            if (result == null)
                return NotFound("Movie not found.");

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(
            int id,
            UpdateMovieDto dto)
        {
            var movie = new Movie
            {
                MovieId = id,
                Title = dto.Title,
                Description = dto.Description,
                DurationMinutes = dto.DurationMinutes,
                Language = dto.Language,
                Genre = dto.Genre,
                ReleaseDate = dto.ReleaseDate
            };

            var result = await _movieService.UpdateMovieAsync(id, movie);

            if (result == null)
                return NotFound("Movie not found.");

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var result = await _movieService.DeleteMovieAsync(id);

            if (!result)
                return NotFound("Movie not found.");

            return Ok("Movie deleted successfully.");
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchMoviesByTitle(
            [FromQuery] string title)
        {
            var result = await _movieService.SearchMoviesByTitleAsync(title);

            return Ok(result);
        }
    }
}