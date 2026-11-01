using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingSystem.DTOs.Screens;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Services.Interfaces;

namespace MovieTicketBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScreensController : ControllerBase
    {
        private readonly IScreenService _screenService;

        public ScreensController(IScreenService screenService)
        {
            _screenService = screenService;
        }

        [HttpPost]
        public async Task<IActionResult> AddScreen(CreateScreenDto dto)
        {
            var screen = new Screen
            {
                TheatreId = dto.TheatreId,
                ScreenName = dto.Name
            };

            var result = await _screenService.AddAsync(screen);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllScreens()
        {
            var result = await _screenService.GetAllAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetScreenById(int id)
        {
            var result = await _screenService.GetByIdAsync(id);

            if (result == null)
                return NotFound("Screen not found.");

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateScreen(
            int id,
            UpdateScreenDto dto)
        {
            var screen = new Screen
            {
                ScreenId = id,
                TheatreId = dto.TheatreId,
                ScreenName = dto.Name
            };

            var result = await _screenService.UpdateAsync(screen);

            if (result == null)
                return NotFound("Screen not found.");

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScreen(int id)
        {
            var result = await _screenService.DeleteAsync(id);

            if (!result)
                return NotFound("Screen not found.");

            return Ok("Screen deleted successfully.");
        }

        [HttpGet("theatre/{theatreId}")]
        public async Task<IActionResult> GetScreensByTheatreId(int theatreId)
        {
            var result = await _screenService.GetByTheatreIdAsync(theatreId);

            return Ok(result);
        }
    }
}