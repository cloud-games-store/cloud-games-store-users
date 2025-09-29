using FIAPCloudGamesStore.Logs.Services.Interfaces;
using Users.Application.DTOs;
using Users.Application.Interfaces;
using Users.Domain.Constants;
using Users.Domain.Entities;
using Users.Domain.Interfaces;

namespace Users.Application.Services;

public class UserService : IUserService
{
    private readonly IPasswordHash _hasher;
    private readonly IUserRepository _repository;
    private readonly IGenericTelemetryService<UserService> _telemetryService;

    public UserService(IUserRepository repository, IPasswordHash hasher, IGenericTelemetryService<UserService> telemetryService)
    {
        _repository = repository;
        _hasher = hasher;
        _telemetryService = telemetryService;
    }

    public async Task<ResultDto<UserDto>> CreateUser(UserRequestDto dto, bool isAdmin = false)
    {
        var userExists = await _repository.UserEmailAlreadyExists(dto.Email);

        if (userExists)
        {
            _telemetryService.LogTextError(nameof(CreateUser), ExceptionMessageConstants.EmailAlreadyExistsException);
            return ResultDto<UserDto>.Fail(ValueObjects.Error.BadRequest(ExceptionMessageConstants.EmailAlreadyExistsException));
        }

        var user = new User(dto.Name, dto.Email, _hasher.Hash(dto.Password));

        if (isAdmin)
        {
            user.SetUserAdmin();
        }

        var userId = await _repository.CreateUser(user);

        return ResultDto<UserDto>.Ok(new UserDto(userId, user?.Name, user?.Email));
    }

    public async Task<ResultDto> DeleteUser(Guid id)
    {
        var user = await _repository.GetUser(id);

        if (user is null)
        {
            _telemetryService.LogTextError(nameof(DeleteUser), ExceptionMessageConstants.UserNotExistsException);
            return ResultDto.Fail(ValueObjects.Error.BadRequest(ExceptionMessageConstants.UserNotExistsException));
        }

        await _repository.DeleteUser(user);

        return ResultDto.Ok();
    }

    public async Task<ResultDto<UserDto?>> GetUser(Guid id)
    {
        var user = await _repository.GetUser(id);

        var dto = user is not null ? new UserDto(user?.Id, user?.Name, user?.Email) : null;

        return ResultDto<UserDto?>.Ok(dto);
    }

    public async Task<ResultDto> UpdateUser(UserRequestDto dto, Guid id)
    {
        var user = await _repository.GetUser(id);

        if (user is null)
        {
            _telemetryService.LogTextError(nameof(UpdateUser), ExceptionMessageConstants.UserNotExistsException);
            return ResultDto.Fail(ValueObjects.Error.BadRequest(ExceptionMessageConstants.UserNotExistsException));
        }

        user.Update(dto.Name, dto.Email, _hasher.Hash(dto.Password));

        await _repository.UpdateUser(user);

        return ResultDto.Ok();
    }
}
