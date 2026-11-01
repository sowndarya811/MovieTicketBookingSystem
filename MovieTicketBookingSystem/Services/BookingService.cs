using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Repositories.Interfaces;
using MovieTicketBookingSystem.Services.Interfaces;

namespace MovieTicketBookingSystem.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<Booking> AddAsync(Booking booking)
        {
            return await _bookingRepository.AddAsync(booking);
        }

        public async Task<List<Booking>> GetAllAsync()
        {
            return await _bookingRepository.GetAllAsync();
        }

        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await _bookingRepository.GetByIdAsync(id);
        }

        public async Task<Booking?> UpdateAsync(Booking booking)
        {
            return await _bookingRepository.UpdateAsync(booking);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _bookingRepository.DeleteAsync(id);
        }

        public async Task<List<Booking>> GetByShowIdAsync(int showId)
        {
            return await _bookingRepository.GetByShowIdAsync(showId);
        }

        public async Task<List<BookingSeat>> GetBookingSeatsAsync(int bookingId)
        {
            return await _bookingRepository.GetBookingSeatsAsync(bookingId);
        }

        public async Task AddBookingSeatAsync(BookingSeat bookingSeat)
        {
            await _bookingRepository.AddBookingSeatAsync(bookingSeat);
        }
    }
}