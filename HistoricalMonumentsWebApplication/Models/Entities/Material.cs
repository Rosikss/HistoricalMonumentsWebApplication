using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HistoricalMonumentsWebApplication.Models.Entities;

public partial class Material : Entity
{
    [Display(Name = "Назва")]
    [Required(ErrorMessage = "Поле не може бути пустим")]
    public string? Name { get; set; }

    public virtual ICollection<HistoricalMonumentMaterial> HistoricalMonumentMaterials { get; set; } = new List<HistoricalMonumentMaterial>();
}
