using MovieTicketBookingSystem.Models;

namespace MovieTicketBookingSystem.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        // Add Booking
        Task<Booking> AddAsync(Booking booking);

        // Get All Bookings
        Task<List<Booking>> GetAllAsync();

        // Get Booking By ID
        Task<Booking?> GetByIdAsync(int id);

        // Update Booking
        Task<Booking?> UpdateAsync(Booking booking);

        // Delete Booking
        Task<bool> DeleteAsync(int id);

        // Get Bookings By Show ID
        Task<List<Booking>> GetByShowIdAsync(int showId);

        // Get Bookings By Customer/User ID
        Task<List<Booking>> GetByUserIdAsync(int userId);

        // Get Seats For A Booking
        Task<List<BookingSeat>> GetBookingSeatsAsync(int bookingId);

        // Add Booking Seat
        Task AddBookingSeatAsync(BookingSeat bookingSeat);

        // Create Booking With Selected Seats
        Task<Booking?> CreateBookingAsync(
            Booking booking,
            List<int> seatIds,
            int userId);

        // Cancel Booking
        Task<Booking?> CancelBookingAsync(int bookingId);
    }
}