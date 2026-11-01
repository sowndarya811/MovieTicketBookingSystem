using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Repositories.Interfaces;
using MovieTicketBookingSystem.Services.Interfaces;

namespace MovieTicketBookingSystem.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;

        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<Movie> AddMovieAsync(Movie movie)
        {
            return await _movieRepository.AddAsync(movie);
        }

        public async Task<List<Movie>> GetAllMoviesAsync(
            int pageNumber,
            int pageSize,
            string? title = null)
        {
            return await _movieRepository.GetAllAsync(
                pageNumber,
                pageSize,
                title);
        }

        public async Task<Movie?> GetMovieByIdAsync(int id)
        {
            return await _movieRepository.GetByIdAsync(id);
        }

        public async Task<Movie?> UpdateMovieAsync(
            int id,
            Movie movie)
        {
            movie.MovieId = id;

            return await _movieRepository.UpdateAsync(movie);
        }

        public async Task<bool> DeleteMovieAsync(int id)
        {
            return await _movieRepository.DeleteAsync(id);
        }

        public async Task<List<Movie>> SearchMoviesByTitleAsync(
            string title)
        {
            return await _movieRepository.SearchByTitleAsync(title);
        }
    }
}