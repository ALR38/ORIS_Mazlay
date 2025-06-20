// MazlaySuperCar/Controllers/CartController.cs
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MazlaySuperCar.Controllers
{
    /// <summary>
    /// Корзина (работает и для гостей, и для авторизованных).
    /// </summary>
    [Route("[controller]")]          //   /Cart, /Cart/Add, …
    public class CartController : Controller
    {
        private readonly ICartService _cart;

        public CartController(ICartService cart) => _cart = cart;

        /* GET  /Cart */
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var id    = GetCurrentId();
            var lines = await _cart.GetAsync(id);

            return View(lines);      // Views/Cart/Index.cshtml
        }

        /* POST /Cart/Add */
        [HttpPost("Add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int productId, int qty = 1)
        {
            await _cart.AddAsync(GetCurrentId(), productId, qty);
            return Ok();
        }

        /* POST /Cart/Update */
        [HttpPost("Update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int productId, int qty)
        {
            var id = GetCurrentId();

            await _cart.RemoveAsync(id, productId);
            await _cart.AddAsync   (id, productId, qty);
            return Ok();
        }

        /* POST /Cart/Remove */
        [HttpPost("Remove")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int productId)
        {
            await _cart.RemoveAsync(GetCurrentId(), productId);
            return Ok();
        }

        /*────────────────── helpers ──────────────────*/

        /// <summary>
        /// Возвращает Guid для текущего зрителя:
        /// - Guid пользователя, если он авторизован  
        /// - или  Guid из сессии <c>CartAnonId</c>
        /// </summary>
        private Guid GetCurrentId()
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // авторизованный
            if (!string.IsNullOrWhiteSpace(claim) && Guid.TryParse(claim, out var userId))
                return userId;

            // гость -> пытаемся взять из сессии
            const string key = "CartAnonId";

            if (HttpContext.Session.TryGetValue(key, out var bytes) &&
                Guid.TryParse(System.Text.Encoding.UTF8.GetString(bytes!), out var anonId))
            {
                return anonId;
            }

            // впервые — создаём Guid и кладём в сессию
            var newId = Guid.NewGuid();
            HttpContext.Session.SetString(key, newId.ToString());

            return newId;
        }
    }
}
