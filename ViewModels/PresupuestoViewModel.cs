using System.ComponentModel.DataAnnotations;

public class PresupuestoViewModel
{
  [Display(Description = "Nombre del destinatario")]
  [Required(ErrorMessage = "El campo nombre es obligatorio")]
  public string NombreDestinatario { get; set; }

  [Display(Description = "Email (Opcional)")]
  [EmailAddress(ErrorMessage = "El campo debe tener un formato de email")]
  public string Email { get; set; }

  [Display(Description = "Fecha de creacion")]
  [Required(ErrorMessage = "El campo fecha de creacion es obligatorio")]
  [FechaMenor]
  public DateTime FechaCreacion { get; set; }
}

public class FechaMenorAttribute : ValidationAttribute
{
  protected override ValidationResult IsValid(object value, ValidationContext context)
  {
    DateTime fecha = Convert.ToDateTime(value);

    if (fecha.Date > DateTime.Today)
      return new ValidationResult(ErrorMessage ?? "La fecha debe ser menor o igual a hoy"); // el errormesage se define afuera

    return ValidationResult.Success;
  }
}