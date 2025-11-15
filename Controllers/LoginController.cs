using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;

public class LoginController : Controller
{
  private IAuthenticationService _authenticationService;
  public LoginController(IAuthenticationService authenticationService)
  {
    _authenticationService = authenticationService;
  }
  [HttpGet]
  public IActionResult Index()
  {
    return View();
  }
  [HttpPost]
  public IActionResult Login(LoginViewModel vm)
  {
    bool ingreso = _authenticationService.Login(vm.Usuario, vm.Contrasena);
    return ingreso ? RedirectToAction("Index") : RedirectToAction("Error");
  }
  [HttpGet]
  public IActionResult Logout()
  {
    _authenticationService.Logout();
    return RedirectToAction("Index");
  }
}