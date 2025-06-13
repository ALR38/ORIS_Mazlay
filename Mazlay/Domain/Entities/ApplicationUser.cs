using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

/// <summary>Пользователь приложения с Guid-ключом.</summary>
public class ApplicationUser : IdentityUser<Guid>
{
    public string  FirstName { get; set; } = null!;
    public string  LastName  { get; set; } = null!;
    public string? Address   { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}