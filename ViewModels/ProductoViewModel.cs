using System.ComponentModel.DataAnnotations;

public class ProductoViewModel
{
  public ProductoViewModel() { }

  public ProductoViewModel(Producto p)
  {
    IdProducto = p.IdProducto;
    Descripcion = p.Descripcion ?? "";
    Precio = p.Precio;
    UrlImagen = p.Imagen ?? "";
  }

  public int IdProducto { get; set; }

  [Display(Name = "Descripcion")]
  [StringLength(250, ErrorMessage = "La descripcion debe ser a lo sumo de 250 caracteres")]
  public string Descripcion { get; set; }

  [Display(Name = "Precio")]
  [Required(ErrorMessage = "El campo precio es obligatorio")]
  [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser mayor que 0")]
  public double Precio { get; set; }

  [Display(Name = "Link a imagen (opcional)")]
  [ImageUrl]
  public string UrlImagen { get; set; }

  
}

public class ImageUrlAttribute : ValidationAttribute
{
  private static readonly string[] extensiones =
    [".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp"];

  protected override ValidationResult IsValid(object value, ValidationContext validationContext)
  {
    if (value == null) return ValidationResult.Success; // opcional si es nullable

    string url = value.ToString();

    if (!Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult))
      return new ValidationResult(ErrorMessage ?? "URL inválida.");

    if (!extensiones.Contains(Path.GetExtension(uriResult.AbsolutePath).ToLower()))
      return new ValidationResult(ErrorMessage ?? "Debe ser una imagen.");

    return ValidationResult.Success;
  }
}