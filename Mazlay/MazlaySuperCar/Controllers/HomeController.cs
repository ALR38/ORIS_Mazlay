using System.Threading.Tasks;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MazlaySuperCar.Controllers;

public class HomeController : Controller
{
    private readonly IProductService _products;
    public HomeController(IProductService products) => _products = products;

    public async Task<IActionResult> Index()
    {
        // берём 8 товаров – ровно столько статических блоков выше
        var latest = await _products.GetLatestAsync(8);
        return View(latest);
    }
}