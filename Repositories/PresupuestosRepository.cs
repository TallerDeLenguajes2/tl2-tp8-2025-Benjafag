
using Microsoft.Data.Sqlite;

public class PresupuestosRepository : IPresupuestosRepository
{
  private static readonly string connectionString = "Data Source = tienda.db;";
  public Presupuesto UltimoInsertado()
  {
    SqliteConnection connection = new SqliteConnection(connectionString);
    connection.Open();
    string selectQuery = "SELECT * FROM Presupuesto ORDER BY IdPresupuesto DESC LIMIT 1";

    using SqliteCommand selectCmd = new SqliteCommand(selectQuery, connection);
    using SqliteDataReader reader = selectCmd.ExecuteReader();
    if (reader.Read())
      return new Presupuesto
      {
        IdPresupuesto = Convert.ToInt32(reader["IdPresupuesto"]),
        NombreDestinatario = Convert.ToString(reader["NombreDestinatario"]),
        FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]),
        Detalles = ObtenerDetalles(Convert.ToInt32(reader["IdPresupuesto"]))
      };
    connection.Close();
    return null;
  }

  public List<PresupuestoDetalle> ObtenerDetalles(int id)
  {
    ProductosRepository repositorioProductos = new ProductosRepository(); // nos sirve para sacar el producto para cada detalle
    SqliteConnection connection = new SqliteConnection(connectionString);
    connection.Open();

    string selectQuery = "SELECT * FROM PresupuestoDetalle WHERE IdPresupuesto = @id";
    using SqliteCommand selectCmd = new SqliteCommand(selectQuery, connection);
    selectCmd.Parameters.AddWithValue("@id", id);

    List<PresupuestoDetalle> resultado = [];

    using SqliteDataReader reader = selectCmd.ExecuteReader();
    while (reader.Read())
    {
      int idProducto = Convert.ToInt32(reader["IdProducto"]);
      int cantidad = Convert.ToInt32(reader["Cantidad"]);

      resultado.Add(new PresupuestoDetalle
      {
        Producto = repositorioProductos.ObtenerPorId(idProducto),
        Cantidad = cantidad
      });
    }

    connection.Close();
    return resultado;
  }
  public bool AgregarProducto(int idPresupuesto, int idProducto, int cantidad)
  {
    try {
      SqliteConnection connection = new SqliteConnection(connectionString);
      connection.Open();

      

      string insertQuery = "INSERT INTO PresupuestoDetalle (IdPresupuesto,IdProducto,Cantidad) VALUES (@idpres,@idprod,@cantidad)";
      using SqliteCommand insertCmd = new SqliteCommand(insertQuery, connection);
      insertCmd.Parameters.AddWithValue("@idpres", idPresupuesto);
      insertCmd.Parameters.AddWithValue("@idprod", idProducto);
      insertCmd.Parameters.AddWithValue("@cantidad", cantidad);
      int numFilas = insertCmd.ExecuteNonQuery();

      return numFilas != 0;
    }
    catch (SqliteException ex) { // VERIFICA SI EXISTEN LOS PRESUPUESTOS Y LOS PRODUCTOS CON LOS ID DADOS
      Console.WriteLine("ERROR SQL: " + ex.Message);
      return false;
    }
  }

  public bool CrearPresupuesto(Presupuesto presupuesto)
  {
    SqliteConnection connection = new SqliteConnection(connectionString);
    connection.Open();

    string insertQuery = "INSERT INTO presupuesto (NombreDestinatario, FechaCreacion) VALUES (@nombre,@fecha)";
    using SqliteCommand insertCmd = new SqliteCommand(insertQuery, connection);
    insertCmd.Parameters.AddWithValue("@nombre", presupuesto.NombreDestinatario );
    insertCmd.Parameters.AddWithValue("@fecha", presupuesto.FechaCreacion.ToString("yyyy-MM-dd"));
    bool creado = insertCmd.ExecuteNonQuery() != 0;

    if (creado) {
      Presupuesto insertado = UltimoInsertado();
      foreach (PresupuestoDetalle detalle in presupuesto.Detalles)
        AgregarProducto(insertado.IdPresupuesto, detalle.Producto.IdProducto, detalle.Cantidad);
      return true;
    }
    return false;
  }

  public bool EliminarPresupuesto(int id)
  {
    SqliteConnection connection = new SqliteConnection(connectionString);
    connection.Open();

    string deleteQuery = "DELETE FROM Presupuesto WHERE IdPresupuesto = @id";
    using SqliteCommand deleteCmd = new SqliteCommand(deleteQuery, connection);
    deleteCmd.Parameters.AddWithValue("@id", id);

    string deleteDetalleQuery = "DELETE FROM PresupuestoDetalle WHERE IdPresupuesto = @id";
    using SqliteCommand deleteDetalleCmd = new SqliteCommand(deleteDetalleQuery, connection);
    deleteDetalleCmd.Parameters.AddWithValue("@id", id);

    int eliminadosDetalle = deleteDetalleCmd.ExecuteNonQuery();
    int eliminados = deleteCmd.ExecuteNonQuery();
    
    Console.WriteLine(eliminadosDetalle);
    return eliminados != 0 ;
  }

  public Presupuesto ObtenerPresupuestoPorId(int id)
  {
    using SqliteConnection connection = new SqliteConnection(connectionString);
    connection.Open();

    string selectQuery = "SELECT * FROM presupuesto WHERE IdPresupuesto = @id";
    using SqliteCommand selectCmd = new SqliteCommand(selectQuery, connection);
    selectCmd.Parameters.AddWithValue("@id", id);

    using SqliteDataReader reader = selectCmd.ExecuteReader();

    if (reader.Read())
    {
      int idPresupuesto = Convert.ToInt32(reader["IdPresupuesto"]);
      Presupuesto presupuesto = new Presupuesto
      {
        IdPresupuesto = idPresupuesto,
        FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]),
        NombreDestinatario = Convert.ToString(reader["NombreDestinatario"]),
        Detalles = ObtenerDetalles(idPresupuesto)
      };

      return presupuesto;
    }

    connection.Close();
    return null;
  }

  public List<Presupuesto> ObtenerPresupuestos()
  {
    using SqliteConnection connection = new SqliteConnection(connectionString);
    connection.Open();

    string selectQuery = "SELECT * FROM presupuesto";
    List<Presupuesto> resultado = [];
    using SqliteCommand selectCmd = new SqliteCommand(selectQuery, connection);

    using SqliteDataReader reader = selectCmd.ExecuteReader();

    while (reader.Read())
    {
      int idPresupuesto = Convert.ToInt32(reader["IdPresupuesto"]);
      Presupuesto presupuesto = new Presupuesto
      {
        IdPresupuesto = idPresupuesto,
        FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]),
        NombreDestinatario = Convert.ToString(reader["NombreDestinatario"]),
        Detalles = ObtenerDetalles(idPresupuesto)
      };

      resultado.Add(presupuesto);
    }

    connection.Close();
    return resultado;
  }
}