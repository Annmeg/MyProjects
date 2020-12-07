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
using System.Collections.ObjectModel;
using TikTak.Models;
using TikTak.Utility;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TikTak
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ObservableCollection<Cell> Cells;
        private static Util utility = new Util();

        public MainPage()
        {
            this.InitializeComponent();
            Cells = Board.GetBoard();
            Board.Initialize();
        }
        private void MyBoard_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as Cell;
            item.val = "X";
            item.color = "Red";
            item.OnPropertyChanged("val");
            item.OnPropertyChanged("color");
            var res = Board.Move(item.x, item.y);
            if (res == -1)
                return;
            if(res==0)
                result.Text = "Draw!";
             else if (res==1)
                result.Text = "You win!";
            else if (res==2)
                result.Text = "You lost!";
            myFlyout.ShowAt(myBoard);     
        }

        private void Inner_Click(object sender, RoutedEventArgs e)
        {
            myFlyout.Hide();
            Board.Reset();
            result.Text = "";
        }
    }
}
