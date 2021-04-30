using GameEngine.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using GameEngine.Models;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;

namespace GameWebApi.Data
{
    public class GamesData : IGamesData
    {
        private static Dictionary<string, List<GameBoard>> games = new Dictionary<string, List<GameBoard>>();
        GameBoard game;
        private Dictionary<int, object> _locks = new Dictionary<int, object>();

        private object _cdLock = new object();//create/delete lock
        public GameBoard CreateJoinGame(object obj)
        {
            lock (_cdLock)
            {
                InitData initData = JsonConvert.DeserializeObject<InitData>(obj.ToString());
                var name = initData.GameName;
                var usr = initData.UserName;
                var id = initData.Id;
                if (!games.ContainsKey(name))
                {
                    games.Add(name, new List<GameBoard>());
                }
                var sorted = games[name].OrderBy(x => x.Id).ToList();
                if (sorted.Count == 0 || sorted.Last<GameBoard>().UsersCount == 2)
                {
                    game = new GameBoard(name);

                    game.Id = DateTime.Now.Second + DateTime.Now.Minute * 60 + DateTime.Now.Hour * 3600;
                    game.UsersCount++;
                    game.CurrentTurn = 1;
                    sorted.Add(game);
                    games[name] = sorted;
                    _locks.Add(game.Id, new object());
                    return game;
                }
                game = sorted.Last<GameBoard>();
                game.UsersCount++;
                games[name][sorted.Count - 1] = game;
                return game;
            }
       }    
        public bool DeleteGame(string name, int id)
        {
            lock (_cdLock)
            {
                if (!games.ContainsKey(name) || games[name].Find(g => g.Id == id) == null)
                    return false;
                int idx = games[name].FindIndex(g => g.Id == id);
                games[name].RemoveAt(idx);
                return true;
            }
        }

        public GameBoard GetGame(string name, int id)
        {
            if (!games.ContainsKey(name))
                return null;
            game = games[name].SingleOrDefault(x => x.Id == id);
            return game;
        }
        public Dictionary<string, List<GameBoard>> GetGames()
        {
            return games;
        }

        public GameBoard GetGameState(string name, int id, int user)
        {
            lock (_locks[id])
            {
                while ((game = GetGame(name, id)) == null || game.CurrentTurn != user)
                    Monitor.Wait(_locks[id]);
            }
            return game;
        }

        public GameBoard ResetGame(string name, int id)
        {
            int idx = games[name].FindIndex(g => g.Id == id);
            games[name][idx].CurrentTurn = 1;
            games[name][idx].CX = -1;
            games[name][idx].CY = -1;
            game = games[name][idx];
            return game;
        }

        public GameBoard UpdateGame(string name, int x, int y, int id, int usr)
        {
            lock (_locks[id])
            {
                if (games.ContainsKey(name))
                {
                    var idx = games[name].FindIndex(g => g.Id == id);
                    game = games[name][idx];
                    game.CX = x;
                    game.CY = y;
                    game.CurrentTurn = usr==1?2:1;
                    games[name][idx] = game;
                    Monitor.PulseAll(_locks[id]);
                }
            }
            return game;
        }
    }
}
