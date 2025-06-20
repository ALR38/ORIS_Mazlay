using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Interfaces;
using MazlaySuperCar.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MazlaySuperCar.Controllers;

/// <summary>Оформление заказа.</summary>
[Authorize]
[Route("Checkout")]
public sealed class CheckoutController : Controller
{
    private readonly ICartService  _cart;
    private readonly IOrderService _orders;

    public CheckoutController(ICartService cart, IOrderService orders) =>
        (_cart, _orders) = (cart, orders);

    /* GET  /Checkout */
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var lines = await _cart.GetAsync(GetUserId());
        return View(new CheckoutViewModel { Lines = lines });
    }

    /* POST /Checkout/Pay */
    [HttpPost("Pay")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Pay()
    {
        var uid   = GetUserId();
        var lines = await _cart.GetAsync(uid);

        if (!lines.Any())
            return RedirectToAction("Index", "Cart");

        var orderId = await _orders.CreateAsync(uid, lines);
        await _cart.ClearAsync(uid);

        return RedirectToAction("Success", new { id = orderId });
    }

    /* GET /Checkout/Success/123 */
    [HttpGet("Success/{id:int}")]
    public IActionResult Success(int id) => View(model: id);

    private Guid GetUserId() =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}