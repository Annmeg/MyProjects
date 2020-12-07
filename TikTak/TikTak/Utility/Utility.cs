using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TikTak.Utility
{

    public class Util
    {
        public class Move
        {
            public int row { get; set; }
            public int col { get; set; }
        };
        char[,] board = new char[3, 3];
        Random randGame = new Random();

        public void initBoard()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                    board[i, j] = ' ';
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
                    board[i, 0] != ' ')
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
                    board[0, i] != ' ')
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
                board[0, 0] != ' ')
            {
                return true;
            }

            if (board[0, 2] == board[1, 1] &&
                board[1, 1] == board[2, 0] &&
                 board[0, 2] != ' ')
            {
                return true;
            }
            return (false);
        }

        //Computer turn 
        public void computer_move(int x, int y, out int idx, out int res)
        {
            board[x, y] = 'X';//apply the human move to board
            idx = -1;
            res = -1;
            if (rowCrossed() || columnCrossed() || diagonalCrossed())
            {
                res = 1;//human win
                return;
            }
            if (!isMovesLeft(board))
            {
                res = 0;
                return;//draw
            }
            int i, j;
            int random = randGame.Next() % 8;
            if (random == 0)
            {
                var rnd = new Random();
                int k = rnd.Next() % 9;
                i = k / 3;
                j = k % 3;
                while (board[i, j] != ' ')
                {
                    k = rnd.Next() % 9;
                    i = k / 3;
                    j = k % 3;
                }
            }
            else
            {
                var move = findBestMove(board);
                i = move.row;
                j = move.col;
            }
            
            board[i, j] = 'O';
            idx = i * 3 + j;
            if (!isMovesLeft(board))
            {
                res = 0;
                return;//draw
            }
            if (rowCrossed() || columnCrossed() || diagonalCrossed())
            {
                res = 2;//Comp win
            }
            return;
        }

        // This function returns true if there are moves 
        // remaining on the board. It returns false if 
        // there are no moves left to play. 
        bool isMovesLeft(char[,] b)
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (b[i, j] == ' ')
                        return true;
            return false;
        }

        int evaluate(char[,] b)
        {
            // Checking for Rows for X or O victory. 
            for (int row = 0; row < 3; row++)
            {
                if (b[row, 0] == b[row, 1] &&
                    b[row, 1] == b[row, 2])
                {
                    if (b[row, 0] == 'O')
                        return +10;
                    else if (b[row, 0] == 'X')
                        return -10;
                }
            }

            // Checking for Columns for X or O victory. 
            for (int col = 0; col < 3; col++)
            {
                if (b[0, col] == b[1, col] &&
                    b[1, col] == b[2, col])
                {
                    if (b[0, col] == 'O')
                        return +10;

                    else if (b[0, col] == 'X')
                        return -10;
                }
            }

            // Checking for Diagonals for X or O victory. 
            if (b[0, 0] == b[1, 1] && b[1, 1] == b[2, 2])
            {
                if (b[0, 0] == 'O')
                    return +10;
                else if (b[0, 0] == 'X')
                    return -10;
            }

            if (b[0, 2] == b[1, 1] && b[1, 1] == b[2, 0])
            {
                if (b[0, 2] == 'O')
                    return +10;
                else if (b[0, 2] == 'X')
                    return -10;
            }

            // Else if none of them have won then return 0 
            return 0;
        }

        // This is the minimax function. It considers all 
        // the possible ways the game can go and returns 
        // the value of the board 
        int minimax(char[,] b, int depth, bool isMax)
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
                        if (b[i, j] == ' ')
                        {
                            b[i, j] = 'O';
                            best = Math.Max(best,
                                minimax(board, depth + 1, !isMax));
                            board[i, j] = ' ';
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
                        if (b[i, j] == ' ')
                        {
                            b[i, j] = 'X';
                            best = Math.Min(best,
                                minimax(board, depth + 1, !isMax));
                            board[i, j] = ' ';
                        }
                    }
                }
                return best;
            }
        }
        Move findBestMove(char[,] board)
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
                    if (board[i, j] == ' ')
                    {
                        // Make the move 
                        board[i, j] = 'O';

                        // compute evaluation function for this 
                        // move. 
                        int moveVal = minimax(board, 0, false);

                        // Undo the move 
                        board[i, j] = ' ';

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
    }
}