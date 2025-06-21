using System.Threading.Tasks;
using Application.Interfaces;
using MazlaySuperCar.Models;
using Microsoft.AspNetCore.Mvc;

namespace MazlaySuperCar.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _products;
    public ProductController(IProductService products) => _products = products;

    // GET /Product/123
    [HttpGet("/Product/{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var product = await _products.GetByIdAsync(id);
        if (product is null) return NotFound();

        var vm = new ProductDetailsViewModel
        {
            Product = product,
            PrevId  = await _products.GetPrevIdAsync(id),
            NextId  = await _products.GetNextIdAsync(id)
        };
        return View(vm);
    }
}