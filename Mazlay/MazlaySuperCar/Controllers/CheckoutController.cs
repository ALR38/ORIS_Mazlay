using Application.Interfaces;
using Application.Common.Interfaces;
using Infrastructure.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace MazlaySuperCar.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ICartService _cart;
        private readonly IOrderService _orders;
        private readonly ICurrentUserService _user;
        private readonly IHubContext<NotificationHub> _hubContext;

        public CheckoutController(
            ICartService cart,
            IOrderService orders,
            ICurrentUserService user,
            IHubContext<NotificationHub> hubContext)
        {
            _cart = cart;
            _orders = orders;
            _user = user;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> Index()
        {
            var lines = await _cart.GetLinesAsync();

            var vm = new MazlaySuperCar.Models.CheckoutViewModel
            {
                Lines = lines,
                Total = lines.Sum(l => l.Price * l.Quantity)
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Place()
        {
            int orderId = await _orders.PlaceOrderAsync(_user.Id);

            // SignalR: уведомление админам
            await _hubContext.Clients.Group("Admins")
                .SendAsync("ReceiveOrderNotification", orderId);

            TempData["orderId"] = orderId;
            return RedirectToAction(nameof(Success));
        }

        public IActionResult Success() => View();
    }
}