using LiveNotificationsMVC.Data;
using LiveNotificationsMVC.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace LiveNotificationsMVC.Hubs;

public class NotificationHub : Hub
{
    private readonly ApplicationDbContext _context;
    public NotificationHub(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task SendNotificationToAll(string message)
    {
        await Clients.All.SendAsync("ReceivedNotifications", message);
    }

    public async Task SendNotificationToClient(string username, string message)
    {
        await SaveUserConnection(username);

        var hubConnections = _context.HubNotifications
            .Where(x => x.Username == username)
            .ToList();

        foreach(var item in hubConnections)
        {
            await Clients.Client(item.ConnectionId).SendAsync("ReceivedClientNotification", message, username);
        }
    }

    public override Task OnConnectedAsync()
    {
        Clients.Caller.SendAsync("OnConnected");
        return base.OnConnectedAsync();
    }

    public async Task SaveUserConnection(string username)
    {
        if(username is not null)
        {
            var connectionId = Context.ConnectionId;
            var hubConnection = new HubNotification
            {
                ConnectionId = connectionId,
                Username = username
            };

            await _context.HubNotifications.AddAsync(hubConnection);
            await _context.SaveChangesAsync();
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var hubConnection = await _context.HubNotifications.FirstOrDefaultAsync(x => x.ConnectionId == Context.ConnectionId);
        if(hubConnection is null)
        {
            _context.HubNotifications.Remove(hubConnection);
            await _context.SaveChangesAsync();
        }

        base.OnDisconnectedAsync(exception);
    }
}
