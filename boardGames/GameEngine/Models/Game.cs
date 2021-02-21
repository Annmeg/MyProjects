using GameEngine.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameEngine.Models
{
    public class GameBoard
    {
        public readonly string botPlayer = "O";
        public readonly string humanPlayer = "X";

        public string[,] Board { get; set; }

        public string CurrentTurn = "X";

        public bool GameComplete => GetWinner() != null || IsADraw();
       
        public GameBoard()
        {
            Reset();
        }

        public void Reset()
        {
            Board = new string[3, 3];

            //Populate the Board with blank pieces
            for (int i = 0; i <= 2; i++)
            {
                for (int j = 0; j <= 2; j++)
                {
                    Board[i, j] = "";
                }
            }
        }

        //Given the coordinates of the space that was clicked...
        public bool PieceClicked(int x, int y)
        {
            //If the game is complete, do nothing
            // if (GameComplete) { return; }

            //If the space is not already claimed...
            var clickedSpace = Board[x, y];
            if (clickedSpace == "")
            {
                //Set the marker to the current turn marker (X or O), then make it the other player's turn
                Board[x, y] = "X";// CurrentTurn;
                
                var move = findBestMove();
                if (move.row == -1 && move.col == -1)
                    return true;

                Board[move.row, move.col] = "O";
                //SwitchTurns();
                return true;
            }
            return false;
        }

        private void SwitchTurns()
        {
            //This is equivalent to: if currently X's turn, 
            // make it O's turn, and vice-versa
           // CurrentTurn = CurrentTurn == "X" ? "O" : "X";
        }

        public bool IsADraw()
        {
            int pieceBlankCount = 0;

            //Count all the blank spaces. If the count is 0, this is a draw.
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    pieceBlankCount = this.Board[i, j] == ""
                                        ? pieceBlankCount + 1
                                        : pieceBlankCount;
                }
            }

            return pieceBlankCount == 0;
        }

        public WinningPlay GetWinner()
        {
            WinningPlay winningPlay = null;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    foreach (EvaluationDirection evalDirection in (EvaluationDirection[])Enum.GetValues(typeof(EvaluationDirection)))
                    {
                        winningPlay = EvaluatePieceForWinner(i, j, evalDirection);
                        if (winningPlay != null) { return winningPlay; }
                    }
                }
            }

            return winningPlay;

        }

        private WinningPlay EvaluatePieceForWinner(int i, int j, EvaluationDirection dir)
        {
            var currentPiece = Board[i, j];
            if (currentPiece == "")
            {
                return null;
            }

            int inARow = 1;
            int iNext = i;
            int jNext = j;

            var winningMoves = new List<string>();

            while (inARow < 3)
            {
                switch (dir)
                {
                    case EvaluationDirection.Up:
                        jNext -= 1;
                        break;
                    case EvaluationDirection.UpRight:
                        iNext += 1;
                        jNext -= 1;
                        break;
                    case EvaluationDirection.Right:
                        iNext += 1;
                        break;
                    case EvaluationDirection.DownRight:
                        iNext += 1;
                        jNext += 1;
                        break;
                }
                if (iNext < 0 || iNext >= 3 || jNext < 0 || jNext >= 3) { break; }
                if (Board[iNext, jNext] == currentPiece)
                {
                    winningMoves.Add($"{iNext},{jNext}");
                    inARow++;
                }
                else
                {
                    return null;
                }
            }

            if (inARow >= 3)
            {
                winningMoves.Add($"{i},{j}");

                return new WinningPlay()
                {
                    WinningMoves = winningMoves,
                   // WinningStyle = currentPiece,
                    WinningDirection = dir,
                };
            }

            return null;
        }

        public string GetGameCompleteMessage()
        {
            var winningPlay = GetWinner();
            return winningPlay != null ? $"{winningPlay.WinningStyle} Wins!" : "Draw!";
        }

        public bool IsGamePieceAWinningPiece(int i, int j)
        {
            var winningPlay = GetWinner();
            return winningPlay?.WinningMoves?.Contains($"{i},{j}") ?? false;
        }
        public bool IsWon(string[] board, string player)
        {
            if (
                   (board[0] == player && board[1] == player && board[2] == player) ||
                   (board[3] == player && board[4] == player && board[5] == player) ||
                   (board[6] == player && board[7] == player && board[8] == player) ||
                   (board[0] == player && board[3] == player && board[6] == player) ||
                   (board[1] == player && board[4] == player && board[7] == player) ||
                   (board[2] == player && board[5] == player && board[8] == player) ||
                   (board[0] == player && board[4] == player && board[8] == player) ||
                   (board[2] == player && board[4] == player && board[6] == player)
                   )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsPlayed(string input)
        {
            return input == "X" || input == "O";
        }
        public string[] GetAvailableSpots(string[] board)
        {
            return board.Where(i => !IsPlayed(i)).ToArray();
        }

        public Move findBestMove()
        {
            int bestVal = -1000;
            Move bestMove = new Move();
            bestMove.row = -1;
            bestMove.col = -1;
            var temp = Board;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    // Check if cell is empty 
                    if (temp[i, j] == "")
                    {
                        // Make the move 
                        temp[i, j]= "O";

                        // compute evaluation function for this 
                        // move. 
                        int moveVal = minimax(temp, 0, false);

                        // Undo the move 
                        temp[i, j] = "";

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
    }
    public class Move
    {
        public int row { get; set; }
        public int col { get; set; }

    }
}

