using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.Hubs
{
    public class NotificationHub : Hub
    {
        // Метод для отправки id заказа группе "Admins"
        public async Task SendOrderNotification(int orderId)
        {
            await Clients.Group("Admins").SendAsync("ReceiveOrderNotification", orderId);
        }

        // При подключении добавляем только админов в группу "Admins"
        public override async Task OnConnectedAsync()
        {
            if (Context.User.IsInRole("Admin"))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");
            }
            await base.OnConnectedAsync();
        }
    }
}