using Domain.Entities;
using MazlaySuperCar.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MazlaySuperCar.Controllers;

[Route("Register")]
[AllowAnonymous]
public sealed class RegisterController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public RegisterController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager) =>
        (_userManager, _signInManager) = (userManager, signInManager);

    [HttpGet("")]
    public IActionResult Index() => View(new RegisterViewModel());

    [HttpPost("")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var email = model.Email.Trim();

        if (await _userManager.FindByEmailAsync(email) is not null)
        {
            ModelState.AddModelError(nameof(model.Email), "Такой e-mail уже зарегистрирован");
            return View(model);
        }

        var user = new ApplicationUser
        {
            Id       = Guid.NewGuid(),
            UserName = email,
            Email    = email
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Home");
        }

        foreach (var err in result.Errors)
            ModelState.AddModelError(string.Empty, err.Description);

        return View(model);
    }
}