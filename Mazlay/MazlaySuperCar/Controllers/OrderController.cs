using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public sealed class OrderController : Controller
{
    private readonly IOrderService          _orders;
    private readonly UserManager<ApplicationUser> _users;

    public OrderController(IOrderService orders, UserManager<ApplicationUser> users)
    {
        _orders = orders;
        _users  = users;
    }

    // GET /Order/Checkout
    public IActionResult Checkout() => View();   // простая форма с кнопкой «Place order»

    // POST /Order/Place
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Place()
    {
        var id = await _orders.PlaceOrderAsync(Guid.Parse(_users.GetUserId(User)!));
        return RedirectToAction(nameof(Details), new { id });
    }

    public async Task<IActionResult> Details(int id)
    {
        var userId = Guid.Parse(_users.GetUserId(User)!);
        var orders = await _orders.GetMyOrdersAsync(userId);
        var order  = orders.FirstOrDefault(o => o.Id == id);
        return order is null ? NotFound() : View(order);
    }

    public async Task<IActionResult> History()
    {
        var userId = Guid.Parse(_users.GetUserId(User)!);
        return View(await _orders.GetMyOrdersAsync(userId));
    }
}