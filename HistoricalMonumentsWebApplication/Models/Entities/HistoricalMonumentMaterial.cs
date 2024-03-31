using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HistoricalMonumentsWebApplication.Models.Entities;

public partial class HistoricalMonumentMaterial : Entity
{
    [Display(Name = "Історична пам'ятка")]
    public int HistoricalMonumentId { get; set; }
    [Display(Name = "Матеріал")]
    public int MaterialId { get; set; }
    [Display(Name = "Історична пам'ятка")]

    public virtual HistoricalMonument? HistoricalMonument { get; set; }
    [Display(Name = "Матеріал")]

    public virtual Material? Material { get; set; }
}
