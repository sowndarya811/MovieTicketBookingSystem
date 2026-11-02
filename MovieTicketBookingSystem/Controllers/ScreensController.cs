using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Services.Interfaces;

namespace MovieTicketBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ScreensController : ControllerBase
    {
        private readonly IScreenService _screenService;

        public ScreensController(IScreenService screenService)
        {
            _screenService = screenService;
        }

        // Admin only
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddScreen(Screen screen)
        {
            var result = await _screenService.AddAsync(screen);

            return Ok(result);
        }

        // Customer + Admin
        [Authorize(Roles = "Customer,Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllScreens()
        {
            var result = await _screenService.GetAllAsync();

            return Ok(result);
        }

        // Customer + Admin
        [Authorize(Roles = "Customer,Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetScreenById(int id)
        {
            var result = await _screenService.GetByIdAsync(id);

            if (result == null)
                return NotFound("Screen not found.");

            return Ok(result);
        }

        // Customer + Admin
        [Authorize(Roles = "Customer,Admin")]
        [HttpGet("theatre/{theatreId}")]
        public async Task<IActionResult> GetScreensByTheatreId(
            int theatreId)
        {
            var result =
                await _screenService.GetByTheatreIdAsync(theatreId);

            return Ok(result);
        }

        // Admin only
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateScreen(
            int id,
            Screen screen)
        {
            screen.ScreenId = id;

            var result =
                await _screenService.UpdateAsync(screen);

            if (result == null)
                return NotFound("Screen not found.");

            return Ok(result);
        }

        // Admin only
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScreen(int id)
        {
            var result =
                await _screenService.DeleteAsync(id);

            if (!result)
                return NotFound("Screen not found.");

            return Ok("Screen deleted successfully.");
        }
    }
}