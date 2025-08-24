using Users.Application.DTOs;

namespace Users.Application.Interfaces;

public interface IUserService
{
    public Task<ResultDto<UserDto>> GetUser(Guid id);
    public Task<ResultDto> CreateUser(UserRequestDto dto, bool isAdmin = false);
    public Task<ResultDto> UpdateUser(UserRequestDto dto, Guid id);
    public Task<ResultDto> DeleteUser(Guid id);
}
