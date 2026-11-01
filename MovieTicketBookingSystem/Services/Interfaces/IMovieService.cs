using MovieTicketBookingSystem.Models;

namespace MovieTicketBookingSystem.Services.Interfaces
{
    public interface IMovieService
    {
        Task<Movie> AddMovieAsync(Movie movie);

        Task<List<Movie>> GetAllMoviesAsync(
            int pageNumber,
            int pageSize,
            string? title = null);

        Task<Movie?> GetMovieByIdAsync(int id);

        Task<Movie?> UpdateMovieAsync(
            int id,
            Movie movie);

        Task<bool> DeleteMovieAsync(int id);

        Task<List<Movie>> SearchMoviesByTitleAsync(
            string title);
    }
}