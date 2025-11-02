using Microsoft.AspNetCore.Mvc;

public class ProductosController : Controller
{
  private static readonly ProductosRepository repository = new ProductosRepository();
  public ProductosController() {  }

  // Listar
  [HttpGet]
  public IActionResult Index() => View(repository.ObtenerTodosLosProductos());
  [HttpGet]
  public IActionResult Detalle(int id) =>  View(repository.ObtenerPorId(id));

  // Crear
  [HttpGet]
  public IActionResult Crear() => View();
  [HttpPost]
  public IActionResult CrearProducto(ProductoDTO p) => View("Detalle", repository.CrearProducto(p));

  // Modificar
  [HttpGet]
  public IActionResult Modificar(int id) => View(repository.ObtenerPorId(id));
  [HttpPost]
  public IActionResult ModificarProducto(Producto p)
  {
    ProductoDTO dto = new ProductoDTO { Descripcion = p.Descripcion, Imagen = p.Imagen, Precio = p.Precio };
    bool modificado = repository.ModificarProducto(p.IdProducto, dto);
    return modificado ? View("Detalle", repository.ObtenerPorId(p.IdProducto)) : View("Index", repository.ObtenerTodosLosProductos());
  }

  // Eliminar
  [HttpGet]
  public IActionResult Eliminar(int id) => View();
  public IActionResult EliminarProducto(int id)
  {
    repository.EliminarPorId(id);
    return RedirectToAction("Index");
  }
}