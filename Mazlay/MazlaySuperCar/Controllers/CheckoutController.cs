using System.Linq;                         // ← для Sum(...)
using System.Threading.Tasks;
using Application.Interfaces;              // ICartService, IOrderService
using Application.Common.Interfaces;       // ICurrentUserService
using MazlaySuperCar.Models;               // CheckoutViewModel
using Microsoft.AspNetCore.Mvc;

namespace MazlaySuperCar.Controllers;

public sealed class CheckoutController : Controller
{
    private readonly ICartService        _cart;
    private readonly IOrderService       _orders;
    private readonly ICurrentUserService _user;

    public CheckoutController(
        ICartService        cart,
        IOrderService       orders,
        ICurrentUserService user)
    {
        _cart   = cart;
        _orders = orders;
        _user   = user;
    }

    /* ----------- Корзина перед оформлением -------------------------- */
    public async Task<IActionResult> Index()
    {
        var lines = await _cart.GetLinesAsync();

        var vm = new CheckoutViewModel
        {
            Lines = lines,
            Total = lines.Sum(l => l.Price * l.Quantity)
        };

        return View(vm);
    }

    /* ----------- Подтверждение заказа ------------------------------- */
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Place()
    {
        int orderId = await _orders.PlaceOrderAsync(_user.Id);

        TempData["orderId"] = orderId;        // для страницы «Спасибо»
        return RedirectToAction(nameof(Success));
    }

    public IActionResult Success() => View();
}