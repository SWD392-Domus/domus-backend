﻿using System.ComponentModel.DataAnnotations;

namespace Domus.Service.Models.Requests.Services;

public class CreateServiceRequest
{
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public double Price { get; set; }
    [Required]
    public string MonetaryUnit { get; set; } = null!;
}