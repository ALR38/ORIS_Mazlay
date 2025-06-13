using System.ComponentModel.DataAnnotations;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MazlaySuperCar.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signIn;
    private readonly UserManager<ApplicationUser>   _userMgr;
    public AccountController(SignInManager<ApplicationUser> s, UserManager<ApplicationUser> u)
        => (_signIn, _userMgr) = (s, u);

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
        => View(new LoginVm { ReturnUrl = returnUrl });

    [HttpPost]
    public async Task<IActionResult> Login(LoginVm vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var result = await _signIn.PasswordSignInAsync(vm.Email, vm.Password, vm.Remember, false);
        if (result.Succeeded) return LocalRedirect(vm.ReturnUrl ?? "/");

        ModelState.AddModelError("", "Invalid credentials");
        return View(vm);
    }

    public async Task<IActionResult> Logout()
    {
        await _signIn.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Denied() => View();
}

public record LoginVm
{
    [Required, EmailAddress]           public string Email    { get; init; } = null!;
    [Required, DataType(DataType.Password)] public string Password { get; init; } = null!;
    public bool   Remember  { get; init; }
    public string? ReturnUrl { get; init; }
}