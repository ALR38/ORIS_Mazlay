// CartController.cs

using Application.Interfaces;
using MazlaySuperCar.Models;
using Microsoft.AspNetCore.Mvc;

public sealed class CartController : Controller
{
    private readonly ICartService _cart;
    public CartController(ICartService cart) => _cart = cart;

    [HttpGet]                               // /Cart/Add/5
    public async Task<IActionResult> Add(int id, int qty = 1,
        string? returnUrl = null)
    {
        await _cart.AddAsync(id, qty);
        return LocalRedirect(returnUrl ?? Url.Action(nameof(Index))!);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(int productId,
        string? returnUrl = null)
    {
        await _cart.RemoveAsync(productId);
        return LocalRedirect(returnUrl ?? Url.Action(nameof(Index))!);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int productId, int qty)
    {
        await _cart.UpdateQtyAsync(productId, qty);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Index()
    {
        var lines = await _cart.GetLinesAsync();
        return View(new CartPageViewModel {
            Lines    = lines,
            Subtotal = lines.Sum(l => l.LineTotal)
        });
    }
}