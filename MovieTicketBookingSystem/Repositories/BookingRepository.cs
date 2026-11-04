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

        public async Task<List<Booking>> GetByUserIdAsync(int userId)
        {
            return await _context.Bookings
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
        }

        public async Task<Booking?> UpdateAsync(Booking booking)
        {
            var existing = await _context.Bookings
                .FirstOrDefaultAsync(b => b.BookingId == booking.BookingId);

            if (existing == null)
                return null;

            existing.ShowId = booking.ShowId;
            existing.CustomerName = booking.CustomerName;
            existing.CustomerPhone = booking.CustomerPhone;
            existing.Status = booking.Status;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null)
                return false;

            var seats = await _context.BookingSeats
                .Where(bs => bs.BookingId == id)
                .ToListAsync();

            _context.BookingSeats.RemoveRange(seats);
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

        public async Task<Booking?> CreateBookingAsync(
            Booking booking,
            List<int> seatIds,
            int userId)
        {
            using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                var show = await _context.Shows
                    .FirstOrDefaultAsync(s =>
                        s.ShowId == booking.ShowId && s.IsActive);

                if (show == null)
                    throw new InvalidOperationException(
                        "Show not found or inactive.");

                if (seatIds == null || seatIds.Count == 0)
                    throw new InvalidOperationException(
                        "At least one seat must be selected.");

                if (seatIds.Distinct().Count() != seatIds.Count)
                    throw new InvalidOperationException(
                        "Duplicate seats are not allowed.");

                var showSeats = await _context.ShowSeats
                    .Where(ss =>
                        ss.ShowId == booking.ShowId &&
                        seatIds.Contains(ss.SeatId))
                    .ToListAsync();

                if (showSeats.Count != seatIds.Count)
                    throw new InvalidOperationException(
                        "Invalid seats selected.");

                if (showSeats.Any(ss => ss.Status != "Available"))
                    throw new InvalidOperationException(
                        "One or more seats are already booked.");

                booking.UserId = userId;
                booking.TotalAmount = seatIds.Count * show.BasePrice;
                booking.Status = "Confirmed";
                booking.BookingDate = DateTime.Now;

                await _context.Bookings.AddAsync(booking);
                await _context.SaveChangesAsync();

                foreach (var showSeat in showSeats)
                {
                    showSeat.Status = "Booked";

                    await _context.BookingSeats.AddAsync(
                        new BookingSeat
                        {
                            BookingId = booking.BookingId,
                            SeatId = showSeat.SeatId,
                            Price = show.BasePrice
                        });
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

        public async Task<Booking?> CancelBookingAsync(int bookingId)
        {
            using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                var booking = await _context.Bookings
                    .FirstOrDefaultAsync(b => b.BookingId == bookingId);

                if (booking == null)
                    return null;

                if (booking.Status == "Cancelled")
                    throw new InvalidOperationException(
                        "Booking is already cancelled.");

                var bookingSeats = await _context.BookingSeats
                    .Where(bs => bs.BookingId == bookingId)
                    .ToListAsync();

                foreach (var item in bookingSeats)
                {
                    var showSeat = await _context.ShowSeats
                        .FirstOrDefaultAsync(ss =>
                            ss.ShowId == booking.ShowId &&
                            ss.SeatId == item.SeatId);

                    if (showSeat != null)
                        showSeat.Status = "Available";
                }

                booking.Status = "Cancelled";

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
    }
}