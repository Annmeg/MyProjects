using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Game2048X
{
    class Cell : Label 
    {
        public event EventHandler Swiped;
        string val;
        public Cell()
        {
            BackgroundColor = Color.White;
            HorizontalTextAlignment = TextAlignment.Center;
            VerticalTextAlignment = TextAlignment.Center;
            FontSize = 28;
            val = "";
            var leftSwipeGesture = new SwipeGestureRecognizer { Direction = SwipeDirection.Left };
            leftSwipeGesture.Swiped += (sender, args) =>
            {
                Swiped?.Invoke(this, args);
            };
            var rightSwipeGesture = new SwipeGestureRecognizer { Direction = SwipeDirection.Right };
            rightSwipeGesture.Swiped += (sender, args) =>
            {
                Swiped?.Invoke(this, args);
            };
            var upSwipeGesture = new SwipeGestureRecognizer { Direction = SwipeDirection.Up };
            upSwipeGesture.Swiped += (sender, args) =>
            {
                Swiped?.Invoke(this, args);
            };
            var downSwipeGesture = new SwipeGestureRecognizer { Direction = SwipeDirection.Down };
            downSwipeGesture.Swiped += (sender, args) =>
            {
                Swiped?.Invoke(this, args);
            };
            GestureRecognizers.Add(leftSwipeGesture);
            GestureRecognizers.Add(rightSwipeGesture);
            GestureRecognizers.Add(upSwipeGesture);
            GestureRecognizers.Add(downSwipeGesture);
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
                    BackgroundColor = getColor(val);
                }
                else
                    BackgroundColor = Color.White;
            }
            get
            {
                return val;
            }
        }
        private Color getColor(string val)
        {
      
            switch (val)
            {
                case "2":
                    return Color.LightPink;
                case "4":
                    return Color.SeaShell;
                case "8":
                    return Color.Aqua;
                case "16":
                    return Color.LightGreen;
                case "32":
                    return Color.Fuchsia;
                case "64":
                    return Color.Coral;
                case "128":
                    return Color.Purple;
                case "256":
                    return Color.PowderBlue;
                case "512":
                    return Color.MediumSeaGreen;
                case "1024":
                    return Color.Yellow;
                case "2048":
                    return Color.Orange;
            }
            return Color.White;
        }

    }
}
