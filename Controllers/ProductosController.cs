using Microsoft.AspNetCore.Mvc;

public class ProductosController : Controller
{
  private readonly IProductosRepository _repository;
  private readonly IAuthenticationService _authenticationService;
  public ProductosController(IProductosRepository repository, IAuthenticationService authenticationService)
  {
    _repository = repository;
    _authenticationService = authenticationService;
  }

  private IActionResult CheckAdminPermissions()
  {
    // 1. No logueado? -> vuelve al login
    if (!_authenticationService.EstaAutenticado()) 
      return RedirectToAction("Index", "Login");
    
    // 2. No es Administrador? -> Da Error
    if (!_authenticationService.TieneNivelAcceso("Administrador"))
      // Llamamos a AccesoDenegado (llama a la vista correspondiente de Productos)
      return RedirectToAction("AccesoDenegado");
    
    return null; // Permiso concedido
  }


  // ---------------------------------------- LISTAR ----------------------------------------
  [HttpGet]
  public IActionResult Index()
  {
    var SecurityCheck = CheckAdminPermissions();
    
    return SecurityCheck ?? View(_repository.ObtenerTodosLosProductos());
  }

  [HttpGet]
  public IActionResult Detalle(int id)
  {
    var SecurityCheck = CheckAdminPermissions();
    if (SecurityCheck != null) return SecurityCheck;

    return View(new ProductoViewModel(_repository.ObtenerPorId(id)));
  }

  // ---------------------------------------- CREAR ----------------------------------------
  [HttpGet]
  public IActionResult Crear()
  {
    return CheckAdminPermissions() ?? View();
  }

  [HttpPost]
  public IActionResult Crear(ProductoViewModel p)
  {
    if (!ModelState.IsValid) return View(p);

    return CheckAdminPermissions() ?? View("Detalle", new ProductoViewModel(_repository.CrearProducto(new ProductoDTO(p))));
  }

  // ---------------------------------------- MODIFICAR ----------------------------------------
  [HttpGet]
  public IActionResult Modificar(int id)
  {

    return CheckAdminPermissions() ?? View(new ProductoViewModel(_repository.ObtenerPorId(id)));
  }

  [HttpPost]
  public IActionResult Modificar(ProductoViewModel p)
  {

    if (!ModelState.IsValid) return View(p);

    ProductoDTO dto = new ProductoDTO(p);
    bool modificado = _repository.ModificarProducto(p.IdProducto, dto);

    return CheckAdminPermissions() ?? 
      (modificado
      ? View("Detalle", new ProductoViewModel(_repository.ObtenerPorId(p.IdProducto)))
      : View("Index", _repository.ObtenerTodosLosProductos()));
  }

  // ---------------------------------------- ELIMINAR ----------------------------------------
  [HttpGet]
  public IActionResult Eliminar(int id)
  {
    return CheckAdminPermissions() ?? View(new ProductoViewModel(_repository.ObtenerPorId(id)));
  }

  [HttpPost, ActionName("Eliminar")]
  public IActionResult EliminarProducto(ProductoViewModel p)
  {
    var SecurityCheck = CheckAdminPermissions();
    if (SecurityCheck != null) return SecurityCheck;

    _repository.EliminarPorId(p.IdProducto);
    return RedirectToAction("Index");
  }

  // ---------------------------------------- ACCESO DENEGADO -----------------------------------------
  [HttpGet]
  public IActionResult AccesoDenegado() => View();
}