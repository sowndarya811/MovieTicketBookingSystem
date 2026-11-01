using MovieTicketBookingSystem.Models;

namespace MovieTicketBookingSystem.Services.Interfaces
{
    public interface ITheatreService
    {
        Task<Theatre> AddAsync(Theatre theatre);

        Task<List<Theatre>> GetAllAsync();

        Task<Theatre?> GetByIdAsync(int id);

        Task<Theatre?> UpdateAsync(Theatre theatre);

        Task<bool> DeleteAsync(int id);
    }
}