using Microsoft.AspNetCore.Mvc;

public class PresupuestosController : Controller
{
  private static readonly PresupuestosRepository repository = new PresupuestosRepository();
  // Listar
  [HttpGet]
  public IActionResult Index() => View(repository.ObtenerPresupuestos());

  [HttpGet]
  public IActionResult Detalle(int id) => View(repository.ObtenerPresupuestoPorId(id));
  
  // Crear
  [HttpGet]
  public IActionResult Crear() => View();
  [HttpPost]
  public IActionResult CrearPresupuesto(Presupuesto p)
  {
    p.Detalles = p.Detalles.FindAll(d => d.Cantidad != 0);
    repository.CrearPresupuesto(p);
    Presupuesto insertado = repository.UltimoInsertado();
    return RedirectToAction("Detalle", new { id = insertado.IdPresupuesto }); // importante!
  }
}