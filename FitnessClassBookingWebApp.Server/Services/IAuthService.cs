using FitnessClassBookingWeb.Models.DTOs;

namespace FitnessClassBookingWebApp.Server.Services;

public interface IAuthService
{
    Task<AuthResponseDto?> LoginAsync(LoginDto loginDto);
    Task<AuthResponseDto?> RegisterAsync(RegisterDto registerDto);
    Task<bool> UserExistsAsync(string email);
    string GenerateJwtToken(int userId, string email, List<string> roles);
}
