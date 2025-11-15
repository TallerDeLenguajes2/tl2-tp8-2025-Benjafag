using System.CodeDom.Compiler;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using TP8.Models;

public class LoginController : Controller
{
  private readonly IAuthenticationService _authenticationService;
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
    if (!ModelState.IsValid)
      RedirectToAction("Index");

    bool ingreso = _authenticationService.Login(vm.Usuario, vm.Contrasena);

    if (ingreso)
      return RedirectToAction("Index","Home");
    
    vm.ErrorMessage =  "Credenciales invalidas";
    return View("Index", vm);
  }

  [HttpGet]
  public IActionResult Logout()
  {
    _authenticationService.Logout();
    return RedirectToAction("Index");
  }
}