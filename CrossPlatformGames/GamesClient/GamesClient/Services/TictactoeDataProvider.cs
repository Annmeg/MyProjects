using System;
using System.Text;
using GameEngine.Models;
//using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GamesClient.Services
{
    class TictactoeDataProvider : IDataProvider
    {
        private static readonly HttpClient client = new HttpClient();
        private string BaseUrl = "https://webgamesapi.azurewebsites.net/api/games";
        // private string BaseUrl = "http://192.168.86.24:45455/api/games";// http://192.168.86.24:45455/api/games

        public async Task<GameBoard> GetBoardData(string name, int id, int user)
        {
            var url = BaseUrl + "/" + name + "/" + id + "/" + user;
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStringAsync();
                var board = JsonConvert.DeserializeObject<GameBoard>(stream);
                return board;
            }
            return null;
        }

        public async Task<GameBoard> UpdateBoard(string name, int x, int y, int id, int user)
        {
            var data = Tuple.Create(name, x, y);
            var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(data));

            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(BaseUrl + "/" + id + "/" + user, httpContent);
            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStringAsync();
                GameBoard board = JsonConvert.DeserializeObject<GameBoard>(stream);
                return board;
            }
            return null;
        }

        public async Task<GameBoard> ReserBoard(string name, int id)
        {
            var httpContent = new StringContent(name, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(BaseUrl + "/" + id, httpContent);
           if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStringAsync();
                GameBoard board = JsonConvert.DeserializeObject<GameBoard>(stream);
                return board;
            }
            return null;
        }
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
    }
}
