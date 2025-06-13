using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MazlaySuperCar.Controllers;

public class CartController : Controller
{
    private readonly ICartService    _cart;
    private readonly IProductService _products;

    public CartController(ICartService cart, IProductService products)
        => (_cart, _products) = (cart, products);

    // GET  /Cart
    [HttpGet("/Cart")]
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var lines  = await _cart.GetAsync(userId);
        return View(lines);                       // Views/Cart/Index.cshtml
    }

    // POST /Cart/Add   (AJAX или форма)
    [HttpPost("/Cart/Add")]
    public async Task<IActionResult> Add(int productId, int qty = 1)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        await _cart.AddAsync(userId, productId, qty);
        return Ok();
    }

    // POST /Cart/Update
    [HttpPost("/Cart/Update")]
    public async Task<IActionResult> Update(int productId, int qty)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        // простая логика: удалить старую строку и добавить новую qty
        await _cart.RemoveAsync(userId, productId);
        await _cart.AddAsync   (userId, productId, qty);
        return Ok();
    }

    // POST /Cart/Remove
    [HttpPost("/Cart/Remove")]
    public async Task<IActionResult> Remove(int productId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        await _cart.RemoveAsync(userId, productId);
        return Ok();
    }
}