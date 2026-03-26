using Suelos.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace Suelos.Shared.DTOs;

public class UserDTO
{
    [Display(Name = "Documento")]
    [MaxLength(20, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres.")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string Document { get; set; } = null!;

    [Display(Name = "Nombres")]
    [MaxLength(50, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres.")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string FirstName { get; set; } = null!;

    [Display(Name = "Apellidos")]
    [MaxLength(50, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres.")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string LastName { get; set; } = null!;

    [Display(Name = "Direccion")]
    [MaxLength(200, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres.")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string Address { get; set; } = null!;

    [Display(Name = "Tipo de usuario")]
    public UserType UserType { get; set; } = UserType.Analyst;

    [Display(Name = "Correo")]
    [EmailAddress(ErrorMessage = "Debes ingresar un correo valido.")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string Email { get; set; } = null!;

    [Display(Name = "Contrasena")]
    [MinLength(6, ErrorMessage = "El campo {0} debe tener al menos {1} caracteres.")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string Password { get; set; } = null!;

    [Compare(nameof(Password), ErrorMessage = "La contrasena y la confirmacion no son iguales.")]
    [Display(Name = "Confirmacion de contrasena")]
    [MinLength(6, ErrorMessage = "El campo {0} debe tener al menos {1} caracteres.")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string PasswordConfirm { get; set; } = null!;
}
