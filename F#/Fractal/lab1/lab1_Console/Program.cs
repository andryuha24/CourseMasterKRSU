using System;
using System.Drawing;
using System.Linq;

namespace lab1_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Path for Newton Fractal Image:");
            var pathNF = Console.ReadLine();
            Console.WriteLine("Path for Dragon Curve Image:");
            var pathDF = Console.ReadLine();
            try
            {
                if (pathNF != null) CreateNewtonF().Save(pathNF); Console.WriteLine("OK!");
                if (pathDF != null) CreateDragonF().Save(pathDF); Console.WriteLine("OK!");
            }
            catch (Exception exception)
            {
               Console.WriteLine(exception.Message);
            }
            Console.WriteLine("Enter any key...");
            Console.ReadKey();
        }

        static Bitmap CreateNewtonF()
        {
            var width = 512;
            var height = 512;
            var drawArea = new Bitmap(width, height);
            for (var i = 0; i < width - 1; i++)
            {
                for (var j = 0; j < height - 1; j++)
                {
                    drawArea.SetPixel(i, j, MyFractal.newton_pixel(i, j, width, height, 50.0));
                }
            }
            return drawArea;
        }

        static Bitmap CreateDragonF()
        {
            var width = 400;
            var height = 300;
            var drawArea = new Bitmap(width, height);
            var zig = MyFractal.zig(100, 100, 356, 100);
            var list = MyFractal.dragon(100, 100, zig.Item1, zig.Item2, 356, 100, 14).ToList();
            var g = Graphics.FromImage(drawArea);
            var pen = new Pen(Brushes.Firebrick, 1);
            for (var i = 0; i < list.Count - 1; i++)
            {
                g.DrawLine(pen, list[i].Item1, list[i].Item2, list[i + 1].Item1, list[i + 1].Item2);
            }
            return drawArea;
        }

    }
}
