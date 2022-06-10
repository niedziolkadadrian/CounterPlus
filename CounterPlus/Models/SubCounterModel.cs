﻿using System.ComponentModel.DataAnnotations;

namespace CounterPlus.Models;

public class SubCounterModel
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public int? Count { get; set; }
    public bool Active { get; set; } = false;
    
    public CounterModel? Counter { get; set; }
}