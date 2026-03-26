using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Suelos.Shared.Enums;

namespace Suelos.Shared.Entities;

public class User : IdentityUser
{
    [Display(Name = "Documento")]
    [MaxLength(20)]
    [Required]
    public string Document { get; set; } = null!;

    [Display(Name = "Nombres")]
    [MaxLength(50)]
    [Required]
    public string FirstName { get; set; } = null!;

    [Display(Name = "Apellidos")]
    [MaxLength(50)]
    [Required]
    public string LastName { get; set; } = null!;

    [Display(Name = "Direccion")]
    [MaxLength(200)]
    [Required]
    public string Address { get; set; } = null!;

    [Display(Name = "Tipo de usuario")]
    public UserType UserType { get; set; } = UserType.Admin;

    public string FullName => $"{FirstName} {LastName}";
}
