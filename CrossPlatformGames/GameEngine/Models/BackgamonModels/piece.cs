using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GameEngine.Models
{
   public class piece : Frame
    {
        public event EventHandler Tapped;
        public int row { get; set; }
        public int col { get; set; }
        public piece(int x, int y)
        {
            row = x;
            col = y;
            BorderColor = Color.Black;
            CornerRadius = 40;
            var tapgesture = new TapGestureRecognizer();
            tapgesture.Tapped += (sender, args) =>
            {
                Tapped?.Invoke(this, args);
            };
            GestureRecognizers.Add(tapgesture);
        }
    }
}
