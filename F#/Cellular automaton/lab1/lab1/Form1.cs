using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using MetroFramework.Forms;
using Timer = System.Windows.Forms.Timer;

namespace lab1
{
    public partial class Form1 : MetroForm
    {
        private readonly Timer _timer1 = new Timer();
        private int _speed = 15;
        private Bitmap _drawArea;
        private Tuple<int, int> _prevPoint;
        private Tuple<CellAutomata.state, ConsoleColor, Tuple<int, int>, CellAutomata.dir> _res;
        public Form1()
        {
            InitializeComponent();
            InitializeCellAutomat();
        }

        private void InitializeCellAutomat()
        {
            _prevPoint = new Tuple<int, int>(215, 150);
            _res = CellAutomata.turmite(CellAutomata.state.A, ConsoleColor.Black, 215, 150, CellAutomata.dir.East);
            _timer1.Tick += timer1_Tick;
            _timer1.Interval = _speed;
            _timer1.Enabled = false;
            _drawArea = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            _timer1.Enabled = true;
        }

        public Color ColorFromConsoleColor(ConsoleColor oldcolor)
        {
            string colorname = oldcolor.ToString();
            return colorname == "DarkYellow" ? Color.FromArgb(255, 128, 128, 0) : Color.FromName(colorname);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveImage("Клеточный автомат", pictureBox1);
        }

        private void SaveImage(string name, PictureBox pBox)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Title = @"Save Dialog_"+name;
                dialog.Filter = @"Image Files(*.jpg; *.jpeg; *.bmp)|*.jpg; *.jpeg; *.bmp";
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
            timer1.Enabled = false;
            pictureBox1.Image = null;
            _prevPoint = new Tuple<int, int>(215, 150);
            _res = CellAutomata.turmite(CellAutomata.state.A, ConsoleColor.Black, 215, 150, CellAutomata.dir.East);
            _drawArea = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            _timer1.Enabled = false;
        }

       
        public string GetKnownColorName(int r, int g, int b)
        {
            var myColor = Color.FromArgb(r, g, b).ToArgb();
            string namedColor = null;
            foreach (var name in Enum.GetNames(typeof(KnownColor)))
            {
                var kc = Color.FromName(name);
                if (kc.IsSystemColor || kc.ToArgb() != myColor) continue;
                namedColor = kc.Name;
                break;
            }
            return namedColor;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Image = _drawArea;
            _drawArea.SetPixel(_prevPoint.Item1, _prevPoint.Item2, ColorFromConsoleColor(_res.Item2));
            var c = _drawArea.GetPixel(_res.Item3.Item1, _res.Item3.Item2);
            var colorName = GetKnownColorName(c.R, c.G, c.B);
            var color = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), (colorName == "Fuchsia" ? "Magenta" : colorName));
            _prevPoint = new Tuple<int, int>(_res.Item3.Item1, _res.Item3.Item2);
            _res = CellAutomata.turmite(_res.Item1, color, _res.Item3.Item1, _res.Item3.Item2, _res.Item4);
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            _timer1.Enabled = false;
        }
    }
}
