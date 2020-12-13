using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TikTakToe
{
    class Cell:Label
    {
        bool isEmpty;
        string val;
        public event EventHandler Tapped;

        public Cell()
        {
            BackgroundColor = Color.White;
            HorizontalTextAlignment = TextAlignment.Center;
            VerticalTextAlignment = TextAlignment.Center;
            FontSize = 50;

            isEmpty = true;
            val = "";
            TapGestureRecognizer tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += (sender, args) =>
            {
                Tapped?.Invoke(this, EventArgs.Empty);
            };
            GestureRecognizers.Add(tapGesture);
        }
        public int Col { set; get; }
        public int Row { set; get; }
        public string Value
        {
            set
            {
                val = value;
                Text = val;
                if (val != "")
                {
                    TextColor = val == "X" ? Color.Red : Color.Blue;
                }
            }
            get
            {
                return val;
            }
        }
        public bool IsEmpty
        {
            set
            {
                if (isEmpty != value)
                {
                    isEmpty = value;
                    //   BackgroundColor = isEmpty ? Color.White : Color.PowderBlue;  
                }
            }
            get
            {
                return isEmpty;
            }
        }
    }

}
    
