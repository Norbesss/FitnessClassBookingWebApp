using FitnessClassBookingWeb.DataAccess.UnitOfWork;
using FitnessClassBookingWeb.Models;
using FitnessClassBookingWeb.Models.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BCrypt.Net;

namespace FitnessClassBookingWebApp.Server.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto loginDto)
    {
        var user = await _unitOfWork.Users.FirstOrDefaultAsync(
            u => u.Email == loginDto.Email,
            u => u.UserRoles
        );

        if (user == null || !VerifyPassword(loginDto.Password, user.PasswordHash))
        {
            return null;
        }

        // Load roles separately
        var userRoles = await _unitOfWork.UserRoles.FindAsync(ur => ur.UserId == user.UserId);
        var roleIds = userRoles.Select(ur => ur.RoleId).ToList();

        var roles = new List<string>();
        foreach (var roleId in roleIds)
        {
            var role = await _unitOfWork.Roles.GetByIdAsync(roleId);
            if (role != null)
            {
                roles.Add(role.Name);
            }
        }

        var token = GenerateJwtToken(user.UserId, user.Email, roles);

        return new AuthResponseDto
        {
            UserId = user.UserId,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Roles = roles,
            Token = token
        };
    }

    public async Task<AuthResponseDto?> RegisterAsync(RegisterDto registerDto)
    {
        if (await UserExistsAsync(registerDto.Email))
        {
            return null;
        }

        var user = new User
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Email = registerDto.Email,
            PasswordHash = HashPassword(registerDto.Password),
            PhoneNumber = registerDto.PhoneNumber,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        var userRole = await _unitOfWork.Roles.FirstOrDefaultAsync(r => r.Name == "User");
        if (userRole != null)
        {
            await _unitOfWork.UserRoles.AddAsync(new UserRole
            {
                UserId = user.UserId,
                RoleId = userRole.RoleId
            });
            await _unitOfWork.SaveChangesAsync();
        }

        var roles = new List<string> { "User" };
        var token = GenerateJwtToken(user.UserId, user.Email, roles);

        return new AuthResponseDto
        {
            UserId = user.UserId,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Roles = roles,
            Token = token
        };
    }

    public async Task<bool> UserExistsAsync(string email)
    {
        return await _unitOfWork.Users.AnyAsync(u => u.Email == email);
    }

    public string GenerateJwtToken(int userId, string email, List<string> roles)
    {
        var jwtKey = _configuration["Jwt:Key"] ?? "YourSecretKeyForAuthenticationOfApplication_MustBeAtLeast32Characters";
        var jwtIssuer = _configuration["Jwt:Issuer"] ?? "FitnessClassBookingApp";
        var jwtAudience = _configuration["Jwt:Audience"] ?? "FitnessClassBookingAppUsers";

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Email, email)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
