namespace Users.Domain.Constants;

public static class ExceptionMessageConstants
{
    public const string NameException = "The name must not be empty and must contain at least 3 characters.";
    public const string EmailEmptyException = "The email must not be empty.";
    public const string PasswordEmptyException = "The password must not be empty.";
    public const string EmailAlreadyExistsException = "Email already registered.";
    public const string UserNotExistsException = "User not found.";
    public const string UserUnauthorized = "Unauthorized.";
}
