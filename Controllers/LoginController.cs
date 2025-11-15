using System.CodeDom.Compiler;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using TP8.Models;

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
    if(!ModelState.IsValid)
      RedirectToAction("Index");

    bool ingreso = _authenticationService.Login(vm.Usuario, vm.Contrasena);
    Console.WriteLine(new {vm.Usuario, vm.Contrasena});
    return ingreso ? RedirectToAction("Index") : Redirect("Error");
  }

  [HttpGet]
  public IActionResult Logout()
  {
    _authenticationService.Logout();
    return RedirectToAction("Index");
  }

  [HttpGet]
  public IActionResult Error()
  {
    return View(new ErrorViewModel{RequestId = "asd"});
  }
}