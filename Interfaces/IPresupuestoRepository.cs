public interface IPresupuestosRepository
{
  public bool CrearPresupuesto(Presupuesto presupuesto);
  public List<Presupuesto> ObtenerPresupuestos();
  public Presupuesto ObtenerPresupuestoPorId(int id);
  public bool AgregarProducto(int idPresupuesto, int idProducto, int cantidad);
  public bool EliminarPresupuesto(int id);
  public Presupuesto UltimoInsertado();
  public List<PresupuestoDetalle> ObtenerDetalles(int id);
  public bool ModificarPresupuesto(Presupuesto presupuesto);
  public bool ModificarProducto(int idPresupuesto, int idProducto, int cantidad);

  public bool EliminarProducto(int idPresupuesto, int idProducto);
}