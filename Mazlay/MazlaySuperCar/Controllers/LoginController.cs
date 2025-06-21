using Domain.Entities;
using MazlaySuperCar.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MazlaySuperCar.Controllers;

[Route("Login")]
[AllowAnonymous]                   // к контроллеру целиком
public sealed class LoginController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser>   _userManager;

    public LoginController(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser>   userManager) =>
        (_signInManager, _userManager) = (signInManager, userManager);

    /* ---------- GET  /Login ---------- */
    [HttpGet("")]
    public IActionResult Index(string returnUrl = "/") =>
        View(new LoginViewModel { ReturnUrl = returnUrl });

    /* ---------- POST /Login ---------- */
    [HttpPost("")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email.Trim());
        if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
        {
            ModelState.AddModelError(string.Empty, "Неверный e-mail или пароль");
            return View(model);
        }

        await _signInManager.SignInAsync(user, model.Remember);
        return LocalRedirect(model.ReturnUrl ?? "/");
    }

    /* ---------- POST /Login/Logout ---- */
    [HttpPost("Logout")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}