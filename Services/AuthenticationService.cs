public class AuthenticationService : IAuthenticationService
{
  private IUserRepository _userRepository;
  private IHttpContextAccessor _contextAccessor;
  public AuthenticationService(IUserRepository userRepository, IHttpContextAccessor contextAccessor)
  {
    _userRepository = userRepository;
    _contextAccessor = contextAccessor;
  }

  public bool EstaAutenticado()
  {
    return _contextAccessor.HttpContext.Session.GetString("IsAuthenticated") == "true";
  }

  public bool Login(string usuario, string contrasena)
  {
    Usuario user = _userRepository.GetUser(usuario, contrasena);
    var context = _contextAccessor.HttpContext;
    if (user != null)
      if (context != null)
      {
        context.Session.SetString("IsAuthenticated", "true"); 
        context.Session.SetString("User", user.NombreUsuario); 
        context.Session.SetString("UserNombre", user.Nombre); 
        context.Session.SetString("Rol", user.Rol); 
        return true;
      }
    return false;
  }

  public void Logout()
  {
    if (EstaAutenticado())
    {
      _contextAccessor.HttpContext.Session.Clear();
    }
  }

  public bool TieneNivelAcceso(string nivelAcceso)
  {
    return _contextAccessor.HttpContext.Session.GetString("Rol")?.ToLower() == nivelAcceso.ToLower();
  }
}