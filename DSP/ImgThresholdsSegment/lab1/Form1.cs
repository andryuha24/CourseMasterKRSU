using System;
using System.Drawing;
using System.Windows.Forms;
using MetroFramework.Forms;


namespace lab1
{
    public partial class Form1 : MetroForm
    {
        static Image TarImage;
        private ImageProcessControl obj = new ImageProcessControl();
        public Form1()
        {
            InitializeComponent();
            
        }


        private void runButton_Click(object sender, EventArgs e)
        {
            SelectSegments();
        }

        private void SelectSegments()
        {
            
            try
            {
                Bitmap img = new Bitmap(pictureBox2.Image);
                if (metroComboBox1.SelectedIndex == 0)
                {
                    pictureBox3.Image = obj.PaintImage(img, 2);
                }
                else if (metroComboBox1.SelectedIndex == 1)
                {
                    pictureBox3.Image = obj.PaintImage(img, 1);
                }
                else if (metroComboBox1.SelectedIndex == 2)
                {
                    pictureBox3.Image = obj.PaintImage(img, 0);
                }
                else if (metroComboBox1.SelectedIndex == 3)
                {
                    pictureBox3.Image = obj.PaintImage(img, 3);
                }
                else
                    MetroFramework.MetroMessageBox.Show(this, "Выберите канал изображения для построения гистограммы!", "Предупреждение.", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            catch (ArgumentOutOfRangeException) { MetroFramework.MetroMessageBox.Show(this, "Не возможно выделить сегменты.\nВыберите другую ширину фильтра гистограммы!", "Предупреждение.", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            catch (NullReferenceException) { MetroFramework.MetroMessageBox.Show(this, "Загрузите изображения для обработки!", "Предупреждение.", MessageBoxButtons.OK, MessageBoxIcon.Warning); }

        }

      

        private void OpenTargetImg()
        {
            OpenFileDialog dialog2 = new OpenFileDialog();
            dialog2.Filter = "Image Files(*.jpg; *.jpeg; *.bmp; *.png;)|*.jpg; *.jpeg; *.bmp; *.png";
            dialog2.FilterIndex = 0;
            dialog2.Title = "Целевое изображение";
            if (dialog2.ShowDialog() == DialogResult.OK)
            {
                TarImage = Image.FromFile(dialog2.FileName);
                pictureBox2.Image = TarImage;
                metroComboBox1.Enabled = true;
                ProcessImage();
            }
        }
        private void openButton_Click(object sender, EventArgs e)
        {
          OpenTargetImg();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if(pictureBox3.Image != null)
            {
                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    dialog.Title = "Save Dialog";
                    dialog.Filter = "Image Files(*.jpg; *.jpeg; *.bmp; *.png;)|*.jpg; *.jpeg; *.bmp; *.png";
                    if (dialog.ShowDialog(this) == DialogResult.OK)
                    {
                        Bitmap b = new Bitmap(pictureBox3.Image);
                        b.Save(dialog.FileName);
                        MetroFramework.MetroMessageBox.Show(this, "Изображение успешно сохранено!", "Сохранение.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
                MetroFramework.MetroMessageBox.Show(this, "Результирующего изображения не существет.\nЗагрузите изображения для обработки!", "Предупреждение.", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = null;
            pictureBox3.Image = null;
            zedChart.GraphPane.CurveList.Clear();
            zedChart.AxisChange();
            zedChart.Invalidate();
        }


        private void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProcessImage();
        }

        private void ProcessImage()
        {
            try
            {
                if (metroComboBox1.SelectedIndex == 0 && !string.IsNullOrWhiteSpace(widthFilter.Text))
                {
                    obj.drawChart(pictureBox2, zedChart, 1, Convert.ToInt32(widthFilter.Text));

                }
                else if (metroComboBox1.SelectedIndex == 1 && !string.IsNullOrWhiteSpace(widthFilter.Text))
                {
                    obj.drawChart(pictureBox2, zedChart, 2, Convert.ToInt32(widthFilter.Text));
                }
                else if (metroComboBox1.SelectedIndex == 2 && !string.IsNullOrWhiteSpace(widthFilter.Text))
                {
                    obj.drawChart(pictureBox2, zedChart, 3, Convert.ToInt32(widthFilter.Text));
                }
                else if (metroComboBox1.SelectedIndex == 3 && !string.IsNullOrWhiteSpace(widthFilter.Text))
                {
                    obj.drawChart(pictureBox2, zedChart, 4, Convert.ToInt32(widthFilter.Text));
                }
            }
            catch (NullReferenceException) { MetroFramework.MetroMessageBox.Show(this, "Загрузите изображения для обработки!", "Предупреждение.", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            metroComboBox1.Enabled = false;
        }

        private void widthFilter_TextChanged(object sender, EventArgs e)
        {
            ProcessImage();
        }

        private void widthFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
