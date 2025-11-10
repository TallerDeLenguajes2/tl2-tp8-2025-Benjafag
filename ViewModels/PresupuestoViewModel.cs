using System.ComponentModel.DataAnnotations;

public class PresupuestoViewModel
{
  public PresupuestoViewModel()  {  }

  public PresupuestoViewModel(Presupuesto p)
  {
    if (p != null)
    {
      IdPresupuesto = p.IdPresupuesto;
      Monto = p.MontoPresupuesto();
      MontoIva = p.MontoPresupuestoIva();
      NombreDestinatario = p.NombreDestinatario;
      FechaCreacion = p.FechaCreacion;
      Detalles = p.Detalles;
    }
  }
  public Presupuesto ToPresupuesto()
  {
    return new Presupuesto
    {
      Detalles = this.Detalles,
      FechaCreacion = this.FechaCreacion,
      IdPresupuesto = this.IdPresupuesto,
      NombreDestinatario = this.NombreDestinatario
    };
  }
  public int IdPresupuesto { get; set; }
  public double Monto { get; set; }
  public List<PresupuestoDetalle> Detalles { get; set; }
  public double MontoIva { get; set; }
  
  [Display(Name = "Nombre del destinatario")]
  [Required(ErrorMessage = "El campo nombre es obligatorio")]
  public string NombreDestinatario { get; set; }

  [Display(Name = "Fecha de creacion")]
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