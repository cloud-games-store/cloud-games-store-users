namespace Users.Application.DTOs;

public record UserDto
{
    public Guid? Id { get; private set; }
    public string? Name { get; private set; }
    public string? Email { get; private set; }

    public UserDto(Guid? id, string? name, string? email)
    {
        Id = id;
        Name = name;
        Email = email;
    }
}
