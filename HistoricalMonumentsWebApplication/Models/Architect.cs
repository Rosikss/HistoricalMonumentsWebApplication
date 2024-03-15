using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HistoricalMonumentsWebApplication.Models;

public partial class Architect : Entity
{
    
    
    
    [Display(Name ="Ім'я")]
    [Required(ErrorMessage = "Поле не може бути пустим")]
    public string? FirstName { get; set; }
    [Display(Name = "Прізвище")]
    [Required(ErrorMessage = "Поле не може бути пустим")]
    public string? LastName { get; set; }
    [Display(Name = "Країна")]
    public int CountryId { get; set; }
    [Display(Name = "Рік народження")]
    [Required(ErrorMessage = "Поле не може бути пустим")]
    public DateOnly? BirthYear { get; set; }
    [Display(Name = "Рік смерті")]
    [Required(ErrorMessage ="Поле не може бути пустим")]
    public DateOnly? DeathYear { get; set; }
    [Display(Name = "Країна")]
    
    public virtual Country? Country { get; set; } 

    public virtual ICollection<HistoricalMonumentArchitect> HistoricalMonumentArchitects { get; set; } = new List<HistoricalMonumentArchitect>();
}
