using Microsoft.AspNetCore.SignalR;

namespace OJT_RAG.Services.Hubs
{
    public class UserChatHub : Hub
    {
        public async Task SendMessage(long senderId, long receiverId, string content)
        {
            await Clients.User(receiverId.ToString())
                .SendAsync("ReceiveMessage", senderId, content);
        }
    }
}
public class UserChatHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        Console.WriteLine($"[SignalR] User connected: {userId}");

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier;
        Console.WriteLine($"[SignalR] User disconnected: {userId}");

        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(long senderId, long receiverId, string content)
    {
        Console.WriteLine($"[SignalR] {senderId} -> {receiverId}: {content}");

        await Clients.User(receiverId.ToString())
            .SendAsync("ReceiveMessage", senderId, content);
    }
}

