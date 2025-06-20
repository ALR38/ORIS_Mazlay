using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MazlaySuperCar.Controllers;

[Authorize]
public class WishlistController : Controller
{
    private readonly IWishlistService _wish;

    public WishlistController(IWishlistService wish) => _wish = wish;

    [HttpGet("/Wishlist")]
    public async Task<IActionResult> Index()
    {
        var ids = await _wish.GetAsync(GetUserId());   
        return View(ids);                              
    }

    [HttpPost("/Wishlist/Toggle")]
    public async Task<IActionResult> Toggle(int productId)
    {
        await _wish.ToggleAsync(GetUserId(), productId);
        return Ok();
    }

    private Guid GetUserId() =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}