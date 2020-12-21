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
        public bool isvalid(int x1, int y1, int x2, int y2)
        {
            if (Math.Abs(x1 - x2) > 1 || Math.Abs(y1 - y2) > 1
                || GetColor(x2, y2) != Color.White)
                return false;
            if (x1 == x2 || y1 == y2)
                return true;
            if (x1 + y1 == 2 && x2 + y2 == 2)
                return true;
            if (x1 == y1 && x2 == y2)
                return true;

            return false;
        }
        bool findDest(int x1, int y1)
        {
            int[,] dirs = new int[,] { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };
            for (var i = 0; i < 4; i++)
            {
                var x = x1 + dirs[i, 0];
                var y = y1 + dirs[i, 1];
                if (x < 0 || y < 0 || x == 3 || y == 3)
                    continue;
                if (colboard[x, y] == Color.White)
                {
                    SetOpnStatus(x1, y1, true);
                    SetOpnStatus(x, y, false);
                    return true;
                }
            }
            return false;
        }
        bool findSource(int candx, int candy)
        {
            int[,] dirs = new int[,] { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };
            for (var i = 0; i < 4; i++)
            {
                var x = candx + dirs[i, 0];
                var y = candy + dirs[i, 1];
                if (x < 0 || y < 0 || x == 3 || y == 3)
                    continue;
                if (colboard[x, y] == Color.Blue)
                {
                    SetOpnStatus(candx, candy, false);
                    SetOpnStatus(x, y, true);
                    return true;
                }
            }
            return false;
        }

        public int moveinboard()
        {
            if (rowCrossed() || columnCrossed() || diagonalCrossed())
            {
                return 1;
            }
            var cds = checkforCand();
            if (cds.Item1 != -1 && cds.Item2 != -1)
            {
                //found possible destination candidates,
                //look is it possible to find source i.e 
                //blue adjacent cell to move from
                var found = findSource(cds.Item1, cds.Item2);
                if (found)
                {
                    if (rowCrossed() || columnCrossed() || diagonalCrossed())
                    {
                        return 2;
                    }
                    return 0;
                }
            }
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++)
                {
                    if (colboard[i, j] == Color.Blue)
                    {
                        if (findDest(i, j))
                        {
                            if (rowCrossed() || columnCrossed() || diagonalCrossed())
                            {
                                return 2;
                            }
                            return 0;
                        }
                    }
                }
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
        Tuple<int, int> checkcols()
        {
            for (int j = 0; j < 3; j++)
            {
                int cntb = 0;
                int cntr = 0;
                int idx = -1;
                for (int i = 0; i < 3; i++)
                {
                    if (colboard[i, j] == Color.White)
                        idx = i;
                    if (colboard[i, j] == Color.Red)
                        cntr++;
                    if (colboard[i, j] == Color.Blue)
                        cntb++;
                }
                if (cntb == 2 || cntr == 2 && idx != -1)
                {
                    return new Tuple<int, int>(idx, j); ;
                }
            }
            return new Tuple<int, int>(-1, -1);
        }
        Tuple<int, int> checkdiag1()
        {
            int cntr = 0;
            int cntb = 0;
            int idx = -1;
            for (int i = 0; i < 3; i++)
            {
                if (colboard[i, i] == Color.Red)
                    cntr++;
                if (colboard[i, i] == Color.Blue)
                    cntb++;
                if (colboard[i, i] == Color.White)
                    idx = i;
            }
            if ((cntb == 2 || cntr==2) && idx != -1)
            {
                  var color = cntb == 2 ? Color.Blue : Color.Red;
                return new Tuple<int, int>(idx,idx);
            }
            return new Tuple<int, int>(-1,-1);
        }
        Tuple<int, int> checkdiag2()
        {
            int cntr = 0;
            int cntb = 0;
            int idx = -1;
            for (int i = 0; i < 3; i++)
            {
                if (colboard[i, 2 - i] == Color.Red)
                    cntr++;
                if (colboard[i, 2 - i] == Color.Blue)
                    cntb++;
                if (colboard[i, 2 - i] == Color.White)
                    idx = i;
            }
            if ((cntb == 2 || cntr == 2) && idx != -1)
            {
                var color = cntb == 2 ? Color.Blue : Color.Red;
                return new Tuple<int, int>(idx, 2 - idx);
            }
            return new Tuple<int, int>(-1, -1);
        }
        Tuple<int,int> checkrows()
        {
            for (int i = 0; i < 3; i++)
            {
                int idx = -1;
                int cntb = 0;
                int cntr = 0;
                for(int j = 0; j < 3; j++)
                {
                    if (colboard[i, j] == Color.White)
                        idx = j;
                    if (colboard[i, j] == Color.Red)
                        cntr++;
                    if (colboard[i, j] == Color.Blue)
                        cntb++;
                }
                if (cntb == 2 || cntr == 2 && idx != -1)
                {
                    return new Tuple<int,int>(i,idx);
                }
           }
            return new Tuple<int, int>(1,-1);
        }
        //check for 2 blue or 2 red and 1 white cells in row/col/diags
        Tuple<int,int> checkforCand()
        {
            var result = checkrows();
            if (result.Item1 != -1 && result.Item2 != -1)
            {
                return result;
            }
            result = checkcols();
            if (result.Item1 != -1 && result.Item2 != -1)
            {
                return result;
            }
            result = checkdiag1();
            if (result.Item1 != -1 && result.Item2 != -1)
            {
                return result;
            }
            result = checkdiag2();
            if (result.Item1 != -1 && result.Item2 != -1)
            {
                return result;
            }
            return new Tuple<int, int>(-1, -1);
        }
        public int movestone()
        {
            if (rowCrossed() || columnCrossed() || diagonalCrossed())
            {
                return 1;
            }
            var res = checkforCand();
            if (res.Item1 != -1 && res.Item2 != -1)
            {
                colboard[res.Item1, res.Item2] = Color.Blue;
            }
            else
            {
                var rnd = new Random();
                int k = rnd.Next() % 9;
                int x = k / 3;
                int y = k % 3;
                while (colboard[x, y] != Color.White)
                {
                    k = rnd.Next() % 9;
                    x = k / 3;
                    y = k % 3;
                }
                colboard[x, y] = Color.Blue;
            }
            if (rowCrossed() || columnCrossed() || diagonalCrossed())
            {
                return 2;//Comp win
            }
            return -1;
        }
    }
}
