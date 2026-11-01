using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Repositories.Interfaces;
using MovieTicketBookingSystem.Services.Interfaces;

namespace MovieTicketBookingSystem.Services
{
    public class ScreenService : IScreenService
    {
        private readonly IScreenRepository _screenRepository;

        public ScreenService(IScreenRepository screenRepository)
        {
            _screenRepository = screenRepository;
        }

        public async Task<Screen> AddAsync(Screen screen)
        {
            return await _screenRepository.AddAsync(screen);
        }

        public async Task<List<Screen>> GetAllAsync()
        {
            return await _screenRepository.GetAllAsync();
        }

        public async Task<Screen?> GetByIdAsync(int id)
        {
            return await _screenRepository.GetByIdAsync(id);
        }

        public async Task<Screen?> UpdateAsync(Screen screen)
        {
            return await _screenRepository.UpdateAsync(screen);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _screenRepository.DeleteAsync(id);
        }

        public async Task<List<Screen>> GetByTheatreIdAsync(int theatreId)
        {
            return await _screenRepository.GetByTheatreIdAsync(theatreId);
        }
    }
}