using System.ComponentModel.DataAnnotations;

namespace CounterPlus.Models;

public class SignInModel
{
    [Required]
    public string? UserName { get; init; }
    [Required]
    public string? Password { get; set; }
}