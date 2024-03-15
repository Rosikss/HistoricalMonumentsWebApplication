using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HistoricalMonumentsWebApplication.Models;

public partial class Status : Entity
{
    [Display(Name = "Стан")]
    [Required(ErrorMessage = "Поле не може бути пустим")]
    public string? Name { get; set; }

    public virtual ICollection<HistoricalMonument> HistoricalMonuments { get; set; } = new List<HistoricalMonument>();
}
