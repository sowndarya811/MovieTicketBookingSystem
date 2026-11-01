using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Repositories.Interfaces;
using MovieTicketBookingSystem.Services.Interfaces;

namespace MovieTicketBookingSystem.Services
{
    public class TheatreService : ITheatreService
    {
        private readonly ITheatreRepository _theatreRepository;

        public TheatreService(ITheatreRepository theatreRepository)
        {
            _theatreRepository = theatreRepository;
        }

        public async Task<Theatre> AddAsync(Theatre theatre)
        {
            return await _theatreRepository.AddAsync(theatre);
        }

        public async Task<List<Theatre>> GetAllAsync()
        {
            return await _theatreRepository.GetAllAsync();
        }

        public async Task<Theatre?> GetByIdAsync(int id)
        {
            return await _theatreRepository.GetByIdAsync(id);
        }

        public async Task<Theatre?> UpdateAsync(Theatre theatre)
        {
            return await _theatreRepository.UpdateAsync(theatre);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _theatreRepository.DeleteAsync(id);
        }
    }
}