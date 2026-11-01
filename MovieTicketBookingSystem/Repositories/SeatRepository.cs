using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystem.Data;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Repositories.Interfaces;

namespace MovieTicketBookingSystem.Repositories
{
    public class SeatRepository : ISeatRepository
    {
        private readonly ApplicationDbContext _context;

        public SeatRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Seat> AddAsync(Seat seat)
        {
            await _context.Seats.AddAsync(seat);
            await _context.SaveChangesAsync();

            return seat;
        }

        public async Task<List<Seat>> GetAllAsync()
        {
            return await _context.Seats.ToListAsync();
        }

        public async Task<Seat?> GetByIdAsync(int id)
        {
            return await _context.Seats
                .FirstOrDefaultAsync(s => s.SeatId == id);
        }

        public async Task<Seat?> UpdateAsync(Seat seat)
        {
            var existingSeat = await _context.Seats
                .FirstOrDefaultAsync(s => s.SeatId == seat.SeatId);

            if (existingSeat == null)
                return null;

            existingSeat.ScreenId = seat.ScreenId;
            existingSeat.SeatNumber = seat.SeatNumber;
            existingSeat.SeatType = seat.SeatType;
            existingSeat.IsAvailable = seat.IsAvailable;

            await _context.SaveChangesAsync();

            return existingSeat;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var seat = await _context.Seats
                .FirstOrDefaultAsync(s => s.SeatId == id);

            if (seat == null)
                return false;

            _context.Seats.Remove(seat);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Seat>> GetByScreenIdAsync(int screenId)
        {
            return await _context.Seats
                .Where(s => s.ScreenId == screenId)
                .ToListAsync();
        }
    }
}