using MovieTicketBookingSystem.DTOs.Bookings;
using MovieTicketBookingSystem.Models;

namespace MovieTicketBookingSystem.Services.Interfaces
{
    public interface IBookingService
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

        // Get Bookings By User ID
        Task<List<Booking>> GetByUserIdAsync(int userId);

        // Get Booking Seats
        Task<List<BookingSeat>> GetBookingSeatsAsync(int bookingId);

        // Add Booking Seat
        Task AddBookingSeatAsync(BookingSeat bookingSeat);

        // Create Booking
        Task<Booking?> CreateBookingAsync(
            CreateBookingDto dto,
            int userId);

        // Cancel Booking
        Task<Booking?> CancelBookingAsync(int bookingId);
    }
}