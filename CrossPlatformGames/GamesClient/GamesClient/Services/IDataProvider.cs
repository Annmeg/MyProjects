using GameEngine.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GamesClient.Services
{
    interface IDataProvider
    {
        Task<GameBoard> GetBoardData(string name, int id, int user);
        Task<GameBoard> UpdateBoard(string name, int x, int y, int id, int user);
        Task<GameBoard> CreateBoard(int gameId);
        Task<GameBoard> ReserBoard(string name, int id);

    }
}
