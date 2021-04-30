using GamesClient.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GamesClient.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TictactoePage : ContentPage
    {
        const int CellSpacing = 2;
        // Calculated during SizeChanged event 
        int cols;
        int rows;
        int cellSize;
        int xMargin;
        int yMargin;

        int gameId = -1;
        string gameName = "tictactoe";
        int userId;
        int currentTurn = 0;

        GameEngine.Models.Grid grid = new GameEngine.Models.Grid();
        private IDataProvider data;

        public TictactoePage()
        {
            InitializeComponent();
            data = new TictactoeDataProvider();
            currentTurn = 1;
            current.Text = String.Format("Player {0} turn!", currentTurn);

        }
        async void Sync()
        {
            while (true)
            {
                if (CheckState())
                    return;
                var board = await data.GetBoardData(gameName, gameId, userId);
                if (board != null && (board.CX != -1 && board.CY != -1) && grid.IsEmpty(board.CX, board.CY))
                {
                    grid.SetStatus(board.CX, board.CY, board.CurrentTurn == 1 ? "O" : "X");
                    currentTurn = userId;
                    current.Text = String.Format("Player {0} turn.",
                         currentTurn);
                    UpdateView();
                }
            }
        }
        async void OnLayoutSizeChanged(object sender, EventArgs args)
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
                var board = await data.CreateBoard(gameId);
                if (board.UsersCount == 1)
                {
                    userId = 1;
                    currentTurn = 1;
                }
                else
                {
                    currentTurn = 1;
                    userId = 2;
                }
                gameId = board.Id;

                Sync();
            }
        }
        bool CheckState()
        {
            var res = grid.CheckGameState();
            //  var res = grid.ComputerMove();
            // UpdateView();
            if (res.Length > 0)
            {
                newGame.IsEnabled = true;
                current.IsEnabled = false;
                UpdateView();
                DisplayAlert(res, "Game Over!", "Ok");

                return true;
            }
            return false;
        }
        async void OnTapGestureTapped(object sender, EventArgs args)
        {
            if (userId != currentTurn)
                return;
            GameEngine.Models.Cell cell = (GameEngine.Models.Cell)sender;
            if (!cell.IsEmpty)
                return;

            var board = await data.UpdateBoard(gameName, cell.Row, cell.Col, gameId, userId);
            grid.SetStatus(cell.Row, cell.Col, userId == 1 ? "X" : "O");
            cell.IsEmpty = false;
            currentTurn = board.CurrentTurn;
            current.Text = String.Format("Player {0} turn.",
                         currentTurn);

            UpdateView();
            CheckState();
        }

        void UpdateView()
        {
            foreach (View view in absoluteLayout.Children)
            {
                GameEngine.Models.Cell cell = view as GameEngine.Models.Cell;
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
                GameEngine.Models.Cell cell = new GameEngine.Models.Cell();
                cell.Tapped += OnTapGestureTapped;
                absoluteLayout.Children.Add(cell);
            }

            int index = 0;

            for (int x = 0; x < cols; x++)
                for (int y = 0; y < rows; y++)
                {
                    GameEngine.Models.Cell cell = cell = (GameEngine.Models.Cell)absoluteLayout.Children[index];
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

        private async void newGame_Clicked(object sender, EventArgs e)
        {
            grid.Clear();
            currentTurn = 1;
            current.IsEnabled = true;
            current.Text = String.Format("Player {0} turn.",
                         currentTurn);
            current.Text = String.Format("Player {0} turn.",
                     currentTurn);

            newGame.IsEnabled = false;
            await data.ReserBoard(gameName, gameId);
            UpdateView();
            Sync();
        }
    }
}