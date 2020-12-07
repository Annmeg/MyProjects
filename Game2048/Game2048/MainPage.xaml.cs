using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Game2048.Models;
using Game2048.Utils;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Windows.UI.Popups;
using System.Threading.Tasks;



// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Game2048
{
   
    public sealed partial class MainPage : Page
    {
        private ObservableCollection<Cell> Cells;
        private int cx, cy;// for swipe implementation
        public string score { get; set; }
        public string plus { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
            Cells = Board.GetBoard();
            Board.Spawn();
            plus = Board.GetBest();
            myscore.Text = "Score:\n";
            mybest.Text = "Best:\n";
        }
        
        private void Grid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            var pt = e.GetCurrentPoint(myBoard).Position;
        }

        private void Grid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var pt = e.GetCurrentPoint(myBoard).Position;
            cx = (int)pt.X / 40;
            cy = (int)pt.Y / 40;
        }
        
        private void Grid_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            var dir = e.Key;
            string control = dir.ToString();
            Board.UpdateBoard(control);
            
            myscore.Text=  Board.GetScore();
            mybest.Text = Board.GetBest();
            if (Board.isgameover)
            {
                myBoard.IsEnabled = false;
                myFlyout.ShowAt(myBoard);
                return;
            }
        }
        private void RestartBoard()
        {
            myBoard.IsEnabled = true;
            myscore.Text = "Score:\n";
            Board.ResetBoard();
        }
        private void Button_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            var dir = e.Key;
            string control = dir.ToString();
            Board.UpdateBoard(control);
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            RestartBoard();
        // item as GridViewItem;    
        //    item.Focus(FocusState.Programmatic);
        }

        private void Inner_Click(object sender, RoutedEventArgs e)
        {
            myFlyout.Hide();
        }
        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = (Cell)e.ClickedItem;
        }
    }

}

