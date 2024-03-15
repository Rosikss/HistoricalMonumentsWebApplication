using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HistoricalMonumentsWebApplication.Models;

public partial class HistoricalMonumentArchitect : Entity
{
    [Display(Name = "Архітектор")]
    public int ArchitectId { get; set; }
    [Display(Name = "Історична пам'ятка")]
    public int HistoricalMonumentId { get; set; }
    [Display(Name = "Архітектор")]
    
    public virtual Architect? Architect { get; set; }
    [Display(Name = "Історична пам'ятка")]
    
    public virtual HistoricalMonument? HistoricalMonument { get; set; }
}
