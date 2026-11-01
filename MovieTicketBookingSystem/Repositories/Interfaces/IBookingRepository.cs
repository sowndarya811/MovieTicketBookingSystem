using MovieTicketBookingSystem.Models;

namespace MovieTicketBookingSystem.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking> AddAsync(Booking booking);
        Task<List<Booking>> GetAllAsync();
        Task<Booking?> GetByIdAsync(int id);
        Task<Booking?> UpdateAsync(Booking booking);
        Task<bool> DeleteAsync(int id);

        Task<List<Booking>> GetByShowIdAsync(int showId);

        Task<List<BookingSeat>> GetBookingSeatsAsync(int bookingId);

        Task AddBookingSeatAsync(BookingSeat bookingSeat);

        Task<Booking?> CreateBookingAsync(
            Booking booking,
            List<int> seatIds);

        Task<Booking?> CancelBookingAsync(int bookingId);
    }
}