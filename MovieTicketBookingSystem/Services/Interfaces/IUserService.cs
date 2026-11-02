using MovieTicketBookingSystem.DTOs.Auth;

namespace MovieTicketBookingSystem.Services.Interfaces
{
    public interface IUserService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);

        Task<AuthResponseDto?> LoginAsync(LoginDto dto);
    }
}