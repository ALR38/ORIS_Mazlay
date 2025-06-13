using Infrastructure.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Infrastructure.Hubs;

/// <summary>Хаб уведомлений о заказах.</summary>
[Authorize]
public class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        if (Context.User!.IsInRole(AppRoles.Admin))
            await Groups.AddToGroupAsync(Context.ConnectionId, HubGroups.Admins);

        if (Context.User.IsInRole(AppRoles.Manager))
            await Groups.AddToGroupAsync(Context.ConnectionId, HubGroups.Managers);

        await base.OnConnectedAsync();
    }
}