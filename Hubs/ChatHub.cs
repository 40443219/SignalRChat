using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR; 

using SignalRChat.Services;
using SignalRChat.Models;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IUserService _userService;

        public ChatHub(IUserService userService)
        {
            _userService = userService;
        }

        public async Task SendMessage(string user, string msg)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, msg);
        }

        public async Task SendGroupMessage(string groupName, string user, string msg)
        {
            await Clients.Group(groupName).SendAsync("ReceiveGroupMessage", groupName, user, msg);
        }

        // public async Task AddGroup(string groupName, string user)
        // {
        //     await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        //     await Clients.Group(groupName).SendAsync("RecGroupMsg", $"{ user } added to group: { groupName }");
        // }

        public async void AddtoUserList(string groupName, string user)
        {
            var _user = new UserModel
            {
                username = user,
                connectionId = Context.ConnectionId,
                groupName = groupName,
                onlineTime = DateTime.Now
            };

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("RecGroupMsg", $"{ user } added to group: { groupName }");
            await Clients.Group(groupName).SendAsync("UpdateUserList", await _userService.AddList(_user));
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            await base.OnDisconnectedAsync(ex);
            var user = await _userService.FindUser(Context.ConnectionId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, user.groupName);
            await Clients.Group(user.groupName).SendAsync("UpdateUserList", await _userService.RemoveList(user.connectionId));
            await Clients.Group(user.groupName).SendAsync("RecGroupMsg", $"{ user.username } disconnect.");
        }
    }
}