using GamesClient.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GamesClient
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private void G2048_Clicked(object sender, EventArgs e)
        {

        }
        private async void TTT_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TictactoePage());
        }

        private async void Bgm_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BackgammonPage());
        }
    }
}
