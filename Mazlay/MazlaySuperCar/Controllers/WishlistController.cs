using System.Threading.Tasks;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MazlaySuperCar.Controllers;

/// <summary>
///  Вишлист — также без Antiforgery, чтобы ссылки /Wishlist/Toggle/5 работали из <a>.
/// </summary>
[IgnoreAntiforgeryToken]
public sealed class WishlistController : Controller
{
    private readonly IWishlistService _wish;
    public WishlistController(IWishlistService wish) => _wish = wish;

    /* ---------- TOGGLE ( /Wishlist/Toggle/5 ) ---------- */
    [HttpGet("Wishlist/Toggle/{id:int}")]
    public async Task<IActionResult> Toggle(int id, string? returnUrl = null)
    {
        await _wish.ToggleAsync(id);
        return LocalRedirect(returnUrl ?? Url.Action(nameof(Index))!);
    }

    /* ---------- CLEAR WHOLE LIST ---------- */
    [HttpPost("Wishlist/Clear"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Clear()
    {
        await _wish.ClearAsync();
        return RedirectToAction(nameof(Index));
    }

    /* ---------- WISHLIST PAGE ---------- */
    [HttpGet("Wishlist")]
    [HttpGet("Wishlist/Index")]
    public async Task<IActionResult> Index()
        => View(await _wish.GetItemsAsync());
}