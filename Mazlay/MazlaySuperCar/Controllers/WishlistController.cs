using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MazlaySuperCar.Controllers;

[Authorize]
public class WishlistController : Controller
{
    private readonly IWishlistService _wish;

    public WishlistController(IWishlistService wish) => _wish = wish;

    // GET /Wishlist
    [HttpGet("/Wishlist")]
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var ids    = await _wish.GetAsync(userId);
        return View(ids);                          // пока отдаём ID-шники
    }

    // POST /Wishlist/Toggle
    [HttpPost("/Wishlist/Toggle")]
    public async Task<IActionResult> Toggle(int productId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        await _wish.ToggleAsync(userId, productId);
        return Ok();
    }
}