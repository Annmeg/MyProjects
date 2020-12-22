using System;
using System.Collections.Generic;
using System.Text;

namespace TikTakToe
{
    class Grid
    {
        public class Move
        {
            public int row { get; set; }
            public int col { get; set; }
        };

        Random randGame = new Random();

        int cols = 3;
        int rows = 3;
        string[,] board = new string[3, 3];

        public void SetSize(int cols, int rows)
        {
            if (cols <= 0 || rows <= 0)
                throw new ArgumentException("Grid.SetSize: Arguments must be greater than zero");
            this.cols = cols;
            this.rows = rows;
        }
        public bool IsEmpty(int x, int y)
        {
            return board[x, y] == "";
        }
        public string GetCellValue(int x, int y)
        {
            return board[x, y];
        }
        public void Clear()
        {
            CreateGridArray();
        }

        public Grid()
        {
            CreateGridArray();
        }

        public void SetStatus(int row, int col, string val)
        {
            board[row, col] = val;
        }

        void CreateGridArray()
        {
            if (rows <= 0 || cols <= 0)
            {
                board = null;
                return;
            }
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    board[i, j] = "";
                }
            }
        }
        // A function that returns true if any of the row 
        // is crossed with the same player's move 
        bool rowCrossed()
        {
            for (int i = 0; i < 3; i++)
            {
                if (board[i, 0] == board[i, 1] &&
                    board[i, 1] == board[i, 2] &&
                    board[i, 0] != "")
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
                if (board[0, i] == board[1, i] &&
                    board[1, i] == board[2, i] &&
                    board[0, i] != "")
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
            if (board[0, 0] == board[1, 1] &&
                board[1, 1] == board[2, 2] &&
                board[0, 0] != "")
            {
                return true;
            }

            if (board[0, 2] == board[1, 1] &&
                board[1, 1] == board[2, 0] &&
                 board[0, 2] != "")
            {
                return true;
            }
            return (false);
        }
        // This function returns true if there are moves 
        // remaining on the board. It returns false if 
        // there are no moves left to play. 
        bool isMovesLeft(string[,] b)
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (b[i, j] == "")
                        return true;
            return false;
        }

        int evaluate(string[,] b)
        {
            // Checking for Rows for X or O victory. 
            for (int row = 0; row < 3; row++)
            {
                if (b[row, 0] == b[row, 1] &&
                    b[row, 1] == b[row, 2])
                {
                    if (b[row, 0] == "O")
                        return +10;
                    else if (b[row, 0] == "X")
                        return -10;
                }
            }

            // Checking for Columns for X or O victory. 
            for (int col = 0; col < 3; col++)
            {
                if (b[0, col] == b[1, col] &&
                    b[1, col] == b[2, col])
                {
                    if (b[0, col] == "O")
                        return +10;

                    else if (b[0, col] == "X")
                        return -10;
                }
            }

            // Checking for Diagonals for X or O victory. 
            if (b[0, 0] == b[1, 1] && b[1, 1] == b[2, 2])
            {
                if (b[0, 0] == "O")
                    return +10;
                else if (b[0, 0] == "X")
                    return -10;
            }

            if (b[0, 2] == b[1, 1] && b[1, 1] == b[2, 0])
            {
                if (b[0, 2] == "O")
                    return +10;
                else if (b[0, 2] == "X")
                    return -10;
            }

            // Else if none of them have won then return 0 
            return 0;
        }

        // This is the minimax function. It considers all 
        // the possible ways the game can go and returns 
        // the value of the board 
        int minimax(string[,] b, int depth, bool isMax)
        {
            int score = evaluate(b);

            // If Maximizer has won the game return his/her 
            // evaluated score 
            if (score == 10)
                return score;

            // If Minimizer has won the game return his/her 
            // evaluated score 
            if (score == -10)
                return score;

            // If there are no more moves and no winner then 
            // it is a tie 
            if (isMovesLeft(b) == false)
                return 0;

            // If this maximizer's move 
            if (isMax)
            {
                int best = -1000;

                // Traverse all cells 
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (b[i, j] == "")
                        {
                            b[i, j] = "O";
                            best = Math.Max(best,
                                minimax(b, depth + 1, !isMax));
                            b[i, j] = "";
                        }
                    }
                }
                return best;
            }
            else
            {
                int best = 1000;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (b[i, j] == "")
                        {
                            b[i, j] = "X";
                            best = Math.Min(best,
                                minimax(b, depth + 1, !isMax));
                            b[i, j] = "";
                        }
                    }
                }
                return best;
            }
        }
        Move findBestMove(string[,] board)
        {
            int bestVal = -1000;
            Move bestMove = new Move();
            bestMove.row = -1;
            bestMove.col = -1;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    // Check if cell is empty 
                    if (board[i, j] == "")
                    {
                        // Make the move 
                        board[i, j] = "O";

                        // compute evaluation function for this 
                        // move. 
                        int moveVal = minimax(board, 0, false);

                        // Undo the move 
                        board[i, j] = "";

                        // If the value of the current move is 
                        // more than the best value, then update 
                        // best/ 
                        if (moveVal > bestVal)
                        {
                            bestMove.row = i;
                            bestMove.col = j;
                            bestVal = moveVal;
                        }
                    }
                }
            }
            return bestMove;
        }

        public int ComputerMove()
        {
            if (rowCrossed() || columnCrossed() || diagonalCrossed())
            {
                return 1;
            }
            if (!isMovesLeft(board))
            {
                return 0;//draw
            }
            int x = -1;
            int y = -1;

            /*  int random = randGame.Next() % 8;
              if (random == 0)
              {
                  var rnd = new Random();
                  int k = rnd.Next() % 9;
                  x = k / 3;
                  y = k % 3;
                  while (board[x, y] != "")
                  {
                      k = rnd.Next() % 9;
                      x = k / 3;
                      y = k % 3;
                  }
              }
              else*/
            {
                var move = findBestMove(board);
                x = move.row;
                y = move.col;
            }
            board[x, y] = "O";
            if (!isMovesLeft(board))
            {
                return 0;//draw
            }
            if (rowCrossed() || columnCrossed() || diagonalCrossed())
            {
                return 2;//Comp win
            }
            return -1;
        }
    }
}
