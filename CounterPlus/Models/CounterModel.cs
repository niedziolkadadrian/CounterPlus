using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CounterPlus.Models;

public class CounterModel
{
    public int? Id { get; set; }
    [DisplayName("Nazwa")]
    public string? Name { get; set; }
    [DisplayName("Licznik")]
    public int? Count { get; set; }
    [DisplayName("Data utworzenia")]
    public DateTime? CreatedAt { get; set; }

    public ICollection<SubCounterModel> SubCounters { get; set; } = new List<SubCounterModel>();
    
    public User? User { get; set; }
}