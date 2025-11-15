public interface IAuthenticationService
{
  bool Login(string usuario, string contrasena);
  void Logout();
  bool EstaAutenticado();
  bool TieneNivelAcceso(string nivelAcceso); 
}