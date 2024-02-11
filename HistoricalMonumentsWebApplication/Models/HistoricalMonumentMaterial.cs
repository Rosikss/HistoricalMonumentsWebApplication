using System;
using System.Collections.Generic;

namespace HistoricalMonumentsWebApplication.Models;

public partial class HistoricalMonumentMaterial
{
    public int Id { get; set; }

    public int HistoricalMonumentId { get; set; }

    public int MaterialId { get; set; }

    public virtual HistoricalMonument HistoricalMonument { get; set; } = null!;

    public virtual Material Material { get; set; } = null!;
}
