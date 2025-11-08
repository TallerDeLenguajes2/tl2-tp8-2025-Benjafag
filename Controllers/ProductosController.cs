using Microsoft.AspNetCore.Mvc;

public class ProductosController : Controller
{
  private static readonly ProductosRepository repository = new ProductosRepository();
  public ProductosController() { }

  // Listar
  [HttpGet]
  public IActionResult Index() => View(repository.ObtenerTodosLosProductos());
  [HttpGet]
  public IActionResult Detalle(int id) => View(new ProductoViewModel(repository.ObtenerPorId(id)));

  // Crear
  [HttpGet]
  public IActionResult Crear() => View();
  [HttpPost]
  public IActionResult Crear(ProductoViewModel p)
    => View("Detalle", new ProductoViewModel(repository.CrearProducto(new ProductoDTO(p))));

  // Modificar
  [HttpGet]
  public IActionResult Modificar(int id)
    => View(new ProductoViewModel(repository.ObtenerPorId(id)));
  
  [HttpPost]
  public IActionResult Modificar(ProductoViewModel p)
  {
    ProductoDTO dto = new ProductoDTO(p);
    bool modificado = repository.ModificarProducto(p.IdProducto, dto);
    return modificado 
      ? View("Detalle", new ProductoViewModel(repository.ObtenerPorId(p.IdProducto))) 
      : View("Index", repository.ObtenerTodosLosProductos());
  }

  // Eliminar
  [HttpGet]
  public IActionResult Eliminar(int id) 
    => View(new ProductoViewModel(repository.ObtenerPorId(id)));

  [HttpPost, ActionName("Eliminar")]
  public IActionResult EliminarProducto(ProductoViewModel p)
  {
    repository.EliminarPorId(p.IdProducto);
    return RedirectToAction("Index");
  }
}