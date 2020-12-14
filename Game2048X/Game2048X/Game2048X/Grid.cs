using System;
using System.Collections.Generic;
using System.Text;

namespace Game2048X
{
    class Grid
    {
        int cols = 4;
        int rows = 4;
       int[,] board = new int[4, 4];
      public  bool isWin = false;
        public void SetSize(int cols, int rows)
        {
            if (cols <= 0 || rows <= 0)
                throw new ArgumentException("Grid.SetSize: Arguments must be greater than zero");
            this.cols = cols;
            this.rows = rows;
        }
        public string GetCellValue(int x, int y)
        {
            if (board[x, y] == 0)
                return "";
            return board[x, y].ToString();
        }
        public void Clear()
        {
            for(int i = 0; i<rows; i++)
            {
                for(int j = 0; j < cols; j++)
                {
                    board[i, j] = 0;
                }
            }
        }
        public void InitSpawn()
        {
            var rand = new Random();
            int val = rand.Next()%16;
            int row = val / 4;
            int col = val % 4;
            board[row, col] = 2;
            val = rand.Next() % 16;
            row = val / 4;
            col = val % 4;
            board[row, col] = 4;
        }   
        private int random_index(int x, Random rand)
        {
            int index;

            index = rand.Next() % x + 0;

            return index;
        }

        public void Spawn()
        {
            int i, j, k;
            var rand = new System.Random();
            do
            {
                i = random_index(4, rand);
                j = random_index(4, rand);
                k = random_index(10, rand);

            } while (board[i, j] != 0);

            if (k < 2)
                board[i, j] = 4;

            else
                board[i, j] = 2;
        }
    
       public void SwipeLeft()
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] != 0 && board[i, j] == board[i, j + 1])
                    {
                        board[i, j] += board[i, j + 1];
                        if (board[i, j] == 2048)
                            isWin = true;
                        board[i, j + 1] = 0;
                        //score += (Math.Log(board[i, j]) - 1) * board[i, j];
                    }
                }
            //fill space
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == 0)
                    {
                        for (int k = j + 1; k < 4; k++)
                            if (board[i, k] != 0)
                            {
                                board[i, j] = board[i, k];
                                board[i, k] = 0;
                                break;
                            }
                    }
                }

        }
        public void SwipeRight()
        {
            for (int i = 0; i < 4; i++)
                for (int j = 3; j > 0; j--)
                {
                    if (board[i, j] != 0 && board[i, j] == board[i, j - 1])
                    {
                        board[i, j] += board[i, j - 1];
                        if (board[i, j] == 2048)
                            isWin = true;
                        board[i, j - 1] = 0;
                       // score += (Math.Log(board[i, j]) - 1) * board[i, j];
                    }
                }
            //fill space
            for (int i = 0; i < 4; i++)
                for (int j = 3; j >= 0; j--)
                {
                    if (board[i, j] == 0)
                    {
                        for (int k = j - 1; k >= 0; k--)
                            if (board[i, k] != 0)
                            {
                                board[i, j] = board[i, k];
                                board[i, k] = 0;
                                break;
                            }
                    }
                }
        }
        public void SwipeUp()
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 3; j++)
                {
                    if (board[j, i] != 0 && board[j, i] == board[j + 1, i])
                    {
                        board[j, i] += board[j + 1, i];
                        if (board[j, i] == 2048)
                            isWin = true;
                        board[j + 1, i] = 0;
                       // score += (Math.Log(board[j, i]) - 1) * board[j, i];
                    }
                }
            //fill space
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 3; j++)
                {
                    if (board[j, i] == 0)
                    {
                        for (int k = j + 1; k < 4; k++)
                        {
                            if (board[k, i] != 0)
                            {
                                board[j, i] = board[k, i];
                                board[k, i] = 0;
                                break;
                            }
                        }
                    }
                }
        }
        public void SwipeDown()
        {
            for (int i = 0; i < 4; i++)
                for (int j = 3; j > 0; j--)
                {
                    if (board[j, i] != 0 && board[j, i] == board[j - 1, i])
                    {
                        board[j, i] += board[j - 1, i];
                        if (board[i, j] == 2048)
                               isWin = true;
                            board[j - 1, i] = 0;
                        // score += (Math.Log(board[j, i]) - 1) * board[j, i];
                    }
                }
            //fill space
            for (int i = 0; i < 4; i++)
                for (int j = 3; j >= 0; j--)
                {
                    if (board[j, i] == 0)
                    {
                        for (int k = j - 1; k >= 0; k--)
                        {
                            if (board[k, i] != 0)
                            {
                                board[j, i] = board[k, i];
                                board[k, i] = 0;
                                break;
                            }
                        }
                    }
                }
        }
    }
}
