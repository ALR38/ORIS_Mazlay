// Controllers/RegisterController.cs
using System.Threading.Tasks;
using Application.Abstractions;
using Application.DTOs;
using Application.Interfaces;
using MazlaySuperCar.Models;
using Microsoft.AspNetCore.Mvc;

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

        var result = await _auth.RegisterAsync(new RegisterDto(vm.Email, vm.Password));
        if (!result.Succeeded)
        {
            foreach (var err in result.Errors)
                ModelState.AddModelError(string.Empty, err);
            return View(vm);
        }
        return RedirectToAction("Index", "Home");
    }
}
