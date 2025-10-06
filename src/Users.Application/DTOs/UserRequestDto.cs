using System.ComponentModel.DataAnnotations;

namespace Users.Application.DTOs;

public class UserRequestDto
{
    [Required(ErrorMessage = "O campo Nome é de preenchimento obrigatório.")]
    public string Name { get; set; } = default!;

    [Required]
    [EmailAddress(ErrorMessage = "Favor informar um endereço de e-mail válido.")]
    public string Email { get; set; } = default!;

    [MinLength(8, ErrorMessage = "A senha deve ter pelo menos 8 caracteres.")]
    [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[\W_]).+$",
        ErrorMessage = "A senha deve conter ao menos uma letra, um número e um caractere especial.")]
    public string Password { get; set; } = default!;
}
