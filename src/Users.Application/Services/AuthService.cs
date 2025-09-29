using FIAPCloudGamesStore.Logs.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Users.Application.DTOs;
using Users.Application.Interfaces;
using Users.Domain.Constants;
using Users.Domain.Entities;
using Users.Domain.Interfaces;

namespace Users.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHash _hasher;
    private readonly IConfiguration _configuration;
    private readonly IGenericTelemetryService<AuthService> _telemetryService;

    public AuthService(IUserRepository userRepository, IPasswordHash hasher, IConfiguration configuration, IGenericTelemetryService<AuthService> telemetryService)
    {
        _hasher = hasher;
        _userRepository = userRepository;
        _configuration = configuration;
        _telemetryService = telemetryService;
    }

    public async Task<ResultDto<string>> Login(LoginRequestDto dto)
    {
        var user = await _userRepository.GetUser(dto.Email);

        if (user is null)
        {
            _telemetryService.LogTextError(nameof(Login), $"User is null, {dto.Email}");
            return ResultDto<string>.Fail(ValueObjects.Error.Unauthorized(ExceptionMessageConstants.UserUnauthorized));
        }

        var samePassword = _hasher.VerifyPassword(dto.Password, user.PasswordHash);

        if (!samePassword)
        {
            _telemetryService.LogTextError(nameof(Login), $"Wrong password, {dto.Email}");
            return ResultDto<string>.Fail(ValueObjects.Error.Unauthorized(ExceptionMessageConstants.UserUnauthorized));
        }

        return ResultDto<string>.Ok(GenerateJwt(user));
    }

    private string GenerateJwt(User user)
    {
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(3),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            ),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Issuer"]
        };

        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }
}
