using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace GameEngine.Models
{
    public class block : AbsoluteLayout
    {
        public int row { get; set; }
        public int col { get; set; }
        public int stones_count { get; set; }
        public Color stone { get; set; }
        public block(Thickness margin, int width, Brush topColor, Brush bottomColor, int row, int col)
        {
            this.row = row;
            this.col = col;
            stones_count = 0;
            stone = Color.Default;
            var shape1 = new Xamarin.Forms.Shapes.Polygon()
            {
                Points = new PointCollection()
                {
                    new Point(0, 2),
                    new Point(width, 3),
                    new Point(width / 2, width * 3.5)
                },
                Fill = topColor,
                Stroke = Brush.Black
            };
            var shape2 = new Xamarin.Forms.Shapes.Polygon()
            {
                Points = new PointCollection()
                {
                    new Point(0, width * 3.5),
                    new Point(width, width * 3.5),
                    new Point(width / 2, 0)
                },
                Fill = bottomColor,
                Stroke = Brush.Black
            };
            this.Margin = margin;
            if (row == 0)
                this.Children.Add(shape1);
            else if (row == 1)
                this.Children.Add(shape2);
        }
    }
}
