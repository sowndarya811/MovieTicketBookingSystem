
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Services.Interfaces;

namespace MovieTicketBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        // Customer + Admin
        [Authorize(Roles = "Customer,Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllMovies(
            int pageNumber = 1,
            int pageSize = 10,
            string? title = null)
        {
            var movies = await _movieService.GetAllMoviesAsync(
                pageNumber,
                pageSize,
                title);

            return Ok(movies);
        }

        // Customer + Admin
        [Authorize(Roles = "Customer,Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieById(int id)
        {
            var movie = await _movieService.GetMovieByIdAsync(id);

            if (movie == null)
            {
                return NotFound("Movie not found.");
            }

            return Ok(movie);
        }

        // Customer + Admin
        [Authorize(Roles = "Customer,Admin")]
        [HttpGet("search")]
        public async Task<IActionResult> SearchMoviesByTitle(
            string title)
        {
            var movies =
                await _movieService.SearchMoviesByTitleAsync(title);

            return Ok(movies);
        }

        // Admin only
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddMovie(Movie movie)
        {
            var result = await _movieService.AddMovieAsync(movie);

            return Ok(result);
        }

        // Admin only
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(
            int id,
            Movie movie)
        {
            var existingMovie =
                await _movieService.GetMovieByIdAsync(id);

            if (existingMovie == null)
            {
                return NotFound("Movie not found.");
            }

            var result =
                await _movieService.UpdateMovieAsync(id, movie);

            return Ok(result);
        }

        // Admin only
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var result =
                await _movieService.DeleteMovieAsync(id);

            if (!result)
            {
                return NotFound("Movie not found.");
            }

            return Ok("Movie deleted successfully.");
        }
    }
}
