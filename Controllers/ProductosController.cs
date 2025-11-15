using Microsoft.AspNetCore.Mvc;

public class ProductosController : Controller
{
  private IProductosRepository _repository;
  public ProductosController(IProductosRepository repository)
  {
    _repository = repository;
  }

  // ---------------------------------------- LISTAR ----------------------------------------
  [HttpGet]
  public IActionResult Index() => View(_repository.ObtenerTodosLosProductos());
  
  [HttpGet]
  public IActionResult Detalle(int id) => View(new ProductoViewModel(_repository.ObtenerPorId(id)));

  // ---------------------------------------- CREAR ----------------------------------------
  [HttpGet]
  public IActionResult Crear() => View();

  [HttpPost]
  public IActionResult Crear(ProductoViewModel p) {
    if (!ModelState.IsValid) return View(p);

    return View("Detalle", new ProductoViewModel(_repository.CrearProducto(new ProductoDTO(p))));
  }

  // ---------------------------------------- MODIFICAR ----------------------------------------
  [HttpGet]
  public IActionResult Modificar(int id)
    => View(new ProductoViewModel(_repository.ObtenerPorId(id)));
  
  [HttpPost]
  public IActionResult Modificar(ProductoViewModel p) {
    if (!ModelState.IsValid) return View(p);

    ProductoDTO dto = new ProductoDTO(p);
    bool modificado = _repository.ModificarProducto(p.IdProducto, dto);
    return modificado 
      ? View("Detalle", new ProductoViewModel(_repository.ObtenerPorId(p.IdProducto))) 
      : View("Index", _repository.ObtenerTodosLosProductos());
  }

  // ---------------------------------------- ELIMINAR ----------------------------------------
  [HttpGet]
  public IActionResult Eliminar(int id)
    => View(new ProductoViewModel(_repository.ObtenerPorId(id)));
    
  [HttpPost, ActionName("Eliminar")]
  public IActionResult EliminarProducto(ProductoViewModel p)
  {
    _repository.EliminarPorId(p.IdProducto);
    return RedirectToAction("Index");
  }
}