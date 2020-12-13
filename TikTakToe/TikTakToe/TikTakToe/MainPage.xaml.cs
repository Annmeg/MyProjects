using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TikTakToe
{
    public partial class MainPage : ContentPage
    {
        const int CellSpacing = 2;

        // Calculated during SizeChanged event 
        int cols;
        int rows;
        int cellSize;
        int xMargin;
        int yMargin;
        Grid grid = new Grid();

        public MainPage()
        {
            InitializeComponent();
        }
        void OnLayoutSizeChanged(object sender, EventArgs args)
        {
            Layout layout = sender as Layout;
            cols = 3;
            rows = 3;
            cellSize = (int)Math.Min(layout.Width / cols, layout.Height / rows);
            xMargin = (int)((layout.Width - cols * cellSize) / 2);
            yMargin = (int)((layout.Height - rows * cellSize) / 2);
            
            if (cols > 0 && rows > 0)
            {
                grid.SetSize(cols, rows);
                UpdateLayout();
            }
        }
        async void OnTapGestureTapped(object sender, EventArgs args)
        {
            Cell cell = (Cell)sender;
            cell.IsEmpty = false;
            grid.SetStatus(cell.Row, cell.Col, "X");
            UpdateView();
            var res = grid.ComputerMove();
            UpdateView();
            if (res == 0 || res == 1 || res == 2)
            {
                string game_result = res == 0 ? "Draw!" : res == 1 ? "You win!" : "You lost!";
                await DisplayAlert(game_result, "Game Over!", "Start new game?");
                grid.Clear();
                UpdateView();
                return;
            }
        }

        void UpdateView()
        {
            foreach (View view in absoluteLayout.Children)
            {
                Cell cell = view as Cell;
                cell.IsEmpty = grid.IsEmpty(cell.Row, cell.Col);
                cell.Value = grid.GetCellValue(cell.Row, cell.Col);
            }
        }
        void UpdateLayout()
        {
            int count = rows * cols;

            System.Diagnostics.Debug.WriteLine("Count = " + count);

            while (absoluteLayout.Children.Count < count)
            {
                Cell cell = new Cell();
                cell.Tapped += OnTapGestureTapped;
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

    }
}
