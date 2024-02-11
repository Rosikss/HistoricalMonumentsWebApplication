﻿using System;
using System.Collections.Generic;

namespace HistoricalMonumentsWebApplication.Models;

public partial class Country
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Architect> Architects { get; set; } = new List<Architect>();

    public virtual ICollection<City> Cities { get; set; } = new List<City>();
}
