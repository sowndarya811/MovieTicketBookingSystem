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

        public async Task<Show> AddAsync(Show show)
        {
            await _context.Shows.AddAsync(show);
            await _context.SaveChangesAsync();

            return show;
        }

        public async Task<List<Show>> GetAllAsync()
        {
            return await _context.Shows.ToListAsync();
        }

        public async Task<Show?> GetByIdAsync(int id)
        {
            return await _context.Shows
                .FirstOrDefaultAsync(s => s.ShowId == id);
        }

        public async Task<Show?> UpdateAsync(Show show)
        {
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

        public async Task<bool> DeleteAsync(int id)
        {
            var show = await _context.Shows
                .FirstOrDefaultAsync(s => s.ShowId == id);

            if (show == null)
                return false;

            _context.Shows.Remove(show);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Show>> GetByMovieIdAsync(int movieId)
        {
            return await _context.Shows
                .Where(s => s.MovieId == movieId)
                .ToListAsync();
        }

        public async Task<List<Show>> GetByScreenIdAsync(int screenId)
        {
            return await _context.Shows
                .Where(s => s.ScreenId == screenId)
                .ToListAsync();
        }
    }
}