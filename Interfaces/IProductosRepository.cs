public interface IProductosRepository
{
  Producto CrearProducto(ProductoDTO producto);
  bool ModificarProducto(int id, ProductoDTO producto);
  List<Producto> ObtenerTodosLosProductos();
  Producto ObtenerPorId(int id);
  Producto EliminarPorId(int id);
  public Producto UltimoInsertado();

}