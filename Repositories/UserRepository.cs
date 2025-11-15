using Microsoft.Data.Sqlite;

public class UserRepository : IUserRepository
{
  private static readonly string connectionString = "Data Source = ../tienda.db;";
  public Usuario GetUser(string usuario, string contrasena)
  {
    SqliteConnection connection = new SqliteConnection(connectionString);
    connection.Open();
    string selectString = "SELECT nombre, usuario, rol FROM usuario WHERE usuario = @usuario and contrasena = @contrasena";
    SqliteCommand selectCmd = new SqliteCommand(selectString,connection);
    selectCmd.Parameters.AddWithValue("@usuario", usuario); 
    selectCmd.Parameters.AddWithValue("@contrasena", contrasena);

    using SqliteDataReader reader = selectCmd.ExecuteReader();
    Usuario user;
    if (reader.Read())
      user = new Usuario
      {
        Nombre = Convert.ToString(reader[0]),
        NombreUsuario = Convert.ToString(reader[1]),
        Rol = Convert.ToString(reader[2])
      };
    else 
      user = null;
    connection.Close();
    return user;
  }
}