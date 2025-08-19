using Users.Domain.Constants;
using Users.Domain.Entities;
using Users.Domain.Enums;
using Users.Domain.Exceptions;

namespace Users.Tests.Domain;

public class UserDomainTests
{
    [Theory]
    [InlineData("Luana", "luana@gmail.com", "senhaNaoHasheada")]
    [InlineData("Mel", "mel@gmail.com", "senhaNaoHasheada")]
    [InlineData("Carlos Alberto", "carlos@gmail.com", "senhaNaoHasheada")]
    [InlineData("João da Silva", "joazinhodasilva@hotmail.com", "senhaNaoHasheada")]
    public void Constructor_ValidParameters_ShouldCreateUser(string name, string email, string password)
    {
        var user = new User(name, email, password);

        Assert.NotNull(user);
        Assert.Equal(name, user.Name);
        Assert.Equal(email, user.Email);
        Assert.Equal(password, user.PasswordHash);
    }

    [Theory]
    [InlineData("", "luana@gmail.com", "senhaNaoHasheada", ExceptionMessageConstants.NameException)]
    [InlineData("lu", "luana@gmail.com", "senhaNaoHasheada", ExceptionMessageConstants.NameException)]
    [InlineData("Carlos Alberto", "", "senhaNaoHasheada", ExceptionMessageConstants.EmailEmptyException)] 
    [InlineData("João da Silva", "joazinhodasilva@hotmail.com", "", ExceptionMessageConstants.PasswordEmptyException)]
    public void Constructor_InvalidParameters_ShouldThrowDomainException(string name, string email, string password, string exceptionMessage)
    {
        var exception = Assert.Throws<DomainException>(() => new User(name, email, password));
        Assert.Equal(exceptionMessage, exception.Message);
    }

    [Theory]
    [InlineData("Luana", "luana@gmail.com", "senhaNaoHasheada")]
    [InlineData("Mel", "mel@gmail.com", "senhaNaoHasheada")]
    [InlineData("Carlos Alberto", "carlos@gmail.com", "senhaNaoHasheada")]
    [InlineData("João da Silva", "joazinhodasilva@hotmail.com", "senhaNaoHasheada")]
    public void Update_ValidParameters_ShouldUpdateUser(string name, string email, string password)
    {
        var user = new User("Initial", "initial@gmail.com", "novaSenhaNaoHasheada");

        user.Update(name, email, password);

        Assert.NotNull(user);
        Assert.Equal(name, user.Name);
        Assert.Equal(email, user.Email);
        Assert.Equal(password, user.PasswordHash);
    }

    [Fact]
    public void SetUserAdmin_ShouldSetRoleToAdmin()
    {
        var user = new User("Lucas", "lucas@hotmail.com", "senhaNaoHasheada");
        user.SetUserAdmin();

        Assert.Equal(UserRole.Admin, user.Role);
    }

    [Fact]
    public void RemoveAdmin_ShouldSetRoleToUser()
    {
        var user = new User("Lucas", "lucas@hotmail.com", "senhaNaoHasheada");
        user.SetUserAdmin();
        user.RemoveAdmin();

        Assert.Equal(UserRole.User, user.Role);
    }
}
