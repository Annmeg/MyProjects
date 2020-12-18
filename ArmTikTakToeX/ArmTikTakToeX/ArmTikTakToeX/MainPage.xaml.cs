using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace ArmTikTakToeX
{
    public partial class MainPage : ContentPage
    {
        const int CellSpacing = 4;
        //board cells
        int cols;
        int rows;
        int cellSize;
        int xMargin;
        int yMargin;
        double setsHeight;
        double layoutWidth;
        Board grid = new Board();
        Cell prev_cell;
        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            grid.Reset();
            ResetView();
        }
        void OnLayoutSizeChanged(object sender, EventArgs args)
        {
            grid.Reset();
            Layout layout = sender as Layout;
            cols = 3;
            rows = 3;
            prev_cell = null;
            double height = layout.Height;
            var boardHeight = height * 3 / 4;
            setsHeight = height / 4;
            layoutWidth = layout.Width;
            cellSize = (int)Math.Min(layout.Width / (1.5*cols), layout.Width / (1.5*rows));
            xMargin = (int)((layout.Width - cols * cellSize) / 2);
            yMargin = (int)((boardHeight - rows * cellSize) / 2);

            if (cols > 0 && rows > 0)
            {
                grid.SetSize(cols, rows);
                UpdateLayout();
            }
        }
        void UpdateLayout()
        {
            UpdateBoard();
            UpdateSets();
        }
        void UpdateBoard() 
        { 
            int count = rows * cols;
            System.Diagnostics.Debug.WriteLine("Count = " + count);

            while (absoluteLayout.Children.Count < count)
            {
                var cell = new Cell(true);
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

                   Xamarin.Forms.Rectangle rect = new Xamarin.Forms.Rectangle(x * cellSize + x*xMargin + CellSpacing / 2,
                                                   y * cellSize + y*yMargin + CellSpacing / 2,
                                                   cellSize - CellSpacing,
                                                   cellSize - CellSpacing);
                    AbsoluteLayout.SetLayoutBounds(cell, rect);
                    index++;
                }
          
        }
        void UpdateSets()
        {
            for(int i = 0; i < grid.mystones; i++)
            {
                var cell = new Cell(false,true);//not board cell, my stone
                cell.Tapped += OnTapGestureTapped;
                absoluteLayout.Children.Add(cell);
            }
            for (int i = 0; i < grid.opstones; i++)
            {
                var cell = new Cell(false,false);//not board cell, not my stone
                absoluteLayout.Children.Add(cell);
            }
            int index = cols * rows;
            int col1 = 0;
            int col2 = (int)layoutWidth - cellSize;//for opponent stones
            int toprow = rows * cellSize + rows * yMargin + CellSpacing;//top row for stones sets
            for (int x = 0; x < grid.mystones; x++) { 
                    var cell = (Cell)absoluteLayout.Children[index];

                Xamarin.Forms.Rectangle rect = new Xamarin.Forms.Rectangle(col1 + CellSpacing / 2,
                                                toprow + x * cellSize / 3,
                                                cellSize - CellSpacing,
                                                cellSize - CellSpacing);
                    AbsoluteLayout.SetLayoutBounds(cell, rect);
                    index++;
                }
            for (int x = 0; x < grid.opstones; x++)
            {
                Cell cell = (Cell)absoluteLayout.Children[index];

                Xamarin.Forms.Rectangle rect = new Xamarin.Forms.Rectangle(col2 + CellSpacing / 2,
                                                 toprow + x * cellSize / 3,
                                                 cellSize - CellSpacing,
                                                cellSize - CellSpacing);
                AbsoluteLayout.SetLayoutBounds(cell, rect);
                index++;
            }
        }
        void ResetView()
        {
            int count = cols * rows;
            int idx = 0;
            grid.mystones = 3;
            grid.opstones = 3;
            foreach (View view in absoluteLayout.Children)
            {
                var cell = view as Cell;
                if(cell.IsBoardCell)
                    cell.BackgroundColor = grid.GetColor(cell.Row, cell.Col);
                else
                {
                    cell.BackgroundColor = cell.IsMyStone ? Color.Red : Color.Blue;
                }
                idx++;
            }
        }
        int CompMoveStone()
        {
            int res = grid.movestone();
            grid.opstones--;
            return res;
        }
        void UpdateView() {
            int count = cols * rows;
            int idx = 0;
            int k = 0;
            foreach (View view in absoluteLayout.Children)
            {
                var cell = view as Cell;
                if (cell.IsBoardCell)
                {
                    cell.BackgroundColor = grid.GetColor(cell.Row, cell.Col);
                }
                else
                {
                    if (k < 3)
                    {
                        if (k < 3 - grid.mystones)
                        {
                            cell.BackgroundColor = Color.PowderBlue;
                            cell.BorderColor = Color.PowderBlue;
                        }
                    }
                    else
                    {
                        if (k - 3 < 3 - grid.opstones)
                        {
                            cell.BackgroundColor = Color.PowderBlue;
                            cell.BorderColor = Color.PowderBlue;
                        }
                    }
                    k++;
                }
                idx++;
            }
        }
       async void OnTapGestureTapped(object sender, EventArgs args)
        {
            var cell = sender as Cell;
            if (!cell.IsBoardCell)
            {
                prev_cell = cell;
            }
            else {
                if (grid.mystones > 0)
                {
                    if (prev_cell != null)
                    {
                        grid.mystones--;
                        grid.SetStatus(cell.Row, cell.Col, false);
                        prev_cell = null;
                        UpdateView();
                        int res = CompMoveStone();
                        if (res == 1)
                        {
                            await DisplayAlert("You win!", "Game Over!", "Start new game? ");
                            grid.Reset();
                            ResetView();
                        }
                        else
                        {
                            UpdateView();
                            if (res == 2)
                            {
                                await DisplayAlert("You lost!", "Game Over!", "Start new game? ");
                                grid.Reset();
                                ResetView();
                            }
                        }
                    }
                }
            else
            {
                    if (prev_cell == null)
                    {
                        prev_cell = cell;
                        if (grid.GetColor(prev_cell.Row, prev_cell.Col) != Color.Red)
                        {
                            prev_cell = null;
                            return;
                        }
                    }
                    else
                    {
                        if (grid.GetColor(cell.Row, cell.Col) == Color.White)
                        {
                            if (Math.Abs(cell.Row - prev_cell.Row) <= 1 &&
                                 Math.Abs(cell.Col - prev_cell.Col) <= 1)
                            {
                                if (cell.Col == prev_cell.Col ||
                                    cell.Row == prev_cell.Row ||
                                    (cell.Row == cell.Col && prev_cell.Row == prev_cell.Col)
                                    || (cell.Row + cell.Col == prev_cell.Row + prev_cell.Col))
                                {
                                    grid.SetStatus(prev_cell.Row, prev_cell.Col, true);
                                    grid.SetStatus(cell.Row, cell.Col, false);
                                    UpdateView();
                                    CompBoardMove();
                                    prev_cell = null;
                                }
                            }
                        }
                    }
                }
            }
        }
      async  void CompBoardMove()
        {
            int x1 = -1;
            int y1 = -1;
            int x2 = -1;
            int y2 = -1;
            int res = -1;
            int time = 10000;
            while (x2 == -1 && y2 == -1 && time-->0)
            {
               res = grid.selectSourceTarget(out x1, out y1, out x2, out y2);
            }
            if(res==1)
            {
                await DisplayAlert("You win!", "Game Over!", "Start new game? ");
                grid.Reset();
                ResetView();
                return;
            }
            grid.SetOpnStatus(x1, y1, true);
            grid.SetOpnStatus(x2, y2, false);
            UpdateView();
            if (res == 2)
            {
                await DisplayAlert("You lost!", "Game Over!", "Start new game? ");
                grid.Reset();
                ResetView();
            }
        }
    }
}
