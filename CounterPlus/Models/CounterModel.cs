using System.ComponentModel.DataAnnotations;

namespace CounterPlus.Models;

public class CounterModel
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public int? Count { get; set; }
    public DateTime? CreatedAt { get; set; }

    public ICollection<SubCounterModel> SubCounters { get; set; } = new List<SubCounterModel>();
    
    public User? User { get; set; }
}