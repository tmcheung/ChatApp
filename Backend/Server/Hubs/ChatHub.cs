using Microsoft.AspNetCore.SignalR;
using Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Hubs
{
    internal class ChatHub : Hub
    {
        private readonly ChatService _chatService;
        private const string defaultGroup = "default";

        public ChatHub(ChatService chatService)
        {
            _chatService = chatService;
        }

        public override async Task OnDisconnectedAsync(Exception e)
        {
            _chatService.HandleDisconnect(Context.ConnectionId);
            await NotifyMemberChange();
        }

        public override async Task OnConnectedAsync()
        {
        }

        public async Task<bool> JoinChat(string username)
        {
            try
            {
                _chatService.AddUser(username, Context.ConnectionId);
            }
            catch
            {
                return false;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, defaultGroup);
            await NotifyMemberChange();
            return true;
        }

        public async Task SendMessage(string message)
        {
            var callingUser = _chatService.GetUser(Context.ConnectionId);
            var m = new
            {
                MessageContent = message,
                Username = callingUser.Username
            };
            await Clients.Group(defaultGroup).SendAsync("ReceiveMessage", m);
        }

        private async Task NotifyMemberChange()
        {
            var users = _chatService.GetUsers();
            await Clients.All.SendAsync("ReceiveMembershipChange", users.Select(u => u.Key));
        }
    }
}
