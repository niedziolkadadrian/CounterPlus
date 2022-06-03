using CounterPlus.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CounterPlus;

public class CPdbContext: IdentityDbContext<User>
{
    public CPdbContext()
    {
    }

    public CPdbContext(DbContextOptions<CPdbContext> options) : base(options)
    {
    }
    
    public DbSet<CounterModel>? Counters { get; set; }
    public DbSet<SubCounterModel>? SubCounters { get; set; }
}