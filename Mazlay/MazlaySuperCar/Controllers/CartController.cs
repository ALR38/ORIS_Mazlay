using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MazlaySuperCar.Controllers;

/// <summary>Корзина текущего пользователя.</summary>
[Route("Cart")]
public sealed class CartController : Controller
{
    private readonly ICartService _cart;
    public CartController(ICartService cart) => _cart = cart;

    /* GET  /Cart */
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var lines = await _cart.GetAsync(GetUserId());
        return View(lines);                // Views/Cart/Index.cshtml
    }

    /* POST /Cart/Add   (productId, qty) */
    [HttpPost("Add")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(int productId, int qty = 1)
    {
        await _cart.AddAsync(GetUserId(), productId, qty);
        return Ok();
    }

    /* POST /Cart/Update */
    [HttpPost("Update")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int productId, int qty)
    {
        var uid = GetUserId();
        await _cart.RemoveAsync(uid, productId);
        await _cart.AddAsync   (uid, productId, qty);
        return Ok();
    }

    /* POST /Cart/Remove */
    [HttpPost("Remove")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(int productId)
    {
        await _cart.RemoveAsync(GetUserId(), productId);
        return Ok();
    }

    /* helpers */
    private Guid GetUserId() =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}