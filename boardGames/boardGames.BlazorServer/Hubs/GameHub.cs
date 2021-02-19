using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;


namespace boardGames.BlazorServer.Hubs
{
    public class GameHub : Microsoft.AspNetCore.SignalR.Hub
    {
        ILogger<GameHub> _logger;
        private static readonly string BOT_GROUP = "BOT";

        public GameHub(ILogger<GameHub> logger)
        {
            _logger = logger;
        }

        public async Task OnBotConnected()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, BOT_GROUP);
            _logger.LogInformation("Bot joined");
        }

        public async Task OnBotDisconnected()
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, BOT_GROUP);
            _logger.LogInformation("Bot left");
        }

        public async Task OnBotMoveReceived(string[] board, string connectionID)
        {
            await Clients.Client(connectionID).SendAsync("NotifyUser", board);
        }

        public async Task OnUserMoveReceived(string[] board)
        {
            await Clients.Group(BOT_GROUP).SendAsync("NotifyBot", board, Context.ConnectionId);
        }
    }
}
