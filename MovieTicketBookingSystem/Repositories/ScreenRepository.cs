using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystem.Data;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Repositories.Interfaces;

namespace MovieTicketBookingSystem.Repositories
{
    public class ScreenRepository : IScreenRepository
    {
        private readonly ApplicationDbContext _context;

        public ScreenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Screen> AddAsync(Screen screen)
        {
            await _context.Screens.AddAsync(screen);
            await _context.SaveChangesAsync();

            return screen;
        }

        public async Task<List<Screen>> GetAllAsync()
        {
            return await _context.Screens
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Screen?> GetByIdAsync(int id)
        {
            return await _context.Screens
                .FirstOrDefaultAsync(s => s.ScreenId == id);
        }

        public async Task<Screen?> UpdateAsync(Screen screen)
        {
            var existingScreen = await _context.Screens
                .FirstOrDefaultAsync(s => s.ScreenId == screen.ScreenId);

            if (existingScreen == null)
            {
                return null;
            }

            existingScreen.TheatreId = screen.TheatreId;
            existingScreen.ScreenName = screen.ScreenName;

            await _context.SaveChangesAsync();

            return existingScreen;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var screen = await _context.Screens
                .FirstOrDefaultAsync(s => s.ScreenId == id);

            if (screen == null)
            {
                return false;
            }

            _context.Screens.Remove(screen);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Screen>> GetByTheatreIdAsync(int theatreId)
        {
            return await _context.Screens
                .Where(s => s.TheatreId == theatreId)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}