﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HistoricalMonumentsWebApplication.Models;

public partial class HistoricalMonumentMaterial
{
    public int Id { get; set; }
    [Display(Name = "Історична пам'ятка")]
    public int HistoricalMonumentId { get; set; }
    [Display(Name = "Матеріал")]
    public int MaterialId { get; set; }
    [Display(Name = "Історична пам'ятка")]
    
    public virtual HistoricalMonument? HistoricalMonument { get; set; }
    [Display(Name = "Матеріал")]
    
    public virtual Material? Material { get; set; }
}
