using MovieTicketBookingSystem.Models;

namespace MovieTicketBookingSystem.Repositories.Interfaces
{
    public interface IMovieRepository
    {
        Task<Movie> AddAsync(Movie movie);

        Task<List<Movie>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string? title = null);

        Task<Movie?> GetByIdAsync(int id);

        Task<Movie?> UpdateAsync(Movie movie);

        Task<bool> DeleteAsync(int id);

        Task<List<Movie>> SearchByTitleAsync(string title);
    }
}