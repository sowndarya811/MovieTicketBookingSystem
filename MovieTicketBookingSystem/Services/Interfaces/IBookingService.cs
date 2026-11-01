using MovieTicketBookingSystem.DTOs.Bookings;
using MovieTicketBookingSystem.Models;

namespace MovieTicketBookingSystem.Services.Interfaces
{
    public interface IBookingService
    {
        Task<Booking> AddAsync(Booking booking);

        Task<List<Booking>> GetAllAsync();

        Task<Booking?> GetByIdAsync(int id);

        Task<Booking?> UpdateAsync(Booking booking);

        Task<bool> DeleteAsync(int id);

        Task<List<Booking>> GetByShowIdAsync(int showId);

        Task<List<BookingSeat>> GetBookingSeatsAsync(int bookingId);

        Task AddBookingSeatAsync(BookingSeat bookingSeat);

        Task<Booking?> CreateBookingAsync(CreateBookingDto dto);

        Task<Booking?> CancelBookingAsync(int bookingId);
    }
}