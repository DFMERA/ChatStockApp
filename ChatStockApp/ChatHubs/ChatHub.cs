using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using ChatStockApp.Models;

namespace ChatStockApp.ChatHubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(Message message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
