// WishlistController.cs

using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

public sealed class WishlistController : Controller
{
    private readonly IWishlistService _wish;
    public WishlistController(IWishlistService wish) => _wish = wish;

    [HttpGet]                               // /Wishlist/Toggle/5
    public async Task<IActionResult> Toggle(int id,
        string? returnUrl = null)
    {
        await _wish.ToggleAsync(id);
        return LocalRedirect(returnUrl ?? Url.Action(nameof(Index))!);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Clear()
    {
        await _wish.ClearAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Index() =>
        View(await _wish.GetItemsAsync());
}