using MovieTicketBookingSystem.Models;

namespace MovieTicketBookingSystem.Repositories.Interfaces
{
    public interface IShowRepository
    {
        Task<Show> AddAsync(Show show);

        Task<List<Show>> GetAllAsync();

        Task<Show?> GetByIdAsync(int id);

        Task<Show?> UpdateAsync(Show show);

        Task<bool> DeleteAsync(int id);

        Task<List<Show>> GetByMovieIdAsync(int movieId);

        Task<List<Show>> GetByScreenIdAsync(int screenId);
        Task<List<ShowSeat>> GetShowSeatsAsync(int showId);
        Task<List<ShowSeat>> GetAvailableShowSeatsAsync(int showId);
        Task<List<ShowSeat>> GetBookedShowSeatsAsync(int showId);
    }
}