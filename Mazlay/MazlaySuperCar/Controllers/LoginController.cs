using Application.Interfaces;
using MazlaySuperCar.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MazlaySuperCar.Controllers;

public class LoginController : Controller
{
    private readonly IAuthService _auth;
    public LoginController(IAuthService auth) => _auth = auth;

    [HttpGet("/Login")]
    public IActionResult Index(string? returnUrl = null)
        => View(new LoginViewModel { ReturnUrl = returnUrl });

    [HttpPost("/Login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(LoginViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var ok = await _auth.LoginAsync(vm.Email, vm.Password, vm.Remember);
        if (!ok) { ModelState.AddModelError("", "Неверные данные"); return View(vm); }

        return LocalRedirect(vm.ReturnUrl ?? "/");
    }

    [HttpPost("/Logout")]
    public async Task<IActionResult> Logout()
    {
        await _auth.LogoutAsync();
        return RedirectToAction("Index", "Home");
    }
}