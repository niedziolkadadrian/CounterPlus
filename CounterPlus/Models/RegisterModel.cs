using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CounterPlus.Models;

public class RegisterModel
{
    [Required]
    [DisplayName("Nazwa użytownika")]
    public string? UserName { get; set; }
    [Required]
    [EmailAddress]
    [DisplayName("Email")]
    public string? Email { get; set; }
    [Required]
    [DisplayName("Hasło")]
    public string? Password { get; set; }
}