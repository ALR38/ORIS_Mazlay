using System.Linq;                    // Sum(..)
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces;
using MazlaySuperCar.Models;
using Microsoft.AspNetCore.Mvc;

namespace MazlaySuperCar.Controllers;

/// <summary>
///  Корзина.  Все действия выполняются БЕЗ проверки Antiforgery-cookie,
///  поэтому ссылки вида /Cart/Add/5 работают как обычные GET-запросы.
/// </summary>
[IgnoreAntiforgeryToken]              //  <-- главное лекарство
public sealed class CartController : Controller
{
    private readonly ICartService _cart;
    public CartController(ICartService cart) => _cart = cart;

    /* ---------- ADD ( /Cart/Add/5?qty=1 ) ---------- */
    [HttpGet("Cart/Add/{id:int}")]
    public async Task<IActionResult> Add(int id, int qty = 1, string? returnUrl = null)
    {
        await _cart.AddAsync(id, qty);
        return LocalRedirect(returnUrl ?? Url.Action(nameof(Index))!);
    }

    /* ---------- REMOVE ---------- */
    [HttpPost("Cart/Remove"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(int productId, string? returnUrl = null)
    {
        await _cart.RemoveAsync(productId);
        return LocalRedirect(returnUrl ?? Url.Action(nameof(Index))!);
    }

    /* ---------- UPDATE QTY ---------- */
    [HttpPost("Cart/Update"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int productId, int qty)
    {
        await _cart.UpdateQtyAsync(productId, qty);
        return RedirectToAction(nameof(Index));
    }

    /* ---------- CART PAGE ---------- */
    [HttpGet("Cart")]
    [HttpGet("Cart/Index")]
    public async Task<IActionResult> Index()
    {
        IReadOnlyList<CartLineDto> lines = await _cart.GetLinesAsync();

        var vm = new CartPageViewModel
        {
            Lines    = lines,
            Subtotal = lines.Sum(l => l.LineTotal)
        };
        return View(vm);
    }
}