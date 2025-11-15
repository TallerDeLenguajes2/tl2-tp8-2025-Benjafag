using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

public class PresupuestosController : Controller
{
  private IPresupuestosRepository _repository;
  private IProductosRepository _repositoryProductos ;
  private readonly IAuthenticationService _authenticationService;

  public PresupuestosController(IPresupuestosRepository presupuestos, IProductosRepository productos, IAuthenticationService authenticationService)
  {
    _repository = presupuestos;
    _repositoryProductos = productos;
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
    if (!_authenticationService.EstaAutenticado())
      return RedirectToAction("Index","Login");

    if (!(_authenticationService.TieneNivelAcceso("Administrador") || _authenticationService.TieneNivelAcceso("Cliente")))
      return View("AccesoDenegado");
    

    var presupuestos = _repository.ObtenerPresupuestos().Select(p => new PresupuestoViewModel(p)).ToList();
    return View(presupuestos);
  }

  [HttpGet]
  public IActionResult Detalle(int id)
  {
    if (!_authenticationService.EstaAutenticado())
      return RedirectToAction("Index","Login");

    if (!(_authenticationService.TieneNivelAcceso("Administrador") || _authenticationService.TieneNivelAcceso("Cliente")))
      return View("AccesoDenegado");

    Presupuesto p = _repository.ObtenerPresupuestoPorId(id);
    return p != null ? View(new PresupuestoViewModel(p)) : View(null);
  }

  // ---------------------------------------- CREAR ----------------------------------------
  [HttpGet]
  public IActionResult Crear()
  {
    var SecurityCheck = CheckAdminPermissions();
    if (SecurityCheck != null) return SecurityCheck;

    return View();
  } 
  
  [HttpPost, ActionName("Crear")]
  public IActionResult CrearPresupuesto(PresupuestoViewModel p)
  {
    var SecurityCheck = CheckAdminPermissions();
    if (SecurityCheck != null) return SecurityCheck;

    _repository.CrearPresupuesto(p.ToPresupuesto());
    Presupuesto insertado = _repository.UltimoInsertado();
    return View("Detalle", new PresupuestoViewModel(insertado)); // importante!
  }

  // ---------------------------------------- MODIFICAR ----------------------------------------
  [HttpGet]
  public IActionResult Modificar(int id) 
  {
    var SecurityCheck = CheckAdminPermissions();
    if (SecurityCheck != null) return SecurityCheck;
    return  View(new PresupuestoViewModel(_repository.ObtenerPresupuestoPorId(id)));
    
  }
  
  [HttpPost]
  public IActionResult Modificar(PresupuestoViewModel p)
  {
    var SecurityCheck = CheckAdminPermissions();
    if (SecurityCheck != null) return SecurityCheck;

    p.Detalles = p.Detalles.FindAll(d => d.Cantidad != 0);
    _repository.ModificarPresupuesto(p.ToPresupuesto());
    return RedirectToAction("Index"); 
  }

  // ---------------------------------------- ELIMINAR ----------------------------------------
  [HttpGet]
  public IActionResult Eliminar(int id) {
    var SecurityCheck = CheckAdminPermissions();
    if (SecurityCheck != null) return SecurityCheck;

    Presupuesto p = _repository.ObtenerPresupuestoPorId(id);
    return p != null ? View(new PresupuestoViewModel(p)) : View(null);
  }

  [HttpPost]
  public IActionResult Eliminar(PresupuestoViewModel p)
  {
    var SecurityCheck = CheckAdminPermissions();
    if (SecurityCheck != null) return SecurityCheck;

    _repository.EliminarPresupuesto(p.IdPresupuesto);
    return RedirectToAction("Index");
  }

  // ---------------------------------------- AGREGAR PRODUCTO ----------------------------------------
  [HttpGet]
  public IActionResult AgregarProducto(int id)
  {
    var SecurityCheck = CheckAdminPermissions();
    if (SecurityCheck != null) return SecurityCheck;

    List<Producto> productos = _repositoryProductos.ObtenerTodosLosProductos();
    AgregarProductoViewModel viewModel = new AgregarProductoViewModel
    {
      IdPresupuesto = id,
      ListaProductos = new SelectList(productos, "IdProducto", "Descripcion"),
      Cantidad = 0
    };
    return View(viewModel);

  }
  [HttpPost]
  public IActionResult AgregarProducto(AgregarProductoViewModel vm)
  {
    var SecurityCheck = CheckAdminPermissions();
    if (SecurityCheck != null) return SecurityCheck;

    if (!ModelState.IsValid)
      return RedirectToAction("AgregarProducto", new { id = vm.IdPresupuesto });
    
    _repository.AgregarProducto(vm.IdPresupuesto, vm.IdProducto, vm.Cantidad);
    return View("Detalle", new PresupuestoViewModel(_repository.ObtenerPresupuestoPorId(vm.IdPresupuesto)));
  }

  // ---------------------------------------- ELIMINAR PRODUCTO ----------------------------------------
  [HttpPost]
  public IActionResult EliminarProducto(int IdProducto, int IdPresupuesto)
  {
    var SecurityCheck = CheckAdminPermissions();
    if (SecurityCheck != null) return SecurityCheck;

    _repository.EliminarProducto(IdPresupuesto, IdProducto);
    return View("Detalle", new PresupuestoViewModel(_repository.ObtenerPresupuestoPorId(IdPresupuesto)));
  }

  // ---------------------------------------- ACCESO DENEGADO -----------------------------------------
  [HttpGet]
  public IActionResult AccesoDenegado() => View();
}