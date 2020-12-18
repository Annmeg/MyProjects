using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ArmTikTakToeX
{
    class Board
    {
        int cols = 3;
        int rows = 3;
        Random rnd = new Random();

        Color[,] colboard = new Color[3, 3];//cells color empty

        public int mystones { get; set; }
        public int opstones { get; set; }
        public void SetSize(int cols, int rows)
        {
            if (cols <= 0 || rows <= 0)
                throw new ArgumentException("Board.SetSize: Arguments must be greater than zero");
            this.cols = cols;
            this.rows = rows;
            mystones = 3;
            opstones = 3;
        }
        public void SetStatus(int row, int col, bool isempty)
        {
            if (!isempty)
                colboard[row, col] = Color.Red;
            else
                colboard[row, col] = Color.White;
        }
        public void SetOpnStatus(int row, int col, bool isempty)
        {
            if (!isempty)
                colboard[row, col] = Color.Blue;
            else
                colboard[row, col] = Color.White;
        }
        public Color GetColor(int x, int y)
        {
            return colboard[x, y];
        }
        public void Reset()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    colboard[i, j] = Color.White;
                }
            }
            mystones = 3; opstones = 3;
        }
        public int selectSourceTarget(out int x1, out int y1, out int x2, out int y2)
        {
            x1 = -1; y1 = -1;
            x2 = -1; y2 = -1;
            if (rowCrossed() || columnCrossed() || diagonalCrossed())
            {
                return 1;
            }

            int k = rnd.Next() % 9;
            x1 = k / 3;
            y1 = k % 3;
            while (colboard[x1, y1] != Color.Blue)
            {
                k = rnd.Next() % 9;
                x1 = k / 3;
                y1 = k % 3;
            }
            int[,] dirs = new int[,] { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };
            for (var i = 0; i < 4; i++)
            {
                var x = x1 + dirs[i, 0];
                var y = y1 + dirs[i, 1];
                if (x < 0 || y < 0 || x == 3 || y == 3)
                    continue;
                if (colboard[x, y] == Color.White)
                {
                    x2 = x;
                    y2 = y;
                    break;
                }
            }
            if (x1 == y1 && x1 + 1 < 3 && colboard[x1 + 1, y1 + 1] == Color.White)
            {
                x2 = x1 + 1;
                y2 = y1 + 1;
            }
            else if (x1 == y1 && x1 - 1 >= 0 && colboard[x1 - 1, y1 - 1] == Color.White)
            {
                x2 = x1 - 1;
                y2 = y1 - 1;
            }
            else if (x1 + y1 == 3)
            {
                if (x1 - 1 >= 0 && y1 + 1 < 3 && colboard[x1 - 1, y1 + 1] == Color.White)
                {
                    x2 = x1 - 1;
                    y2 = y1 + 1;
                }
                else if (y1 - 1 >= 0 && x1 + 1 < 3 && colboard[x1 + 1, y1 - 1] == Color.White)
                {
                    x2 = x1 + 1;
                    y2 = y1 - 1;
                }
            }
            if (rowCrossed() || columnCrossed() || diagonalCrossed())
            {
                return 2;
            }
            return 0;
        }
        // A function that returns true if any of the row 
        // is crossed with the same player's move 
        bool rowCrossed()
        {
            for (int i = 0; i < 3; i++)
            {
                if (colboard[i, 0] == colboard[i, 1] &&
                    colboard[i, 1] == colboard[i, 2] &&
                    colboard[i, 0] != Color.White)
                {
                    return true;
                }
            }
            return (false);
        }

        // A function that returns true if any of the column 
        // is crossed with the same player's move 
        bool columnCrossed()
        {
            for (int i = 0; i < 3; i++)
            {
                if (colboard[0, i] == colboard[1, i] &&
                    colboard[1, i] == colboard[2, i] &&
                    colboard[0, i] != Color.White)
                {
                    return true;
                }
            }
            return (false);
        }

        // A function that returns true if any of the diagonal 
        // is crossed with the same player's move 
        bool diagonalCrossed()
        {
            if (colboard[0, 0] == colboard[1, 1] &&
                colboard[1, 1] == colboard[2, 2] &&
                colboard[0, 0] != Color.White)
            {
                return true;
            }

            if (colboard[0, 2] == colboard[1, 1] &&
                colboard[1, 1] == colboard[2, 0] &&
                 colboard[0, 2] != Color.White)
            {
                return true;
            }
            return (false);
        }
        public int movestone()
        {
            if (rowCrossed() || columnCrossed() || diagonalCrossed())
            {
                return 1;
            }
            int x = -1;
            int y = -1;
            var rnd = new Random();
            int k = rnd.Next() % 9;
            x = k / 3;
            y = k % 3;
            while (colboard[x, y] != Color.White)
            {
                k = rnd.Next() % 9;
                x = k / 3;
                y = k % 3;
            }
            colboard[x, y] = Color.Blue;
            if (rowCrossed() || columnCrossed() || diagonalCrossed())
            {
                return 2;//Comp win
            }
            return -1;
        }
    }

}
