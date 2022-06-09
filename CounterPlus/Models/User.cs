using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CounterPlus.Models;

public class User : IdentityUser
{
    public ICollection<CounterModel> Counters { get; set; } = new List<CounterModel>();
}