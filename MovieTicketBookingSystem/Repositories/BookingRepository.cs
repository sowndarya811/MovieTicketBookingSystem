using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystem.Data;
using MovieTicketBookingSystem.DTOs.Bookings;
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

        // CREATE BOOKING
        public async Task<Booking> CreateBookingAsync(CreateBookingDto dto)
        {
            if (dto.SeatIds == null || dto.SeatIds.Count == 0)
                throw new InvalidOperationException("At least one seat must be selected.");

            if (string.IsNullOrWhiteSpace(dto.CustomerName))
                throw new InvalidOperationException("Customer name is required.");

            if (string.IsNullOrWhiteSpace(dto.CustomerPhone))
                throw new InvalidOperationException("Customer phone is required.");

            // Duplicate seat validation
            if (dto.SeatIds.Count != dto.SeatIds.Distinct().Count())
                throw new InvalidOperationException("Duplicate seat IDs are not allowed.");

            // Get show
            var show = await _context.Shows
                .FirstOrDefaultAsync(s => s.ShowId == dto.ShowId);

            if (show == null)
                throw new KeyNotFoundException("Show not found.");

            if (!show.IsActive)
                throw new InvalidOperationException("This show is inactive.");

            // Check selected seats
            var seats = await _context.Seats
                .Where(s => dto.SeatIds.Contains(s.SeatId))
                .ToListAsync();

            if (seats.Count != dto.SeatIds.Count)
                throw new KeyNotFoundException("One or more selected seats were not found.");

            // Check seats belong to the same screen as the show
            if (seats.Any(s => s.ScreenId != show.ScreenId))
                throw new InvalidOperationException(
                    "One or more selected seats do not belong to the show's screen.");

            // Get ShowSeats
            var showSeats = await _context.ShowSeats
                .Where(ss =>
                    ss.ShowId == dto.ShowId &&
                    dto.SeatIds.Contains(ss.SeatId))
                .ToListAsync();

            if (showSeats.Count != dto.SeatIds.Count)
                throw new InvalidOperationException(
                    "One or more selected seats are not available for this show.");

            // Check availability
            if (showSeats.Any(ss => ss.Status != "Available"))
                throw new InvalidOperationException(
                    "One or more selected seats are already booked.");

            // Calculate total amount from server-side show price
            decimal totalAmount = show.BasePrice * dto.SeatIds.Count;

            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                // Create Booking
                var booking = new Booking
                {
                    ShowId = dto.ShowId,
                    CustomerName = dto.CustomerName,
                    CustomerPhone = dto.CustomerPhone,
                    BookingDate = DateTime.Now,
                    TotalAmount = totalAmount,
                    Status = "Confirmed"
                };

                await _context.Bookings.AddAsync(booking);
                await _context.SaveChangesAsync();

                // Create BookingSeats
                foreach (var showSeat in showSeats)
                {
                    var bookingSeat = new BookingSeat
                    {
                        BookingId = booking.BookingId,
                        SeatId = showSeat.SeatId,
                        Price = show.BasePrice
                    };

                    await _context.BookingSeats.AddAsync(bookingSeat);

                    // Update ShowSeat status
                    showSeat.Status = "Booked";
                }

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return booking;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // GET ALL
        public async Task<List<Booking>> GetAllAsync()
        {
            return await _context.Bookings
                .ToListAsync();
        }

        // GET BY ID
        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await _context.Bookings
                .FirstOrDefaultAsync(b => b.BookingId == id);
        }

        // UPDATE
        public async Task<Booking?> UpdateAsync(Booking booking)
        {
            var existingBooking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.BookingId == booking.BookingId);

            if (existingBooking == null)
                return null;

            existingBooking.ShowId = booking.ShowId;
            existingBooking.CustomerName = booking.CustomerName;
            existingBooking.CustomerPhone = booking.CustomerPhone;
            existingBooking.Status = booking.Status;

            await _context.SaveChangesAsync();

            return existingBooking;
        }

        // DELETE
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

        // GET BOOKINGS BY SHOW
        public async Task<List<Booking>> GetByShowIdAsync(int showId)
        {
            return await _context.Bookings
                .Where(b => b.ShowId == showId)
                .ToListAsync();
        }

        // GET BOOKING SEATS
        public async Task<List<BookingSeat>> GetBookingSeatsAsync(int bookingId)
        {
            return await _context.BookingSeats
                .Where(bs => bs.BookingId == bookingId)
                .ToListAsync();
        }

        // ADD BOOKING SEAT
        public async Task AddBookingSeatAsync(BookingSeat bookingSeat)
        {
            await _context.BookingSeats.AddAsync(bookingSeat);

            await _context.SaveChangesAsync();
        }

        // CANCEL BOOKING
        public async Task<bool> CancelBookingAsync(int bookingId)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);

            if (booking == null)
                return false;

            if (booking.Status == "Cancelled")
                throw new InvalidOperationException(
                    "Booking is already cancelled.");

            var bookingSeats = await _context.BookingSeats
                .Where(bs => bs.BookingId == bookingId)
                .ToListAsync();

            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                // Change booking status
                booking.Status = "Cancelled";

                // Get corresponding ShowSeats
                var seatIds = bookingSeats
                    .Select(bs => bs.SeatId)
                    .ToList();

                var showSeats = await _context.ShowSeats
                    .Where(ss =>
                        ss.ShowId == booking.ShowId &&
                        seatIds.Contains(ss.SeatId))
                    .ToListAsync();

                // Make seats available again
                foreach (var showSeat in showSeats)
                {
                    showSeat.Status = "Available";
                }

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}