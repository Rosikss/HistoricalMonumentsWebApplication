using System;
using System.Collections.Generic;

namespace HistoricalMonumentsWebApplication.Models;

public partial class Architect
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public int CountryId { get; set; }

    public DateOnly? BirthYear { get; set; }

    public DateOnly? DeathYear { get; set; }

    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<HistoricalMonumentArchitect> HistoricalMonumentArchitects { get; set; } = new List<HistoricalMonumentArchitect>();
}
