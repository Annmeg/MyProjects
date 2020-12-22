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
        public event EventHandler DragStart;
        public event EventHandler Dropped;

        public Cell(bool isboardcell, bool ismystone = true)
        {
            BorderColor = Color.Black;
            CornerRadius = 40;
            DragGestureRecognizer dragGesture = new DragGestureRecognizer();
            DropGestureRecognizer dropGesture = new DropGestureRecognizer();

            dragGesture.DragStarting += (sender, args) =>
            {
                DragStart?.Invoke(this, EventArgs.Empty);
            };
            dragGesture.CanDrag = true;
            GestureRecognizers.Add(dragGesture);

            dropGesture.Drop += (sender, args) =>
            {
                Dropped?.Invoke(this, EventArgs.Empty);
            };
            dropGesture.AllowDrop = true;
            GestureRecognizers.Add(dropGesture);

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
