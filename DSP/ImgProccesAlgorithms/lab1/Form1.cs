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

              if (pictureBox2.Image == null)
                  MetroFramework.MetroMessageBox.Show(this, "Загрузите изображения для обработки!", "Предупреждение.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
              else
              {
                  if(metroComboBox1.SelectedIndex==0)
                  {
                      pictureBox3.Image = obj.Smoothing(pictureBox2.Image, metroGrid1);
                      coeffLabel.Text = Math.Round(ImageProcessControl.coeff, 2).ToString() + " *";
                  }
                  else if(metroComboBox1.SelectedIndex == 1)
                  {
                      pictureBox3.Image = obj.EdgeEmphasize(pictureBox2.Image, metroGrid1,1);
                     coeffLabel.Text = " ";
                  }
                  else if (metroComboBox1.SelectedIndex == 2)
                  {
                      pictureBox3.Image = obj.EdgeEmphasize(pictureBox2.Image, metroGrid1, 2);
                      coeffLabel.Text = Math.Round(ImageProcessControl.coeff, 2).ToString() + " *";
                  }
                  else if (metroComboBox1.SelectedIndex == 3)
                  {
                      pictureBox3.Image = obj.EdgeEmphasize(pictureBox2.Image, metroGrid1, 3);
                      coeffLabel.Text = " ";
                   }
                else if (metroComboBox1.SelectedIndex == 4)
                {
                    pictureBox3.Image = obj.AllocateEdge(pictureBox2.Image, metroGrid1, 1);
                    coeffLabel.Text = " ";
                }
                else if (metroComboBox1.SelectedIndex == 5)
                {
                    pictureBox3.Image = obj.AllocateEdge(pictureBox2.Image, metroGrid1, 2);
                    coeffLabel.Text = " ";
                }
                else if (metroComboBox1.SelectedIndex == 6)
                {
                    pictureBox3.Image = obj.StampingImage(pictureBox2.Image, metroGrid1, 1);
                    coeffLabel.Text = " ";
                }
                else if (metroComboBox1.SelectedIndex == 7)
                {
                    pictureBox3.Image = obj.StampingImage(pictureBox2.Image, metroGrid1, 2);
                    coeffLabel.Text = " ";
                }
                else if (metroComboBox1.SelectedIndex == 8)
                {
                    pictureBox3.Image = obj.MedianFilterImage(pictureBox2.Image, metroGrid1);
                    coeffLabel.Text = " ";
                }
                else if (metroComboBox1.SelectedIndex == 9)
                {
                    pictureBox3.Image = obj.WatercolorImage(pictureBox2.Image, metroGrid1);
                    coeffLabel.Text = " ";
                }


            }


        }


        private void OpenTargetImg()
        {
            OpenFileDialog dialog2 = new OpenFileDialog();
            dialog2.Filter = "Image Files(*.jpg; *.jpeg; *.bmp)|*.jpg; *.jpeg; *.bmp";
            dialog2.FilterIndex = 0;
            dialog2.Title = "Целевое изображение";
            if (dialog2.ShowDialog() == DialogResult.OK)
            {
                TarImage = Image.FromFile(dialog2.FileName);
                pictureBox2.Image = TarImage;
            }
        }
        private void openButton_Click(object sender, EventArgs e)
        {
          OpenTargetImg();
        }

        private void saveButton_Click(object sender, EventArgs e)
        { 
          using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Title = "Save Dialog";
                dialog.Filter = "Image Files(*.jpg; *.jpeg; *.bmp)|*.jpg; *.jpeg; *.bmp";
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
            pictureBox2.Image = null;
            pictureBox3.Image = null;
        }




        private void Form1_Load(object sender, EventArgs e)
        {
            metroComboBox1.SelectedIndex = 0;
        }
    }
}
