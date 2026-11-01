using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Repositories.Interfaces;
using MovieTicketBookingSystem.Services.Interfaces;

namespace MovieTicketBookingSystem.Services
{
    public class SeatService : ISeatService
    {
        private readonly ISeatRepository _seatRepository;

        public SeatService(ISeatRepository seatRepository)
        {
            _seatRepository = seatRepository;
        }

        public async Task<Seat> AddAsync(Seat seat)
        {
            return await _seatRepository.AddAsync(seat);
        }

        public async Task<List<Seat>> GetAllAsync()
        {
            return await _seatRepository.GetAllAsync();
        }

        public async Task<Seat?> GetByIdAsync(int id)
        {
            return await _seatRepository.GetByIdAsync(id);
        }

        public async Task<Seat?> UpdateAsync(Seat seat)
        {
            return await _seatRepository.UpdateAsync(seat);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _seatRepository.DeleteAsync(id);
        }

        public async Task<List<Seat>> GetByScreenIdAsync(int screenId)
        {
            return await _seatRepository.GetByScreenIdAsync(screenId);
        }
    }
}