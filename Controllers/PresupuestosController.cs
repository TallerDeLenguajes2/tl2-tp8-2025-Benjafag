using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

public class PresupuestosController : Controller
{
  private static readonly PresupuestosRepository repository = new PresupuestosRepository();
  private static readonly ProductosRepository repositoryProductos = new ProductosRepository();


  // ---------------------------------------- LISTAR ----------------------------------------
  [HttpGet]
  public IActionResult Index()
  {
    var presupuestos = repository.ObtenerPresupuestos().Select(p => new PresupuestoViewModel(p)).ToList();
    return View(presupuestos);
  }

  [HttpGet]
  public IActionResult Detalle(int id)
  {
    Presupuesto p = repository.ObtenerPresupuestoPorId(id);
    return p != null ? View(new PresupuestoViewModel(p)) : View(null);
  }

  // ---------------------------------------- CREAR ----------------------------------------
  [HttpGet]
  public IActionResult Crear() => View();
  
  [HttpPost, ActionName("Crear")]
  public IActionResult CrearPresupuesto(PresupuestoViewModel p)
  {
    Console.WriteLine(new { Fecha= p.FechaCreacion, Nombre = p.NombreDestinatario });
    repository.CrearPresupuesto(p.ToPresupuesto());
    Presupuesto insertado = repository.UltimoInsertado();
    return View("Detalle", new PresupuestoViewModel(insertado)); // importante!
  }

  // ---------------------------------------- MODIFICAR ----------------------------------------
  [HttpGet]
  public IActionResult Modificar(int id) 
    => View(new PresupuestoViewModel(repository.ObtenerPresupuestoPorId(id)));
  
  [HttpPost]
  public IActionResult Modificar(PresupuestoViewModel p)
  {
    p.Detalles = p.Detalles.FindAll(d => d.Cantidad != 0);
    repository.ModificarPresupuesto(p.ToPresupuesto());
    return RedirectToAction("Index"); 
  }

  // ---------------------------------------- ELIMINAR ----------------------------------------
  [HttpGet]
  public IActionResult Eliminar(int id) {
    Presupuesto p = repository.ObtenerPresupuestoPorId(id);
    return p != null ? View(new PresupuestoViewModel(p)) : View(null);
  }

  [HttpPost]
  public IActionResult Eliminar(PresupuestoViewModel p)
  {
    System.Console.WriteLine(new {id = p.IdPresupuesto});
    repository.EliminarPresupuesto(p.IdPresupuesto);
    return RedirectToAction("Index");
  }

  // ---------------------------------------- AGREGAR PRODUCTO ----------------------------------------
  [HttpGet]
  public IActionResult AgregarProducto(int id)
  {
    List<Producto> productos = repositoryProductos.ObtenerTodosLosProductos();
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
    
    repository.AgregarProducto(vm.IdPresupuesto, vm.IdProducto, vm.Cantidad);
    return View("Detalle", new PresupuestoViewModel(repository.ObtenerPresupuestoPorId(vm.IdPresupuesto)));
  }
  // ---------------------------------------- ELIMINAR PRODUCTO ----------------------------------------
  [HttpPost]
  public IActionResult EliminarProducto(int IdProducto, int IdPresupuesto)
  {
    System.Console.WriteLine("ENTRE");
    repository.EliminarProducto(IdPresupuesto, IdProducto);
    return View("Detalle", new PresupuestoViewModel(repository.ObtenerPresupuestoPorId(IdPresupuesto)));
  }
}