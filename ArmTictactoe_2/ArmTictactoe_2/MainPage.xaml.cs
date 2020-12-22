using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace ArmTictactoe_2
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {

            InitializeComponent();
            MainViewModel model = new MainViewModel();
        }
    }
}
