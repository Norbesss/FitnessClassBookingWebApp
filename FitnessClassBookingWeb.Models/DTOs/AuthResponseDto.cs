namespace FitnessClassBookingWeb.Models.DTOs;

public class AuthResponseDto
{
    public int UserId { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<string> Roles { get; set; }
    public string Token { get; set; }
}
