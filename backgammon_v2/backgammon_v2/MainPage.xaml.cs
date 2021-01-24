using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace backgammon_v2
{
    public partial class MainPage : ContentPage
    {
        double width;
        double height;
        board backgammon;
        int n1, n2;
        int cnt;
        int steps = 2;
        double cellsize;
        Color current;
        string[] dies = new string[6];
        bool firsttime = true;
        dice _dice;
        public MainPage()
        {
            InitializeComponent();
            backgammon = new board();
            // Get Display dimensions using Xamarin.Essentials
            current = Color.White;
            dies[0] = "die1_white.png";
            dies[1] = "die2_white.png";
            dies[2] = "die3_white.png";
            dies[3] = "die4_white.png";
            dies[4] = "die5_white.png";
            dies[5] = "die6_white.png";
            InitializeDice();
        }

        private void OnDiceRolled(object sender, EventArgs e)
        {
            roll.IsEnabled = true;
        }

        private void buildBoard()
        {
            var center = width / 2;
            int blockSize = (int)(width) / 14;
            var colors = new List<Brush>()
            {
                Brush.LightGray,//FromHex("#E8E9B7"),
                Brush.LightSlateGray//FromHex("#7A8992"),
            };
            var bar = new Xamarin.Forms.Shapes.Polygon()
            {
                Points = new PointCollection()
                {
                    new Point(center - 5, 0),
                    new Point(center + 5, 0),
                    new Point(center + 5, height-2),
                    new Point(center - 5, height-2)
                },
                Fill = Brush.Maroon// "#6D657E"
            };
            var xPos = 2;
            var yPos = 0;
            int k = 0;
            List<Brush> rowColors = colors.GetRange(0, colors.Count);
            while (k < 12)
            {
                if (k++ == 6)
                    xPos += 10;
                rowColors = reorderColors(rowColors);
                var b = new block(new Thickness(xPos, yPos + 5), blockSize,
                    rowColors[0], rowColors[1], 0, k);
                backgroundLayout.Children.Add(b);
                xPos += blockSize + 5;
            }
            colors = reorderColors(colors);
            xPos = 2;
            yPos = (int)height - 100;
            k = 0;
            while (k < 12)
            {
                if (k++ == 6)
                    xPos += 10;
                rowColors = reorderColors(rowColors);
                var b = new block(new Thickness(xPos, yPos), blockSize,
                    rowColors[0], rowColors[1], 1, k);
                backgroundLayout.Children.Add(b);
                xPos += blockSize + 5;
            }
            backgroundLayout.Children.Add(bar);
        }

        private void fillBoard()
        {
            var rad = 30;
            int j = 0;
            double xpos = 15 + cellsize * 11;
            for (; j < 12; j++)
            {
                int whites = backgammon.getWhites(0, j);
                int blacks = backgammon.getBlacks(0, j);
                int cnt = whites > 0 ? whites : blacks;
                if (j == 6)
                    xpos -= 10;
                int k = 0;
                int shift = cnt < 6 ? 20 : 100 / cnt;
                var color = whites > 0 ? Color.White : Color.Aqua;
                while (k < cnt)
                {
                    var b = new piece(0, j);
                    b.BackgroundColor = color;
                    b.Tapped += OnStoneTapped;
                    backgroundLayout.Children.Add(b);
                    var rect = new Xamarin.Forms.Rectangle(xpos - cellsize * j, k * shift,
                         rad, rad);
                    k++;
                    AbsoluteLayout.SetLayoutBounds(b, rect);
                }
            }
            xpos = 4;
            var ypos = height - 25;
            for (j = 0; j < 12; j++)
            {
                int whites = backgammon.getWhites(1, j);
                int blacks = backgammon.getBlacks(1, j);
                int cnt = whites > 0 ? whites : blacks;
                int k = 0;
                int shift = cnt < 6 ? 20 : 100 / cnt;
                var color = whites > 0 ? Color.White : Color.Aqua;

                if (j == 6)
                    xpos += 10;

                while (k < cnt)
                {
                    var b = new piece(1, j);
                    b.BackgroundColor = color;
                    b.Tapped += OnStoneTapped;
                    backgroundLayout.Children.Add(b);

                    var rect = new Xamarin.Forms.Rectangle(cellsize * j + xpos, ypos - k * shift,
                         rad, rad);
                    k++;
                    AbsoluteLayout.SetLayoutBounds(b, rect);
                }
            }
            var pendingWhite = backgammon.getWhitePending();
            for (int i = 0; i < pendingWhite; i++)
            {
                var b = new piece(-1, -1);
                b.BackgroundColor = Color.White;
                b.Tapped += OnStoneTapped;
                backgroundLayout.Children.Add(b);

                var rect = new Xamarin.Forms.Rectangle(width / 2 - 10, 100 + i * 10,
                     rad, rad);
                AbsoluteLayout.SetLayoutBounds(b, rect);
            }
            var pendingBlack = backgammon.getBlackPending();

            for (int i = 0; i < pendingBlack; i++)
            {
                var b = new piece(-1, -1);
                b.BackgroundColor = Color.Aqua;
                b.Tapped += OnStoneTapped;
                backgroundLayout.Children.Add(b);
                var rect = new Xamarin.Forms.Rectangle(width / 2 - 10, 200 + i * 10,
                     rad, rad);
                AbsoluteLayout.SetLayoutBounds(b, rect);
            }
        }
        public List<Brush> reorderColors(List<Brush> list)
        {
            var first = list.First();
            list.RemoveAt(0);
            list.Add(first);
            return list;
        }
        private void updateView()
        {
            backgroundLayout.Children.Clear();
            buildBoard();
            fillBoard();
            fillBars();
        }
        private void nextPlayer()
        {
            current = current == Color.White ? Color.Aqua : Color.White;
            roll.IsEnabled = true;
        }
        private void updateAfterMove()
        {
            cnt++;
            updateView();
            reorder.IsEnabled = false;
            if (cnt == steps)
            {
                hideDices();
                current = current == Color.White ? Color.Aqua : Color.White;
                roll.IsEnabled = true;
                var text = current == Color.White ? "Whites turn." : "Blacks turn.";
                reminder.Text = text + " Please roll dices.";
            }
        }
        private async void OnStoneTapped(object sender, EventArgs args)
        {
            var c = sender as piece;
            if (c.BackgroundColor != current)
                return;
            int n = cnt == 0 ? n1 : n2;

            int res = checkStuckorPending(n);
            if (res == 0)
            {
                //move pending piece from bar to row
                updateAfterMove();
                return;
            }
            if (cnt == 0 && res == 2)
            {
                //have stucked piece
                nextPlayer();
                return;
            }
            if (res == 1)
            {
                // cannot move pending piece from bar
                if (cnt > 0)
                {
                    //if it is not first stone no moves possible
                    nextPlayer();
                    return;
                }
                else
                {
                    //must reorder and try again
                    var text = current == Color.White ? "Whites turn." : "Blacks turn.";
                    if (cnt == 0)
                        reminder.Text = text + ", please try to reorder dices and move again.";
                    return;
                }
            }

            if (cnt == 0 && !backgammon.isMovePossible(current, n1)
                && !backgammon.isMovePossible(current, n2))
            {
                string cur = current == Color.White ? "White" : "Black";
                reminder.Text = cur + " has no move. Next player's turn";
                nextPlayer();
                return;
            }
            if (cnt > 0 && !backgammon.isMovePossible(current, n))
            {
                string cur = current == Color.White ? "White" : "Black";
                reminder.Text = cur + " has no move. Next player's turn";
                nextPlayer();
                return;
            }
            int row = c.row;
            int col = c.col;

            int index = row * 12 + col;
            if (row == -1 || col == -1)
                index = 0;
            if (current == Color.White)
            {
                var ret = await whiteTurn(index, n, cnt + 1);
                if (ret == false)
                {
                    if (!backgammon.isMovePossible(current, n))
                    {
                        if (cnt == 0)
                        {
                            reminder.Text = "Whites turn: please try to reorder dices and move again.";
                            return;
                        }
                        else
                        {
                            reminder.Text = "White has no move. Next player's turn";
                            nextPlayer();
                            return;
                        }
                    }
                }
            }
            else
            {
                var ret = await blackTurn(index, n, cnt + 1);
                if (ret == false)
                {
                    if (!backgammon.isMovePossible(current, n))
                    {

                        if (cnt == 0)
                        {
                            reminder.Text = "Blacks turn: please try to reorder dices and move again.";
                            return;
                        }
                        else
                        {
                            reminder.Text = "Black has no move. Next player's turn";
                            nextPlayer();
                            return;
                        }
                    }
                }
            }
            updateAfterMove();
        }
        private void onLayout_SizeChanged(object sender, EventArgs e)
        {
            var layout = sender as AbsoluteLayout;
            width = layout.Width;
            height = layout.Height;
            cellsize = (width - 15) / 12;
            cellsize = (width - 15) / 12;
            buildBoard();
            fillBoard();
            fillBars();
        }
        private void hideDices()
        {
            dice1_.Opacity = 0;
            dice2_.Opacity = 0;
            dice1.Opacity = 0;
            dice2.Opacity = 0;
        }
        private void showDices()
        {
            dice1.ImageSource = dies[n1 - 1];
            dice1.Scale = 0.1;
            dice1.Opacity = 1;
            dice2.ImageSource = dies[n2 - 1];
            dice2.Scale = 0.1;
            dice2.Opacity = 1;
        }

        private void InitializeDice()
        {
            _dice = new dice();
            _dice.RollingChanged += OnDiceRollingChanged;
            _dice.Rolled += OnDiceRolled;
        }

        private void OnDiceRollingChanged(object
              sender, EventArgs e)
        {
            hideDices();
            rollDices();
            showDices();
        }

        private void rollDices()
        {
            var r = new Random();
            n1 = r.Next() % 6 + 1;
            n2 = r.Next() % 6 + 1;
            if (!firsttime && n1 < n2)
            {
                var t = n1;
                n1 = n2;
                n2 = t;
            }
            cnt = 0;
            steps = 2;
            if (n1 == n2 && !firsttime)
                steps = 4;
        }
        private void roll_Clicked(object sender, EventArgs e)
        {
            cnt = 0;
            _dice.Roll();
            if (!firsttime)
            {
                if (n1 == n2)
                {
                    dice1_.ImageSource = dies[n1 - 1];
                    dice1_.Scale = 0.1;
                    dice2_.ImageSource = dies[n1 - 1];
                    dice2_.Scale = 0.1;
                    dice1_.Opacity = 1;
                    dice2_.Opacity = 1;
                }
            }
            reminder.Opacity = 1;
            if (firsttime == true)
            {
                firsttime = false;
                if (n1 == n2)
                {
                    reminder.Text = "Roll dices again to choose who plays first.";
                    roll.IsEnabled = true;
                    return;
                }
                if (n1 > n2)
                {
                    current = Color.White;
                }
                else
                {
                    current = Color.Aqua;
                }
            }
            reorder.IsEnabled = true;
            roll.IsEnabled = false;
            int n = n1;
            reminder.Text = current == Color.White ? "White's turn. Please choose piece to move." :
                "Black's turn. Please choose piece to move.";
        }
        private int checkStuckorPending(int move)
        {
            if (current == Color.White)
                return checkStuckorPendingWhite(move);
            else
                return checkStuckorPendingBlack(move);
        }
        private int checkStuckorPendingWhite(int move)
        {
            bool pending = backgammon.checkPendingWhite();

            if (pending)
            {
                int rowChoice = 25;
                bool stuck = backgammon.checkStuckWhite(n1, n2);
                if (stuck)
                {
                    reminder.Text = "Your piece is stuck on the bar. You have no possible moves, your turn is over.";
                    return 2;
                }
                int moved = rowChoice - move;
                int errorCheck = backgammon.movePieceWhitePending(moved);
                if (errorCheck == 0)
                {
                    //success
                    return 0;
                }
                else
                    //Cannot move on row 
                    return 1;
            }
            //no stuck or pending
            return -1;
        }
        private async Task<bool> whiteTurn(int index, int move, int turn)
        {
            int errorCheck = -1;
            if (move != 0)
            {
                int rowChoice = index;
                int moved = rowChoice - move;
                errorCheck = backgammon.movePieceWhite(rowChoice, moved);
                if (errorCheck == 0)
                {
                    if (backgammon.checkWhiteWin())
                    {
                        await DisplayAlert("Game over!", "Whites win!", "OK");
                        return true;
                    }
                }
                else
                    return false;
            }
            return true;
        }
        private int checkStuckorPendingBlack(int move)
        {
            bool pending = backgammon.checkPendingBlack();

            if (pending)
            {
                int rowChoice = 0;
                bool stuck = backgammon.checkStuckBlack(n1, n2);
                if (stuck)
                {
                    reminder.Text = "Blacks turn: Your piece is stuck on the bar.You have no possible moves, your turn is over.";
                    return 2;
                }
                if (move != 0)
                {
                    int moved = rowChoice + move;
                    int errorCheck = backgammon.movePieceBlackPending(moved);
                    if (errorCheck == 0)
                    {
                        return 0;
                    }
                    else
                        return 1;
                }
            }
            return -1;
        }

        private void reorder_Clicked(object sender, EventArgs e)
        {
            hideDices();
            var t = n1;
            n1 = n2;
            n2 = t;
            showDices();
        }
        private void fillBars()
        {
            if (current == Color.White)
                bar1.Children.Clear();
            else
                bar2.Children.Clear();
            int shift = 20;
            int cnt = current == Color.White ? backgammon.getWhiteTotal() : backgammon.getBlackTotal();
            if (cnt > 8)
                shift = 150 / cnt;
            for (int i = 0; i < cnt; i++)
            {
                var b = new piece(-1, -1);
                b.BackgroundColor = current;
                if (current == Color.White)
                    bar1.Children.Add(b);
                else
                    bar2.Children.Add(b);
                var rect = new Xamarin.Forms.Rectangle(5 + i * shift, 0, 30, 30);
                AbsoluteLayout.SetLayoutBounds(b, rect);
            }
            return;
        }
        private async void newgame_Clicked(object sender, EventArgs e)
        {
            var res = await DisplayAlert("", "Start new game?", "ok", "cancel");
            if (!res)
                return;

            backgammon.reset();
            backgroundLayout.Children.Clear();
            current = Color.White;
            buildBoard();
            fillBoard();
            bar1.Children.Clear();
            bar2.Children.Clear();
            fillBars();
        }

        private async Task<bool> blackTurn(int index, int move, int turn)
        {
            int errorCheck = -1;
            if (move != 0)
            {
                int rowChoice = index;
                int moved = rowChoice + move;
                errorCheck = backgammon.movePieceBlack(rowChoice, moved);
                if (errorCheck == 0)
                {
                    if (backgammon.checkBlackWin())
                    {
                        await DisplayAlert("Game over!", "Blacks win!", "OK");
                        return true;
                    }
                }
                else
                    return false;
            }
            updateView();
            return true;
        }
    }
}
