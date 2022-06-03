namespace CounterPlus.Models;

public class CounterModel
{
    public int? CounterId { get; set; }
    public string? Name { get; set; }
    public int? Count { get; set; }
    public TimeSpan? Timer { get; set; }
    public DateTime? CreatedAt { get; set; }
    
    public IEnumerable<SubCounterModel>? SubCounters { get; set; }
}