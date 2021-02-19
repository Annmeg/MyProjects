using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR.Client;
 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameEngine.Models;

namespace gameWebservice
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private HubConnection connection;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }
        async Task NotifyBot(string[] board, string connectionID)
        {
            GameBoard game = new GameBoard();
            _logger.LogInformation($"Move received from {connectionID}");
            //  Move move = board.GetBestSpot(board, board.botPlayer);
            //   board[int.Parse(move.index)] = board.botPlayer;
            //   _logger.LogInformation($"Bot Move with the index of {move.index} send to {connectionID}");
            await connection.InvokeAsync("OnBotMoveReceived", board, connectionID);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:44380/gamehub")
                .Build();
            connection.On<string[], string>("NotifyBot", NotifyBot);
            await connection.StartAsync(); // Start the connection.

            //Add to BOT Group When Bot Connected
            await connection.InvokeAsync("OnBotConnected");
            _logger.LogInformation("Bot connected");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
