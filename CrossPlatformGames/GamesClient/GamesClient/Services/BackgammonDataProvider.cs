using GameEngine.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GamesClient.Services
{
    class BackgammonDataProvider : IDataProvider
    {
        private static readonly HttpClient client = new HttpClient();
        private static string BaseUrl = "https://webgamesapi.azurewebsites.net/api/games";

        public async Task<GameBoard> CreateBoard(int gameId)
        {
            InitData data = new InitData();
            data.GameName = "tictactoe";
            data.UserName = "user";
            data.Id = gameId;
            var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(data));

            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            var uri = BaseUrl;

            HttpResponseMessage response = await client.PostAsync(uri, httpContent);

            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStringAsync();
                var board = JsonConvert.DeserializeObject<GameBoard>(stream);
                return board;
            }
            return null;
        }

        public Task<GameBoard> GetBoardData(string name, int id, int user)
        {
            throw new NotImplementedException();
        }

        public Task<GameBoard> ReserBoard(string name, int id)
        {
            throw new NotImplementedException();
        }

        public Task<GameBoard> UpdateBoard(string name, int x, int y, int id, int user)
        {
            throw new NotImplementedException();
        }
    }
}
