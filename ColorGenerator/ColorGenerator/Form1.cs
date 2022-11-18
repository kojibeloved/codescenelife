using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ColorGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private List<ColorBase> colors;
        private int colorIndex = 0;
        private Color GetNextColor()
        {
            var color = colors[colorIndex].GetColor();
            colorIndex++;
            return color;
        }
        private void Form1_Load(object sender, EventArgs e)
        {

            colors = new List<ColorBase>();
            var r = new Random((int)DateTime.Now.Ticks%int.MaxValue);
            colors.Add(new ColorBase(128, 128, 0));
            //colors.Add(new ColorBase(255, 255, 255));
            //colors.Add(new ColorBase(255, 0, 0));
            //colors.Add(new ColorBase(0, 255, 0));
            //colors.Add(new ColorBase(0, 0, 255));
            //colors.Add(new ColorBase(255, 255, 0));
            //colors.Add(new ColorBase(0, 255, 255));
            //colors.Add(new ColorBase(255, 0, 255));
            //colors.Add(new ColorBase(128, 128, 128));
            for (int i = 0; i < 1000; i++)
            {
                var number = 10;
                var newColors = new ColorDistance[number];
                for (int index = 0; index < number; index++)
                {
                    int red = r.Next(255);
                    int green = r.Next(255);
                    int blue = r.Next(255);
                    for (int loop = 0; loop < 10; loop++)
                    {
                        if ((red + green + blue) < 550)
                            break;

                        red = r.Next(255);
                        green = r.Next(255);
                        blue = r.Next(255);
                    }
                    newColors[index] = new ColorDistance(red, green , blue);
                }

                foreach (var c in colors)
                {
                    foreach (var nc in newColors)
                    {
                        var dis = (decimal)Math.Sqrt(Math.Pow(c.R - nc.R, 2) + Math.Pow(c.G - nc.G, 2) + Math.Pow(c.B - nc.B, 2));
                        nc.UpdateDistance(dis);
                    }

                }
                var newColor = newColors.OrderByDescending(x => x.Distance).First();
                if (newColor.Distance > 80)
                    colors.Add(newColor);
            }


            int width = 800;
            int height = 600;
            Color color = GetNextColor();
            var bmp = new Bitmap(width, height);

            for (int i = 0; i < width; i++)
            {

                for (int j = 0; j < height; j++)
                {
                    if (i > 0 && i % 200 != 0)
                    {
                        color = bmp.GetPixel(i - 1, j);
                    }
                    else
                    if (j % 100 == 0)
                    {
                        color = GetNextColor();
                        Console.WriteLine(colorIndex +" : "+color.R.ToString() + " " + color.G.ToString() + " " +color.B);
                    }
                    bmp.SetPixel(i, j, color);
                }
            }
            pictureBox2.Image = bmp;
            bmp.Save("c:\\temp\\test.png");

        }
    }
    public class ColorBase
    {
        public ColorBase(int r, int g, int b)
        {
            R = r;
            G = g;
            B = b;
        }

        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }

        public Color GetColor()
        {
            return Color.FromArgb(R, G, B);
        }
    }

    public class ColorDistance : ColorBase
    {
        public ColorDistance(int r, int g, int b) : base(r, g, b)
        {
        }

        public decimal? Distance { get; set; }

        public void UpdateDistance(decimal distance)
        {
            if (Distance == null || Distance > distance)
                Distance = distance;
        }
    }
}
