using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameEngine.Models;


namespace GameWebApi.Data
{
    public interface IGamesData
    {
       Dictionary<string,List<GameBoard>> GetGames();
        GameBoard CreateJoinGame(object o);

        GameBoard UpdateGame(string name, int x, int y, int id, int usr);
        GameBoard GetGame(string name, int id);
        GameBoard GetGameState(string name, int id, int user);
        GameBoard ResetGame(string name, int id);
        bool DeleteGame(string name, int id);

    }
}
