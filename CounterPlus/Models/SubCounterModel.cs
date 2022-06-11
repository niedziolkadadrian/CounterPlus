using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CounterPlus.Models;

public class SubCounterModel
{
    public int? Id { get; set; }
    [DisplayName("Nazwa")]
    public string? Name { get; set; }
    [DisplayName("Licznik")]
    public int? Count { get; set; }
    [DisplayName("Aktywny")]
    public bool Active { get; set; } = false;
    
    public CounterModel? Counter { get; set; }
}