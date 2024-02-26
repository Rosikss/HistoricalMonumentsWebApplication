using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HistoricalMonumentsWebApplication.Models;

public partial class HistoricalMonument
{
    public int Id { get; set; }
    [Display(Name = "Назва")]
    [Required(ErrorMessage = "Поле не може бути пустим")]
    public string? Name { get; set; }
    [Display(Name = "Початок будівництва")]
    [Required(ErrorMessage = "Поле не може бути пустим")]
    public DateOnly? StartingYear { get; set; }
    [Display(Name = "Кінець будівництва")]
    [Required(ErrorMessage = "Поле не може бути пустим")]
    public DateOnly? EndingYear { get; set; }
    [Display(Name = "Опис")]
    [Required(ErrorMessage = "Поле не може бути пустим")]
    public string? Description { get; set; }
    [Display(Name = "Місто")]
    public int CityId { get; set; }
    [Display(Name = "Категорія")]
    public int ClassificationId { get; set; }
    [Display(Name = "Статус")]
    public int StatusId { get; set; }
    [Display(Name = "Місто")]
    
    public virtual City? City { get; set; }
    [Display(Name = "Категорія")]
    
    public virtual Classification? Classification { get; set; }

    public virtual ICollection<HistoricalMonumentArchitect> HistoricalMonumentArchitects { get; set; } = new List<HistoricalMonumentArchitect>();

    public virtual ICollection<HistoricalMonumentMaterial> HistoricalMonumentMaterials { get; set; } = new List<HistoricalMonumentMaterial>();

    public virtual Status? Status { get; set; }
}
