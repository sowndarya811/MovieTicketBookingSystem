using MovieTicketBookingSystem.Models;

namespace MovieTicketBookingSystem.Services.Interfaces
{
    public interface IShowService
    {
        Task<Show> AddAsync(Show show);

        Task<List<Show>> GetAllAsync();

        Task<Show?> GetByIdAsync(int id);

        Task<Show?> UpdateAsync(Show show);

        Task<bool> DeleteAsync(int id);

        Task<List<Show>> GetByMovieIdAsync(int movieId);

        Task<List<Show>> GetByScreenIdAsync(int screenId);
    }
}