using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MazlaySuperCar.Controllers;

/// <summary>Список товаров с фильтрами поиска.</summary>
[Route("Shop")]
public sealed class ShopController : Controller
{
    private readonly IProductService _products;
    private const int PageSize = 12;

    public ShopController(IProductService products) => _products = products;

    /*  GET /Shop?cat=1&page=2&search=iPhone  */
    [HttpGet("")]
    public async Task<IActionResult> Index(int? cat, int page = 1, string? search = null)
    {
        var (items, total) = await _products.SearchAsync(page, PageSize, cat, search);

        ViewBag.Page       = page;
        ViewBag.TotalPages = (int)Math.Ceiling(total / (double)PageSize);
        ViewBag.CategoryId = cat;
        ViewBag.Query      = search;

        return View(items);                // Views/Shop/Index.cshtml
    }
}