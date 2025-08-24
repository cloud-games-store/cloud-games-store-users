using Moq;
using System.ComponentModel.DataAnnotations;
using Users.Application.DTOs;
using Users.Application.Interfaces;
using Users.Application.Services;
using Users.Domain.Constants;
using Users.Domain.Entities;
using Users.Domain.Exceptions;
using Users.Domain.Interfaces;

namespace Users.Tests.Services;

public class UserServiceTests
{
    private readonly IUserService _service;
    private readonly Mock<IPasswordHash> _hasher;
    private readonly Mock<IUserRepository> _repository;

    public UserServiceTests()
    {
        _hasher = new Mock<IPasswordHash>();
        _repository = new Mock<IUserRepository>();
        _service = new UserService(_repository.Object, _hasher.Object);
    }


    [Theory]
    [InlineData("Luana", "luana@gmail.com", "senha123@1")]
    [InlineData("Mel", "mel@gmail.com", "OutraSenha!90")]
    [InlineData("Carlos Alberto", "carlos@gmail.com", "senhaNaoHasheada1#")]
    [InlineData("João da Silva", "joazinhodasilva@hotmail.com", "1234joao$0")]
    public async Task CreateUser_ValidParameters_ShouldReturnsOk(string name, string email, string password)
    {
        var dto = new UserRequestDto()
        {
            Name = name,
            Email = email,
            Password = password
        };

        _hasher.Setup(service => service.Hash(dto.Password)).Returns((string password) => BCrypt.Net.BCrypt.HashPassword(password));
        _repository.Setup(repository => repository.CreateUser(It.IsAny<User>()));

        var result = await _service.CreateUser(dto);

        Assert.True(result.Success);
    }

    [Theory]
    [InlineData("Luana", "luana@gmail.com", "senha123")]
    [InlineData("Mel", "mel@gmail.com", "OutraSenha90")]
    [InlineData("Carlos Alberto", "", "senhaNaoHasheada1#")]
    [InlineData("", "joazinhodasilva@hotmail.com", "1234joao$0")]
    public async Task UserRequestDto_InvalidParameters_ShouldReturnValidationErrors(string name, string email, string password)
    {
        var dto = new UserRequestDto()
        {
            Name = name,
            Email = email,
            Password = password
        };

        var context = new ValidationContext(dto);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(dto, context, results, validateAllProperties: true);

        Assert.False(isValid);
        Assert.NotEmpty(results);
    }

    [Theory]
    [InlineData("Luana", "luana@gmail.com", "senha123@1")]
    public async Task CreateUser_EmailAlreadyExists_ShouldThrowException(string name, string email, string password)
    {
        var dto = new UserRequestDto()
        {
            Name = name,
            Email = email,
            Password = password
        };

        _hasher.Setup(service => service.Hash(dto.Password)).Returns((string password) => BCrypt.Net.BCrypt.HashPassword(password));
        _repository.Setup(repository => repository.UserEmailAlreadyExists(dto.Email)).ReturnsAsync((string email) => true);

        var exception = await Assert.ThrowsAsync<DomainException>(() => _service.CreateUser(dto));
        Assert.Equal(ExceptionMessageConstants.EmailAlreadyExistsException, exception.Message);
    }

    [Theory]
    [InlineData("Luana", "luana@gmail.com", "senha123@1")]
    public async Task UpdateUser_ValidParameters_ShouldUpdateUser(string name, string email, string password)
    {
        var user = new User("Name Test", "test@gmail.com", "password");

        var dto = new UserRequestDto()
        {
            Name = name,
            Email = email,
            Password = password
        };

        _hasher.Setup(service => service.Hash(dto.Password)).Returns((string password) => BCrypt.Net.BCrypt.HashPassword(password));
        _repository.Setup(repository => repository.GetUser(It.IsAny<Guid>())).ReturnsAsync((Guid id) => user);

        var result = await _service.UpdateUser(dto, Guid.NewGuid());

        Assert.True(result.Success);
    }

    [Theory]
    [InlineData("Luana", "luana@gmail.com", "senha123@1")]
    public async Task UpdateUser_UserNotExists_ShouldThrowException(string name, string email, string password)
    {
        var dto = new UserRequestDto()
        {
            Name = name,
            Email = email,
            Password = password
        };

        _repository.Setup(repository => repository.GetUser(It.IsAny<Guid>())).ReturnsAsync((Guid id) => null);

        var exception = await Assert.ThrowsAsync<DomainException>(() => _service.UpdateUser(dto, Guid.NewGuid()));
        Assert.Equal(ExceptionMessageConstants.UserNotExistsException, exception.Message);
    }
}
