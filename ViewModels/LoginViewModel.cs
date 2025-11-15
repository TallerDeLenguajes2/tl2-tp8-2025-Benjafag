using System.ComponentModel.DataAnnotations;

public class LoginViewModel
{
  [Required(ErrorMessage = "Este campo es obligatorio")]
  [MaxLength(50, ErrorMessage = "El usuario debe tener a lo sumo 50 caracteres")]
  [Display(Name = "Usuario")]
  public string Usuario {get;set;}
  
  [Required(ErrorMessage = "Este campo es obligatorio")]
  [MaxLength(50,ErrorMessage = "La contraseña debe tener a lo sumo 50 caracteres")]
  [Display(Name = "Contraseña")]
  public string Contrasena {get;set;}
}