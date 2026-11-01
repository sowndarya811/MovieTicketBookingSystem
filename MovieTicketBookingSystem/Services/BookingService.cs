using MovieTicketBookingSystem.DTOs.Bookings;
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

        public async Task<Booking?> CreateBookingAsync(CreateBookingDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.CustomerName))
                throw new InvalidOperationException(
                    "Customer name is required.");

            if (string.IsNullOrWhiteSpace(dto.CustomerPhone))
                throw new InvalidOperationException(
                    "Customer phone is required.");

            if (dto.SeatIds == null || dto.SeatIds.Count == 0)
                throw new InvalidOperationException(
                    "At least one seat must be selected.");

            var booking = new Booking
            {
                ShowId = dto.ShowId,
                CustomerName = dto.CustomerName,
                CustomerPhone = dto.CustomerPhone
            };

            return await _bookingRepository.CreateBookingAsync(
                booking,
                dto.SeatIds);
        }

        public async Task<Booking?> CancelBookingAsync(int bookingId)
        {
            return await _bookingRepository.CancelBookingAsync(bookingId);
        }
    }
}