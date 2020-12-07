using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Game2048.Utils;

namespace Game2048.Models
{
    public class Cell : INotifyPropertyChanged
    {
        public int x { get; set; }
        public int y { get; set; }
        public string col { get; set; }

        public string val { get; set; }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void OnPropertyChanged(string propertyName = null)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Board
    {
        public static bool isgameover { get; set; }
        private static Utility utility = new Utility();

        private static ObservableCollection<Cell> Cells = new ObservableCollection<Cell>();
        public static ObservableCollection<Cell> GetBoard()
        {
            var a = new Cell();
            a.x = 0;
            a.y = 0;
            a.val = "";
            Cells.Add(a);
            Cells.Add(new Cell { x = 0, y = 1, val = "" });
            Cells.Add(new Cell { x = 0, y = 2, val = "" });
            Cells.Add(new Cell { x = 0, y = 3, val = "" });
            Cells.Add(new Cell { x = 1, y = 0, val = "" });
            Cells.Add(new Cell { x = 1, y = 1, val = "" });
            Cells.Add(new Cell { x = 1, y = 2, val = "" });
            Cells.Add(new Cell { x = 1, y = 3, val = "" });
            Cells.Add(new Cell { x = 2, y = 0, val = "" });
            Cells.Add(new Cell { x = 2, y = 1, val = "" });
            Cells.Add(new Cell { x = 2, y = 2, val = "" });
            Cells.Add(new Cell { x = 2, y = 3, val = "" });
            Cells.Add(new Cell { x = 3, y = 0, val = "" });
            Cells.Add(new Cell { x = 3, y = 1, val = "" });
            Cells.Add(new Cell { x = 3, y = 2, val = "" });
            Cells.Add(new Cell { x = 3, y = 3, val = "" });
            foreach (var c in Cells)
                c.col = "RosyBrown";
            return Cells;
        }
        public static void ResetBoard()
        {
            foreach (var c in Cells)
            {
                c.col = "RosyBrown";
                c.val = "";
                c.OnPropertyChanged("val");
                c.OnPropertyChanged("col");
            }
            Spawn();
            utility.score = 0;
        }

        static void copycells(int[,] a)
        {
            int i = 0;
            for (int k = 0; k < Cells.Count();)
            {
                for (int j = 0; j < 4; j++)
                {
                    var str = Cells.ElementAt(k++).val;
                    if (str.Length == 0)
                        a[i, j] = 0;
                    else
                        a[i, j] = Int32.Parse(str);
                }
                i++;
            }
        }

        public static void UpdateBoard(string control)
        {
            int[,] a = new int[4,4];
            copycells(a);
            utility.update_board(control,a);
            utility.fill_space(control,a);
            utility.spawn(a);

            int i = 0;
            for (int k = 0; k < Cells.Count();)
            {
                for (int j = 0; j < 4; j++, k++)
                {
                    if (a[i, j] == 0)
                    {
                        Cells.ElementAt(k).val = "";
                        Cells.ElementAt(k).col = "RosyBrown";
                    }
                    else
                    {
                        Cells.ElementAt(k).val = a[i, j].ToString();
                        Cells.ElementAt(k).col = utility.color_change(Cells.ElementAt(k).val);

                    }
                    Cells[k].OnPropertyChanged("val");
                    Cells[k].OnPropertyChanged("col");
                }
                i++;
            }
            isgameover=utility.isGameOver(a);
        }

        public static void Spawn()
        {
            var rand = new Random();
            int idx = rand.Next() % 16;
            Cells[idx].val = "2";
            Cells[idx].col = "LightBlue";
            Cells[idx].OnPropertyChanged("val");
            Cells[idx].OnPropertyChanged("col");
            idx = rand.Next() % 16;
            while (Cells[idx].val.Length != 0)
            {
                idx = rand.Next() % 16;
            }
            Cells[idx].val = "4";
            Cells[idx].col = "Blue";
            Cells[idx].OnPropertyChanged("val");
            Cells[idx].OnPropertyChanged("col");
        }
        public static string GetScore()
        {
            int tmp = (int)utility.score;
            return "Score: "+ tmp.ToString();
        }
        public static string GetBest()
        {
            if(utility.plus < utility.score)
            {
                utility.plus = utility.score;
            }
            int tmp = (int)utility.plus;
            return "Best: " + tmp.ToString();
        }

    }
}

