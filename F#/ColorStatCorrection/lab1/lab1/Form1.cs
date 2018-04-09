using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using MetroFramework.Forms;


namespace lab1
{
    public partial class Form1 : MetroForm
    {

        public UtilityMatrixAndType.BMP CurrImage, TarImage;
        private bool _contrast,_hsl,_lab;


        public Form1()
        {
            InitializeComponent();
            
        }
        private void runButton_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null && pictureBox2.Image == null)
                MetroFramework.MetroMessageBox.Show(this, "Загрузите изображения для обработки!", "Предупреждение.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (pictureBox1.Image != null && pictureBox2.Image == null)
            {
                MetroFramework.MetroMessageBox.Show(this, "Загрузите целевое изображение для обработки!", "Предупреждение.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                OpenTargetImg();
            }
            else if (pictureBox2.Image != null && pictureBox1.Image == null)
            {
                MetroFramework.MetroMessageBox.Show(this, "Загрузите  изображение источника цвета!", "Предупреждение.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                OpenSourceImg();
            }
            else
            {
                if (!_hsl && _lab)
                {
                    //nonParallel
                    var stopwatch1 = Stopwatch.StartNew();
                    var resBmp1 = ColorCorrect.rgb2lab(CurrImage.Img, TarImage.Img, _contrast);
                    stopwatch1.Stop();
                    pictureBox3.Image = resBmp1;
                    metroLabel3.Text = @"Результат.T_последоват.(мс) = " + Math.Round(stopwatch1.Elapsed.TotalSeconds,4);
                    //parallel
                    var stopwatch2 = Stopwatch.StartNew();
                    var resBmp2 = ParallelColorCorrect.rgb2labParallel(CurrImage, TarImage, _contrast);
                    stopwatch2.Stop();
                    
                    pictureBox4.Image = resBmp2;
                    metroLabel4.Text = @"Результат.T_паралл.(с) = " + Math.Round(stopwatch2.Elapsed.TotalSeconds,4);
                }                  
                else if (_hsl && !_lab)
                {
                    //nonParallel
                    var stopwatch1 = Stopwatch.StartNew();
                    var resBmp1 = ColorCorrect.rgb2hsl(CurrImage.Img, TarImage.Img, _contrast);
                    stopwatch1.Stop();
                    pictureBox3.Image = resBmp1;
                    metroLabel3.Text = @"Результат.T_последоват.(мс) = " + Math.Round(stopwatch1.Elapsed.TotalSeconds,4);
                    //parallel
                    var stopwatch2 = Stopwatch.StartNew();
                    var resBmp2 = ParallelColorCorrect.rgb2hslParallel(CurrImage, TarImage, _contrast);
                    stopwatch2.Stop();
                    pictureBox4.Image = resBmp2;
                    metroLabel4.Text = @"Результат.T_паралл.(с) = " + Math.Round(stopwatch2.Elapsed.TotalSeconds,4);
                }
                    
                else
                    MetroFramework.MetroMessageBox.Show(this, "Выберите режим обработки!", "Предупреждение.",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void OpenSourceImg()
        {            
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = @"Image Files(*.jpg; *.jpeg; *.bmp)|*.jpg; *.jpeg; *.bmp";
            dialog.FilterIndex = 0;
            dialog.Title = @"Изображение источник";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
          
                pictureBox1.Image = Image.FromFile(dialog.FileName);
                CurrImage = new UtilityMatrixAndType.BMP(new Bitmap(pictureBox1.Image),dialog.FileName);
            }
        }

        private void OpenTargetImg()
        {
            OpenFileDialog dialog2 = new OpenFileDialog();
            dialog2.Filter = @"Image Files(*.jpg; *.jpeg; *.bmp)|*.jpg; *.jpeg; *.bmp";
            dialog2.FilterIndex = 0;
            dialog2.Title = @"Целевое изображение";
            if (dialog2.ShowDialog() == DialogResult.OK)
            {
                pictureBox2.Image = Image.FromFile(dialog2.FileName);
                TarImage = new UtilityMatrixAndType.BMP(new Bitmap(pictureBox2.Image), dialog2.FileName);
            }
        }
        private void openButton_Click(object sender, EventArgs e)
        {
          OpenSourceImg();
          OpenTargetImg();
        }

        private void saveButton_Click(object sender, EventArgs e)
        { 
          using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Title = @"Save Di alog";
                dialog.Filter = @"Image Files(*.jpg; *.jpeg; *.bmp)|*.jpg; *.jpeg; *.bmp";
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    Bitmap b = new Bitmap(pictureBox3.Image);
                    b.Save(dialog.FileName);
                    MetroFramework.MetroMessageBox.Show(this, "Изображение успешно сохранено!", "Сохранение.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            pictureBox3.Image = null;
            pictureBox4.Image = null;
        }

        private void metroCheckBox2_Click(object sender, EventArgs e)
        {
            _hsl = true;
            metroCheckBox1.Checked = false;
            _lab = false;
        }

        private void metroCheckBox1_Click(object sender, EventArgs e)
        {
            _lab = true;
            metroCheckBox2.Checked = false;
            _hsl = false;
        }

        private void metroCheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            if(metroCheckBox3.Checked)
            {
                _contrast = true;
            }
            if (!metroCheckBox3.Checked)
            {
                _contrast = false;
            }
        }

    
    }
}
