using System.Linq.Expressions;
using Users.Application.DTOs;
using Users.Application.Interfaces;
using Users.Domain.Constants;
using Users.Domain.Entities;
using Users.Domain.Exceptions;
using Users.Domain.Interfaces;

namespace Users.Application.Services;

public class UserService : IUserService
{
    private readonly IPasswordHash _hasher;
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository, IPasswordHash hasher)
    {
        _repository = repository;
        _hasher = hasher;
    }

    public async Task<ResultDto> CreateUser(UserRequestDto dto, bool isAdmin = false)
    {
        var userExists = await _repository.UserEmailAlreadyExists(dto.Email);

        if (userExists)
        {
            throw new DomainException(ExceptionMessageConstants.EmailAlreadyExistsException);
        }

        var user = new User(dto.Name, dto.Email, _hasher.Hash(dto.Password));

        if (isAdmin)
        {
            user.SetUserAdmin();
        }

        await _repository.CreateUser(user);

        return ResultDto.Ok();
    }

    public Task<ResultDto> DeleteUser(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<ResultDto<UserDto>> GetUser(Guid id)
    {
        throw new NotImplementedException();
    }

    public void Teste(Expression<Func<UserDto, bool>> predicate)
    {

    }

    public Task<ResultDto> UpdateUser(UserRequestDto dto)
    {
        throw new NotImplementedException();
    }
}
