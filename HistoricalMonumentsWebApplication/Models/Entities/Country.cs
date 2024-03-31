using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HistoricalMonumentsWebApplication.Models.Entities;

public partial class Country : Entity
{
    [Display(Name = "Назва")]
    [Required(ErrorMessage = "Поле не може бути пустим")]
    public string? Name { get; set; }

    public virtual ICollection<Architect> Architects { get; set; } = new List<Architect>();

    public virtual ICollection<City> Cities { get; set; } = new List<City>();
}
