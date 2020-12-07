using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2048.Utils
{
    public class Utility
    {
        public bool iswin { get; set; }
        public double plus { get; set; }
        public double score { get; set; }
       
        public bool isGameOver(int[,] a)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (a[i, j] == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public string color_change(string val)
        {
            switch (val)
            {
                case "2":
                    return "LightBlue";
                case "4":
                    return "Blue";
                case "8":
                    return "Aqua";
                case "16":
                    return "LightGreen";
                case "32":
                    return "LawnGreen";
                case "64":
                    return "Red";
                case "128":
                    return "Purple";
                case "256":
                    return "Gold";
                case "512":
                    return "DarkRed";

            }
            return "RosyBrown";
        }

        public int random_index(int x, Random rand)
        {
            int index;

            index = rand.Next() % x + 0;

            return index;
        }

        public void spawn(int[,] board)
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
     
        public void update_board(string control, int[,] board)
        {
            switch (control)
            {
                case "Up":
                    for (int i = 0; i < 4; i++)
                        for (int j = 0; j < 3; j++)
                        {
                            if (board[j, i] != 0 && board[j, i] == board[j + 1, i])
                            {
                                board[j, i] += board[j + 1, i];
                                if (board[j, i] == 2048)
                                    iswin = true;
                                board[j + 1, i] = 0;
                                score += (Math.Log(board[j,i]) - 1) * board[j,i];
                            }
                        }
                    break;

                case "Down":
                    for (int i = 0; i < 4; i++)
                        for (int j = 3; j > 0; j--)
                        {
                            if (board[j, i] != 0 && board[j, i] == board[j - 1, i])
                            {
                                board[j, i] += board[j - 1, i];
                                if (board[i, j] == 2048)
                                    iswin = true;
                                board[j - 1, i] = 0;
                                score += (Math.Log(board[j,i]) - 1) * board[j,i];
                              }
                        }
                    break;

                case "Left":
                    for (int i = 0; i < 4; i++)
                        for (int j = 0; j < 3; j++)
                        {
                            if (board[i, j] != 0 && board[i, j] == board[i, j + 1])
                            {
                                board[i, j] += board[i, j + 1];
                                if (board[i, j] == 2048)
                                    iswin = true;
                                board[i, j + 1] = 0;
                                score += (Math.Log(board[i,j]) - 1) * board[i,j];
                             }
                        }
                    break;

                case "Right":
                    for (int i = 0; i < 4; i++)
                        for (int j = 3; j > 0; j--)
                        {
                            if (board[i, j] != 0 && board[i, j] == board[i, j - 1])
                            {
                                board[i, j] += board[i, j - 1];
                                if (board[i, j] == 2048)
                                    iswin = true;
                                board[i, j - 1] = 0;
                                score += (Math.Log(board[i, j]) - 1) * board[i, j];
                             }
                        }
                    break;
            }
            plus = Math.Max(plus, score);
        }

        public void fill_space(string control, int[,] board)
        {
            switch (control)
            {
                case "Up":
                    for (int i = 0; i < 4; i++)
                        for (int j = 0; j < 4; j++)
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
                    break;
                case "Down":
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
                    break;
                case "Left":
                    for (int i = 0; i < 4; i++)
                        for (int j = 0; j < 4; j++)
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
                    break;
                case "Right":
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
                    break;
            }
        }

    }
}
