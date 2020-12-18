using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArmTikTakToeX
{
    public partial class App : Application
    {
        public App()
        {
            Device.SetFlags(new string[] { "Shapes_Experimental", "DragAndDrop_Experimental" });
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
