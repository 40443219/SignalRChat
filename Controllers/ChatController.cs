using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

using SignalRChat.Hubs;

namespace SignalRChat.Controllers
{
    [Route("/Chat")]
    public class ChatController : Controller
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost]
        [Route("SendMessage")]
        public async Task SendMessage(string user, string msg)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", user, msg);
        }
    }
}