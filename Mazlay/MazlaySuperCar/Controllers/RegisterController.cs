using Application.Interfaces;
using MazlaySuperCar.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MazlaySuperCar.Controllers;

public class RegisterController : Controller
{
    private readonly IAuthService _auth;
    public RegisterController(IAuthService auth) => _auth = auth;

    [HttpGet("/Register")]
    public IActionResult Index() => View();

    [HttpPost("/Register")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(RegisterViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var ok = await _auth.RegisterAsync(
            new RegisterDto(vm.Email, vm.Password));

        if (!ok) { ModelState.AddModelError("", "Не получилось"); return View(vm); }

        return RedirectToAction("Index", "Home");
    }
}