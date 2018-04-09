using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MetroFramework.Forms;

namespace lab1
{
    public partial class Form1 : MetroForm
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void runButton_Click(object sender, EventArgs e)
        {
            var drawArea = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            pictureBox1.Image = drawArea;
            for (var i = 0; i < pictureBox1.Image.Width - 1; i++)
            {
                for (var j = 0; j < pictureBox1.Height - 1; j++)
                {
                    drawArea.SetPixel(i, j, MyFractal.newton_pixel(i, j, pictureBox1.Width, pictureBox1.Height, 50.0));
                }
            }

            var drawDragonArea = new Bitmap(pictureBox2.Size.Width, pictureBox2.Size.Height);
            var zig = MyFractal.zig(100, 100, 356, 100);
            var list = MyFractal.dragon(100, 100, zig.Item1, zig.Item2, 356, 100, 13).ToList();
            var g = Graphics.FromImage(drawDragonArea);
       
            var pen = new Pen(Brushes.DarkGreen, 1);
            for (var i = 0; i < list.Count - 1; i++)
            {
                g.DrawLine(pen, list[i].Item1, list[i].Item2, list[i + 1].Item1, list[i + 1].Item2);
            }
            pictureBox2.Image = drawDragonArea;
        }


        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveImage("Newton Fractal", pictureBox1);
            SaveImage("Dragon Curve", pictureBox2);
        }

        private void SaveImage(string name, PictureBox pBox)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Title = "Save Dialog_"+name;
                dialog.Filter = "Image Files(*.jpg; *.jpeg; *.bmp)|*.jpg; *.jpeg; *.bmp";
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    Bitmap b = new Bitmap(pBox.Image);
                    b.Save(dialog.FileName);
                    MetroFramework.MetroMessageBox.Show(this, "Изображение успешно сохранено!", "Сохранение.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            pictureBox2.Image = null;
        }
    
    }
}
