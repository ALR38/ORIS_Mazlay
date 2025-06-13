using Application.Interfaces;
using MazlaySuperCar.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MazlaySuperCar.Controllers;

[Authorize]
public class CheckoutController : Controller
{
    private readonly ICartService  _cart;
    private readonly IOrderService _orders;
    private const string ClaimId = ClaimTypes.NameIdentifier;

    public CheckoutController(ICartService cart, IOrderService orders)
        => (_cart, _orders) = (cart, orders);

    [HttpGet("/Checkout")]
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimId)!;
        var lines  = await _cart.GetAsync(userId);
        return View(new CheckoutViewModel { Lines = lines });
    }

    [HttpPost("/Checkout/Pay")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Pay()
    {
        var userIdStr = User.FindFirstValue(ClaimId)!;
        var lines     = await _cart.GetAsync(userIdStr);
        if (!lines.Any()) return RedirectToAction("Index", "Cart");

        var userId = GetUserId();
        var orderId = await _orders.CreateAsync(userId, lines);

        await _cart.ClearAsync(userIdStr);
        return RedirectToAction("Success", new { id = orderId });
    }

    [HttpGet("/Checkout/Success/{id:int}")]
    public IActionResult Success(int id) => View(model: id);
    
    private Guid GetUserId()
        => Guid.Parse(User.FindFirstValue(ClaimId)!);
}