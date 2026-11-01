using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystem.Data;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Repositories.Interfaces;

namespace MovieTicketBookingSystem.Repositories
{
    public class ShowRepository : IShowRepository
    {
        private readonly ApplicationDbContext _context;

        public ShowRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // CREATE SHOW
        public async Task<Show> AddAsync(Show show)
        {
            // Validate start and end time
            if (show.EndTime <= show.StartTime)
            {
                throw new InvalidOperationException(
                    "End time must be later than start time.");
            }

            // Check overlapping shows on the same screen
            var overlappingShow = await _context.Shows
                .AnyAsync(s =>
                    s.ScreenId == show.ScreenId &&
                    s.IsActive &&
                    show.StartTime < s.EndTime &&
                    show.EndTime > s.StartTime);

            if (overlappingShow)
            {
                throw new InvalidOperationException(
                    "Another show already exists on this screen during the selected time.");
            }

            // Create show
            await _context.Shows.AddAsync(show);
            await _context.SaveChangesAsync();

            // Automatically create ShowSeats
            var seats = await _context.Seats
                .Where(s => s.ScreenId == show.ScreenId)
                .ToListAsync();

            var showSeats = seats
                .Select(seat => new ShowSeat
                {
                    ShowId = show.ShowId,
                    SeatId = seat.SeatId,
                    Status = "Available"
                })
                .ToList();

            if (showSeats.Count > 0)
            {
                await _context.ShowSeats.AddRangeAsync(showSeats);
                await _context.SaveChangesAsync();
            }

            return show;
        }

        // GET ALL SHOWS
        public async Task<List<Show>> GetAllAsync()
        {
            return await _context.Shows
                .ToListAsync();
        }

        // GET SHOW BY ID
        public async Task<Show?> GetByIdAsync(int id)
        {
            return await _context.Shows
                .FirstOrDefaultAsync(s => s.ShowId == id);
        }

        // UPDATE SHOW
        public async Task<Show?> UpdateAsync(Show show)
        {
            if (show.EndTime <= show.StartTime)
            {
                throw new InvalidOperationException(
                    "End time must be later than start time.");
            }

            // Check overlapping shows excluding current show
            var overlappingShow = await _context.Shows
                .AnyAsync(s =>
                    s.ShowId != show.ShowId &&
                    s.ScreenId == show.ScreenId &&
                    s.IsActive &&
                    show.StartTime < s.EndTime &&
                    show.EndTime > s.StartTime);

            if (overlappingShow)
            {
                throw new InvalidOperationException(
                    "Another show already exists on this screen during the selected time.");
            }

            var existingShow = await _context.Shows
                .FirstOrDefaultAsync(s => s.ShowId == show.ShowId);

            if (existingShow == null)
                return null;

            existingShow.MovieId = show.MovieId;
            existingShow.ScreenId = show.ScreenId;
            existingShow.StartTime = show.StartTime;
            existingShow.EndTime = show.EndTime;
            existingShow.BasePrice = show.BasePrice;
            existingShow.IsActive = show.IsActive;

            await _context.SaveChangesAsync();

            return existingShow;
        }

        // DELETE SHOW
        public async Task<bool> DeleteAsync(int id)
        {
            var show = await _context.Shows
                .FirstOrDefaultAsync(s => s.ShowId == id);

            if (show == null)
                return false;

            // Delete related ShowSeats first
            var showSeats = await _context.ShowSeats
                .Where(ss => ss.ShowId == id)
                .ToListAsync();

            if (showSeats.Count > 0)
            {
                _context.ShowSeats.RemoveRange(showSeats);
            }

            _context.Shows.Remove(show);

            await _context.SaveChangesAsync();

            return true;
        }

        // GET SHOWS BY MOVIE
        public async Task<List<Show>> GetByMovieIdAsync(int movieId)
        {
            return await _context.Shows
                .Where(s => s.MovieId == movieId)
                .ToListAsync();
        }

        // GET SHOWS BY SCREEN
        public async Task<List<Show>> GetByScreenIdAsync(int screenId)
        {
            return await _context.Shows
                .Where(s => s.ScreenId == screenId)
                .ToListAsync();
        }

        // GET ALL SHOW SEATS
        public async Task<List<ShowSeat>> GetShowSeatsAsync(int showId)
        {
            return await _context.ShowSeats
                .Where(ss => ss.ShowId == showId)
                .ToListAsync();
        }

        // GET AVAILABLE SHOW SEATS
        public async Task<List<ShowSeat>> GetAvailableShowSeatsAsync(int showId)
        {
            return await _context.ShowSeats
                .Where(ss =>
                    ss.ShowId == showId &&
                    ss.Status == "Available")
                .ToListAsync();
        }

        // GET BOOKED SHOW SEATS
        public async Task<List<ShowSeat>> GetBookedShowSeatsAsync(int showId)
        {
            return await _context.ShowSeats
                .Where(ss =>
                    ss.ShowId == showId &&
                    ss.Status == "Booked")
                .ToListAsync();
        }
    }
}