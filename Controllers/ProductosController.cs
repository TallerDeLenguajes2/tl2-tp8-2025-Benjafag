using Microsoft.AspNetCore.Mvc;

public class ProductosController : Controller
{
  private static readonly ProductosRepository repository = new ProductosRepository();
  public ProductosController()
  {
  }

  public IActionResult Index()
  {
    List<Producto> productos = repository.ObtenerTodosLosProductos();

    return View(productos);
  }

  [HttpGet("/Productos/Detalle/{id}")]
  public IActionResult Detalle(int id)
  {
    Producto producto = repository.ObtenerPorId(id);
    return View(producto);
  }
}