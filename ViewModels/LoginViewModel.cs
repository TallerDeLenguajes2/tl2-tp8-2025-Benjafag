using System.ComponentModel.DataAnnotations;

public class LoginViewModel
{
  public IAuthenticationService _authenticationService;

  public LoginViewModel()
  {
  }

  public LoginViewModel(IAuthenticationService authenticationService)
  {
    _authenticationService = authenticationService;
  }

  [Required(ErrorMessage = "Este campo es obligatorio")]
  [MaxLength(50, ErrorMessage = "El usuario debe tener a lo sumo 50 caracteres")]
  [Display(Name = "Usuario")]
  public string Usuario {get;set;}
  
  [Required(ErrorMessage = "Este campo es obligatorio")]
  [MaxLength(50,ErrorMessage = "La contraseña debe tener a lo sumo 50 caracteres")]
  [Display(Name = "Contraseña")]
  public string Contrasena {get;set;}

  public string ErrorMessage{get;set;}
}