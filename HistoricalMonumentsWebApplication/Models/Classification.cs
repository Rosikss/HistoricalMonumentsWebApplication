using System;
using System.Collections.Generic;

namespace HistoricalMonumentsWebApplication.Models;

public partial class Classification
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<HistoricalMonument> HistoricalMonuments { get; set; } = new List<HistoricalMonument>();
}
