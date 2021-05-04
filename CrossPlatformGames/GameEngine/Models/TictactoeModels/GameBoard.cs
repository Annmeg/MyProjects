using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngine.Models
{
   public class GameBoard 
    {
        public GameBoard(string name)
        {
            UsersCount = 0;
            CX = -1;
            CY = -1;
            GameName = name;
            CurrentTurn = 0;
        }
        public string  GameName { get; set; }
        public int Id { get; set; }
        public int UsersCount { get; set; }
        public int CurrentTurn
        {
            get; set;
        }
        public int CX  { get; set; }
        public int CY { get; set; }
    }
}
