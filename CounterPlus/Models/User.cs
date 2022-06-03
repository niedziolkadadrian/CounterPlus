using Microsoft.AspNetCore.Identity;

namespace CounterPlus.Models;

public class User : IdentityUser
{
    public ICollection<CounterModel>? Counters { get; set; }
}