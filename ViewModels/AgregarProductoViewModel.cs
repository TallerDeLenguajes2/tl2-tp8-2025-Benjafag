using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

public class AgregarProductoViewModel
{
  public AgregarProductoViewModel() { }
  public int IdPresupuesto { get; set; }
  public SelectList ListaProductos { get; set; }

  [Display(Name = "Cantidad")]
  [Required(ErrorMessage = "Este campo es obligatorio")]
  [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que 0")]
  public int Cantidad { get; set; }

  [Display(Name = "Producto")]
  [Required(ErrorMessage = "Este campo es obligatorio")]
  [DeniedValues(0)]
  public int IdProducto { get; set; }
}

