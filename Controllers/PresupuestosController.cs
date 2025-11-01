using Microsoft.AspNetCore.Mvc;

public class PresupuestosController : Controller
{
  private static readonly PresupuestosRepository repository = new PresupuestosRepository();
  [HttpGet]
  public IActionResult Index()
  {
    List<Presupuesto> presupuestos = repository.ObtenerPresupuestos();
    return View(presupuestos);
  }
  [HttpGet]
  public IActionResult Detalle(int id)
  {
    Presupuesto resultado = repository.ObtenerPresupuestoPorId(id);
    return View(resultado);
  }
}