using System;
using System.Collections.Generic;

namespace HistoricalMonumentsWebApplication.Models;

public partial class HistoricalMonumentArchitect
{
    public int Id { get; set; }

    public int ArchitectId { get; set; }

    public int HistoricalMonumentId { get; set; }

    public virtual Architect Architect { get; set; } = null!;

    public virtual HistoricalMonument HistoricalMonument { get; set; } = null!;
}
