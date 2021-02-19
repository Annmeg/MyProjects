using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameEngine.Enums;

namespace GameEngine.Models
{
    public class GamePiece
    {
        public PieceStyle Style;

        public GamePiece()
        {
            Style = PieceStyle.Blank;
        }
    }
}
