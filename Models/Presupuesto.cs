public class Presupuesto
{
  private static readonly double IVA = 0.21;
  public int IdPresupuesto { get; set;}
  public string NombreDestinatario { get; set;}
  public DateTime FechaCreacion { get; set;}
  public List<PresupuestoDetalle> Detalles { get; set; }
  public double MontoPresupuesto()
    => Detalles.Sum(d => d.Producto.Precio * d.Cantidad);
  public double MontoPresupuestoIva() => MontoPresupuesto() * (1 + IVA);
  public int CantidadProductos() => Detalles.Sum(d => d.Cantidad);

}