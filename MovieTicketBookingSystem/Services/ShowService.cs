using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Repositories.Interfaces;
using MovieTicketBookingSystem.Services.Interfaces;

namespace MovieTicketBookingSystem.Services
{
    public class ShowService : IShowService
    {
        private readonly IShowRepository _showRepository;

        public ShowService(IShowRepository showRepository)
        {
            _showRepository = showRepository;
        }

        public async Task<Show> AddAsync(Show show)
        {
            return await _showRepository.AddAsync(show);
        }

        public async Task<List<Show>> GetAllAsync()
        {
            return await _showRepository.GetAllAsync();
        }

        public async Task<Show?> GetByIdAsync(int id)
        {
            return await _showRepository.GetByIdAsync(id);
        }

        public async Task<Show?> UpdateAsync(Show show)
        {
            return await _showRepository.UpdateAsync(show);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _showRepository.DeleteAsync(id);
        }

        public async Task<List<Show>> GetByMovieIdAsync(int movieId)
        {
            return await _showRepository.GetByMovieIdAsync(movieId);
        }

        public async Task<List<Show>> GetByScreenIdAsync(int screenId)
        {
            return await _showRepository.GetByScreenIdAsync(screenId);
        }
    }
}