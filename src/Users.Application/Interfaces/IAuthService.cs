using Users.Application.DTOs;

namespace Users.Application.Interfaces;

public interface IAuthService
{
    public Task<ResultDto<object>> Login(LoginRequestDto dto);
}
