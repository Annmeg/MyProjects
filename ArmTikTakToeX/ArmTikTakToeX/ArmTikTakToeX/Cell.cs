using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms.Shapes;

using Xamarin.Forms;
using System.Windows.Input;

namespace ArmTikTakToeX
{
    class Cell : Frame
    {
        public bool IsMyStone { get; set; }
        public bool IsBoardCell { get; set; }

        public event EventHandler Tapped;

        public Cell(bool isboardcell, bool ismystone = true)
        {
            BorderColor = Color.Black;
            CornerRadius = 40;
            TapGestureRecognizer tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += (sender, args) =>
            {
                Tapped?.Invoke(this, EventArgs.Empty);
            };
            GestureRecognizers.Add(tapGesture);
            IsBoardCell = isboardcell;
            if (!isboardcell)
            {
                IsMyStone = ismystone;
                if (ismystone)
                    BackgroundColor = Color.Red;
                else
                    BackgroundColor = Color.Blue;
            }
        }
        public int Row { get; set; }
        public int Col { get; set; }
    }
}
