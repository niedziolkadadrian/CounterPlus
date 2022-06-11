using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CounterPlus.Models;

public class SignInModel
{
    [Required]
    [DisplayName("Nazwa użytkownika")]
    public string? UserName { get; init; }
    [Required]
    [DisplayName("Hasło")]
    public string? Password { get; set; }
}