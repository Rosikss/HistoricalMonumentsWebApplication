using System;
using System.Collections.Generic;

namespace HistoricalMonumentsWebApplication.Models;

public partial class Material
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<HistoricalMonumentMaterial> HistoricalMonumentMaterials { get; set; } = new List<HistoricalMonumentMaterial>();
}
