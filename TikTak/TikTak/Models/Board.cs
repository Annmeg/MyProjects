using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using TikTak.Utility;

namespace TikTak.Models
{
    public class Cell : INotifyPropertyChanged
    {
        public int x { get; set; }
        public int y { get; set; }
        public string val { get; set; }
        public string color { get; set; }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void OnPropertyChanged(string propertyName = null)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
       
    }
    public class Board
    {
        private static List<int> ComputerMove { get; set; }
        private static Util utility = new Util();
        
        private static ObservableCollection<Cell> Cells = new ObservableCollection<Cell>();
        public static ObservableCollection<Cell> GetBoard()
        {
            Cells.Add(new Cell { x = 0, y = 0, val = "" });
            Cells.Add(new Cell { x = 0, y = 1, val = "" });
            Cells.Add(new Cell { x = 0, y = 2, val = "" });
            Cells.Add(new Cell { x = 1, y = 0, val = "" });
            Cells.Add(new Cell { x = 1, y = 1, val = "" });
            Cells.Add(new Cell { x = 1, y = 2, val = "" });
            Cells.Add(new Cell { x = 2, y = 0, val = "" });
            Cells.Add(new Cell { x = 2, y = 1, val = "" });
            Cells.Add(new Cell { x = 2, y = 2, val = "" });
            foreach (var c in Cells)
                c.color = "Red";

            return Cells;
        }
        public static void Reset()
        {
            Initialize();
            foreach(var c in Cells)
            {
                c.val = "";
                c.OnPropertyChanged("val");
            }
        }
        public static void Initialize()
        {
            utility.initBoard();
        }
        public static int Move(int x, int y)
        {
            int idx=-1, result=-1;
            utility.computer_move(x, y, out idx, out result);
            if (idx != -1)
            {
                Cells[idx].val = "O";
                Cells[idx].color = "Blue";
                Cells[idx].OnPropertyChanged("val");
                Cells[idx].OnPropertyChanged("color");
            }
            return result;
        }
    }
}
