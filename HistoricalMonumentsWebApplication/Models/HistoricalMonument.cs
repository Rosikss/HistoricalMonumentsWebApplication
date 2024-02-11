using System;
using System.Collections.Generic;

namespace HistoricalMonumentsWebApplication.Models;

public partial class HistoricalMonument
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateOnly? StartingYear { get; set; }

    public DateOnly? EndingYear { get; set; }

    public string? Description { get; set; }

    public int CityId { get; set; }

    public int ClassificationId { get; set; }

    public int StatusId { get; set; }

    public virtual City City { get; set; } = null!;

    public virtual Classification Classification { get; set; } = null!;

    public virtual ICollection<HistoricalMonumentArchitect> HistoricalMonumentArchitects { get; set; } = new List<HistoricalMonumentArchitect>();

    public virtual ICollection<HistoricalMonumentMaterial> HistoricalMonumentMaterials { get; set; } = new List<HistoricalMonumentMaterial>();

    public virtual Status Status { get; set; } = null!;
}
