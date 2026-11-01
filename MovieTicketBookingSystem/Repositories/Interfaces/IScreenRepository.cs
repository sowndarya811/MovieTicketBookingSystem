using MovieTicketBookingSystem.Models;

namespace MovieTicketBookingSystem.Repositories.Interfaces
{
    public interface IScreenRepository
    {
        Task<Screen> AddAsync(Screen screen);

        Task<List<Screen>> GetAllAsync();

        Task<Screen?> GetByIdAsync(int id);

        Task<Screen?> UpdateAsync(Screen screen);

        Task<bool> DeleteAsync(int id);

        Task<List<Screen>> GetByTheatreIdAsync(int theatreId);
    }
}