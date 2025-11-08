using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

public class AgregarProductoViewModel
{
  public SelectList ListaProductos { get; set; }

  [Required(ErrorMessage = "El campo cantidad es obligatorio")]
  [Range(1,int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que 0")]
  public int Cantidad { get; set; }
}