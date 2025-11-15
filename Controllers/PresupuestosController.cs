using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

public class PresupuestosController : Controller
{
  private IPresupuestosRepository _repository;
  private IProductosRepository _repositoryProductos ;

  public PresupuestosController(IPresupuestosRepository presupuestos, IProductosRepository productos)
  {
    _repository = presupuestos;
    _repositoryProductos = productos;
  }


  // ---------------------------------------- LISTAR ----------------------------------------
  [HttpGet]
  public IActionResult Index()
  {
    var presupuestos = _repository.ObtenerPresupuestos().Select(p => new PresupuestoViewModel(p)).ToList();
    return View(presupuestos);
  }

  [HttpGet]
  public IActionResult Detalle(int id)
  {
    Presupuesto p = _repository.ObtenerPresupuestoPorId(id);
    return p != null ? View(new PresupuestoViewModel(p)) : View(null);
  }

  // ---------------------------------------- CREAR ----------------------------------------
  [HttpGet]
  public IActionResult Crear() => View();
  
  [HttpPost, ActionName("Crear")]
  public IActionResult CrearPresupuesto(PresupuestoViewModel p)
  {
    Console.WriteLine(new { Fecha= p.FechaCreacion, Nombre = p.NombreDestinatario });
    _repository.CrearPresupuesto(p.ToPresupuesto());
    Presupuesto insertado = _repository.UltimoInsertado();
    return View("Detalle", new PresupuestoViewModel(insertado)); // importante!
  }

  // ---------------------------------------- MODIFICAR ----------------------------------------
  [HttpGet]
  public IActionResult Modificar(int id) 
    => View(new PresupuestoViewModel(_repository.ObtenerPresupuestoPorId(id)));
  
  [HttpPost]
  public IActionResult Modificar(PresupuestoViewModel p)
  {
    p.Detalles = p.Detalles.FindAll(d => d.Cantidad != 0);
    _repository.ModificarPresupuesto(p.ToPresupuesto());
    return RedirectToAction("Index"); 
  }

  // ---------------------------------------- ELIMINAR ----------------------------------------
  [HttpGet]
  public IActionResult Eliminar(int id) {
    Presupuesto p = _repository.ObtenerPresupuestoPorId(id);
    return p != null ? View(new PresupuestoViewModel(p)) : View(null);
  }

  [HttpPost]
  public IActionResult Eliminar(PresupuestoViewModel p)
  {
    System.Console.WriteLine(new {id = p.IdPresupuesto});
    _repository.EliminarPresupuesto(p.IdPresupuesto);
    return RedirectToAction("Index");
  }

  // ---------------------------------------- AGREGAR PRODUCTO ----------------------------------------
  [HttpGet]
  public IActionResult AgregarProducto(int id)
  {
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
    if (!ModelState.IsValid)
      return RedirectToAction("AgregarProducto", new { id = vm.IdPresupuesto });
    
    _repository.AgregarProducto(vm.IdPresupuesto, vm.IdProducto, vm.Cantidad);
    return View("Detalle", new PresupuestoViewModel(_repository.ObtenerPresupuestoPorId(vm.IdPresupuesto)));
  }
  // ---------------------------------------- ELIMINAR PRODUCTO ----------------------------------------
  [HttpPost]
  public IActionResult EliminarProducto(int IdProducto, int IdPresupuesto)
  {
    _repository.EliminarProducto(IdPresupuesto, IdProducto);
    return View("Detalle", new PresupuestoViewModel(_repository.ObtenerPresupuestoPorId(IdPresupuesto)));
  }
}