
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.Sqlite;


public class ProductoDTO
{
  public string Descripcion { get; set; }
  public double Precio { get; set; }
}

public class ProductosRepository : IProductosRepository
{
  static readonly string connectionString = "Data Source = tienda.db;";
  public Producto UltimoInsertado()
  {
    SqliteConnection connection = new SqliteConnection(connectionString);
    connection.Open();
    string selectQuery = "SELECT * FROM Producto ORDER BY IdProducto DESC LIMIT 1";

    using SqliteCommand selectCmd = new SqliteCommand(selectQuery, connection);
    using SqliteDataReader reader = selectCmd.ExecuteReader();
    if (reader.Read())
      return new Producto
      {
        IdProducto = Convert.ToInt32(reader["IdProducto"]),
        Descripcion = Convert.ToString(reader["Descripcion"]),
        Precio = Convert.ToDouble(reader["Precio"])
      };
    connection.Close();
    return null;
  }
  public Producto CrearProducto(ProductoDTO producto)
  {
    SqliteConnection connection = new SqliteConnection(connectionString);
    connection.Open();

    string insertQuery = "INSERT INTO producto (Descripcion, Precio) VALUES (@descripcion, @precio)";

    using var insertCmd = new SqliteCommand(insertQuery, connection);
    insertCmd.Parameters.AddWithValue("@descripcion", producto.Descripcion);
    insertCmd.Parameters.AddWithValue("@precio", producto.Precio);
    int cantidad = insertCmd.ExecuteNonQuery();
    Console.WriteLine(cantidad);
    if (cantidad != 0) Console.WriteLine($"Producto {producto.Descripcion} creado exitosamente");
    connection.Close();

    return cantidad == 0 ? null : UltimoInsertado();
  }

  public Producto EliminarPorId(int id)
  {
    SqliteConnection connection = new SqliteConnection(connectionString);
    connection.Open();
    string deleteQuery = "DELETE FROM producto WHERE IdProducto = @id";

    using SqliteCommand deleteCmd = new SqliteCommand(deleteQuery, connection);
    deleteCmd.Parameters.AddWithValue("@id", id);
    Producto eliminado = ObtenerPorId(id);
    int nroEliminados = deleteCmd.ExecuteNonQuery();
    return nroEliminados == 0 ? null : eliminado;

  }

  public bool ModificarProducto(int id, ProductoDTO producto)
  {
    SqliteConnection connection = new SqliteConnection(connectionString);
    connection.Open();
    string deleteQuery = "UPDATE producto SET Descripcion = @descripcion, Precio = @precio WHERE IdProducto = @id";

    using SqliteCommand deleteCmd = new SqliteCommand(deleteQuery, connection);
    deleteCmd.Parameters.AddWithValue("@descripcion", producto.Descripcion);
    deleteCmd.Parameters.AddWithValue("@precio", producto.Precio);
    deleteCmd.Parameters.AddWithValue("@id", id);

    int afectados = deleteCmd.ExecuteNonQuery();
    return afectados != 0;
  }

  public Producto ObtenerPorId(int id)
  {
    SqliteConnection connection = new SqliteConnection(connectionString);
    connection.Open();
    string selectQuery = "SELECT * FROM producto WHERE IdProducto = @id";
    using var selectCmd = new SqliteCommand(selectQuery, connection);
    selectCmd.Parameters.AddWithValue("@id", id);
    using SqliteDataReader reader = selectCmd.ExecuteReader();
    if (reader.Read())
    {
      Producto resultado = new Producto
      {
        IdProducto = Convert.ToInt32(reader["IdProducto"]),
        Descripcion = Convert.ToString(reader["Descripcion"]),
        Precio = Convert.ToDouble(reader["Precio"]),
        Imagen = Convert.ToString(reader["Imagen"])
      };
      connection.Close();
      return resultado;
    }
    connection.Close();
    return null;
  }

  public List<Producto> ObtenerTodosLosProductos()
  {
    SqliteConnection connection = new SqliteConnection(connectionString);
    connection.Open();
    string selectQuery = "SELECT * FROM Producto";

    using SqliteCommand selectCmd = new SqliteCommand(selectQuery, connection);
    List<Producto> productos = new List<Producto>();

    using SqliteDataReader reader = selectCmd.ExecuteReader();
    while (reader.Read())
      productos.Add(new Producto
      {
        IdProducto = Convert.ToInt32(reader["IdProducto"]),
        Descripcion = Convert.ToString(reader["Descripcion"]),
        Precio = Convert.ToDouble(reader["Precio"])
      });

    connection.Close();
    return productos;
  }
}