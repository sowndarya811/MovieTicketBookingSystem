using MovieTicketBookingSystem.Models;

namespace MovieTicketBookingSystem.Services.Interfaces
{
    public interface ISeatService
    {
        Task<Seat> AddAsync(Seat seat);

        Task<List<Seat>> GetAllAsync();

        Task<Seat?> GetByIdAsync(int id);

        Task<Seat?> UpdateAsync(Seat seat);

        Task<bool> DeleteAsync(int id);

        Task<List<Seat>> GetByScreenIdAsync(int screenId);
    }
}