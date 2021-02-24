using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR.Client;
 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameEngine.Models;
using GameEngine.Enums;

namespace gameWebservice
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private HubConnection connection;
        private GameBoard gameboard;
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }
        async Task NotifyBot(string[] board, string connectionID)
        {
            _logger.LogInformation($"Move received from {connectionID}");
             Move move = gameboard.findBestMove();
            board[move.row*3+move.col] = "O";
             _logger.LogInformation($"Bot Move with {move.row}, {move.col} send to {connectionID}");
            await connection.InvokeAsync("OnBotMoveReceived", board, connectionID);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            gameboard = new GameBoard();
                connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:44380/")
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
