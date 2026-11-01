using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingSystem.DTOs.Theatres;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Services.Interfaces;

namespace MovieTicketBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TheatresController : ControllerBase
    {
        private readonly ITheatreService _theatreService;

        public TheatresController(ITheatreService theatreService)
        {
            _theatreService = theatreService;
        }

        [HttpPost]
        public async Task<IActionResult> AddTheatre(CreateTheatreDto dto)
        {
            var theatre = new Theatre
            {
                Name = dto.Name,
                Location = $"{dto.Address}, {dto.City}"
            };

            var result = await _theatreService.AddAsync(theatre);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTheatres()
        {
            var result = await _theatreService.GetAllAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTheatreById(int id)
        {
            var result = await _theatreService.GetByIdAsync(id);

            if (result == null)
                return NotFound("Theatre not found.");

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTheatre(
            int id,
            UpdateTheatreDto dto)
        {
            var theatre = new Theatre
            {
                TheatreId = id,
                Name = dto.Name,
                Location = $"{dto.Address}, {dto.City}"
            };

            var result = await _theatreService.UpdateAsync(theatre);

            if (result == null)
                return NotFound("Theatre not found.");

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTheatre(int id)
        {
            var result = await _theatreService.DeleteAsync(id);

            if (!result)
                return NotFound("Theatre not found.");

            return Ok("Theatre deleted successfully.");
        }
    }
}