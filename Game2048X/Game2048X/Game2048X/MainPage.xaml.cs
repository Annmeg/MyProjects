using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Game2048X
{
    public partial class MainPage : ContentPage
    {
        const int CellSpacing = 2;

        int cols;
        int rows;
        int cellSize;
        int xMargin;
        int yMargin;
        bool isGameOver = false;
        Grid grid = new Grid();

        public MainPage()
        {
            InitializeComponent();
        }
        void OnLayoutSizeChanged(object sender, EventArgs args)
        {
            Layout layout = sender as Layout;
            cols = 4;
            rows = 4;
            cellSize = (int)Math.Min(layout.Width / cols, layout.Height / rows);
            xMargin = (int)((layout.Width - cols * cellSize) / 2);
            yMargin = (int)((layout.Height - rows * cellSize) / 2);

            if (cols > 0 && rows > 0)
            {
                grid.SetSize(cols, rows);
                UpdateLayout();
            }
            grid.InitSpawn();
            UpdateView();
        }
        void UpdateView()
        {
            bool isgameover = true;
            foreach (View view in absoluteLayout.Children)
            {
                Cell cell = view as Cell;
                cell.Value = grid.GetCellValue(cell.Row, cell.Col);
                if (cell.Value == "")
                    isgameover = false;
            }
            isGameOver = isgameover;
        }
       async void OnSwipeGestureSwiped(object sender, EventArgs args)
        {
            var e = args as SwipedEventArgs;
            Cell cell = (Cell)sender;
            int row = cell.Row;
            int col = cell.Col;
            switch (e.Direction)
            {
                case SwipeDirection.Left:
                    grid.SwipeLeft();
                    break;
                case SwipeDirection.Right:
                    grid.SwipeRight();
                    break;
                case SwipeDirection.Up:
                    grid.SwipeUp();
                    break;
                case SwipeDirection.Down:
                    grid.SwipeDown();
                    break;
            }
            //generate random number(2 or 4), put in random cell 
            grid.Spawn();
            UpdateView();
            if (grid.isWin)
            {
                await DisplayAlert("You win!", "Game Over!", "Ok");
                grid.Clear();
                UpdateView();
                return;
            }
            if (isGameOver)
            {
                await DisplayAlert("You lost!", "Game Over!", "Ok");
                grid.Clear();
                UpdateView();
                return;
            }
        }

         void UpdateLayout()
        {

            int count = rows * cols;

            System.Diagnostics.Debug.WriteLine("Count = " + count);

            while (absoluteLayout.Children.Count < count)
            {
                Cell cell = new Cell();
                cell.Swiped += OnSwipeGestureSwiped;
                absoluteLayout.Children.Add(cell);
            }

            int index = 0;

            for (int x = 0; x < cols; x++)
                for (int y = 0; y < rows; y++)
                {
                    Cell cell = cell = (Cell)absoluteLayout.Children[index];
                    cell.Col = x;
                    cell.Row = y;

                    Rectangle rect = new Rectangle(x * cellSize + xMargin + CellSpacing / 2,
                                                   y * cellSize + yMargin + CellSpacing / 2,
                                                   cellSize - CellSpacing,
                                                   cellSize - CellSpacing);
                    AbsoluteLayout.SetLayoutBounds(cell, rect);
                    index++;
                }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            grid.Clear();
            grid.InitSpawn();
            UpdateView();
        }
    }
}



