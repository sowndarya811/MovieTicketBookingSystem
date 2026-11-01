using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystem.Data;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Repositories.Interfaces;

namespace MovieTicketBookingSystem.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Booking> AddAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();

            return booking;
        }

        public async Task<List<Booking>> GetAllAsync()
        {
            return await _context.Bookings.ToListAsync();
        }

        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await _context.Bookings
                .FirstOrDefaultAsync(b => b.BookingId == id);
        }

        public async Task<Booking?> UpdateAsync(Booking booking)
        {
            var existingBooking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.BookingId == booking.BookingId);

            if (existingBooking == null)
                return null;

            existingBooking.ShowId = booking.ShowId;
            existingBooking.CustomerName = booking.CustomerName;
            existingBooking.CustomerPhone = booking.CustomerPhone;
            existingBooking.BookingDate = booking.BookingDate;
            existingBooking.TotalAmount = booking.TotalAmount;
            existingBooking.Status = booking.Status;

            await _context.SaveChangesAsync();

            return existingBooking;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null)
                return false;

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Booking>> GetByShowIdAsync(int showId)
        {
            return await _context.Bookings
                .Where(b => b.ShowId == showId)
                .ToListAsync();
        }

        public async Task<List<BookingSeat>> GetBookingSeatsAsync(int bookingId)
        {
            return await _context.BookingSeats
                .Where(bs => bs.BookingId == bookingId)
                .ToListAsync();
        }

        public async Task AddBookingSeatAsync(BookingSeat bookingSeat)
        {
            await _context.BookingSeats.AddAsync(bookingSeat);
            await _context.SaveChangesAsync();
        }
    }
}