using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Admin")]
[Route("admin")]
public class AdminController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("GetUsers")]
    public IActionResult GetUsers()
    {
        var users = _userManager.Users
            .Select(u => new { u.Id, u.UserName, u.Email })
            .ToList();
        return Json(users);
    }

    [HttpPost("DeleteUser")]
    public async Task<IActionResult> DeleteUser([FromBody] Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
            return NotFound();

        await _userManager.DeleteAsync(user);
        return Ok();
    }
}