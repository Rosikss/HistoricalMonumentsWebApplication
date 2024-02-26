using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HistoricalMonumentsWebApplication.Models;

public partial class City
{
    public int Id { get; set; }
    [Display(Name ="Назва")]
    [Required(ErrorMessage = "Поле не може бути пустим")]
    public string? Name { get; set; }
    [Display(Name = "Країна")]
    public int CountryId { get; set; }
    [Display(Name = "Країна")]
    public virtual Country? Country { get; set; }

    public virtual ICollection<HistoricalMonument> HistoricalMonuments { get; set; } = new List<HistoricalMonument>();
}
