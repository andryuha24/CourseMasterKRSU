using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MetroFramework.Forms;
using System.Numerics;
using System.Windows.Forms.DataVisualization.Charting;

namespace lab1
{
    public delegate Complex Functions(Complex t);
    public partial class MainForm : MetroForm
    {
        private readonly FFT _fft = new FFT();
        private readonly List<Functions> _fnc = new List<Functions>();
        public  int N { get; set; }
        private  double Dt { get; set; }
        private double Dv { get; set; }

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            _fnc.Add(x => Math.Sin(2 * x.Real));
            _fnc.Add(x => Math.Sin(2 * x.Real) + Math.Sin(4 * x.Real));
            _fnc.Add(x => x.Real);
            numDT.Value = (decimal) 0.5;
            numN.Value = 7;
            chart2.Series[0].ChartType = SeriesChartType.BoxPlot;
            chart1.Series[0].ChartType = SeriesChartType.Line;
            chart3.Series[0].ChartType = SeriesChartType.Line;
            metroComboBox1.SelectedIndex = 0;
        }

        private void runButton_Click(object sender, EventArgs e)
        {

            try
            {
                chart1.Series[0].Points.Clear();
                chart2.Series[0].Points.Clear();
                chart3.Series[0].Points.Clear();
                Dt = (double)numDT.Value;
                N = (int)Math.Pow(2, (int)numN.Value);
                Dv = 1.0 / (N * Dt);
                List<Complex> X;
                if (metroComboBox1.SelectedIndex!=3)
                  X = FFT.Generate(N, Dt, i => _fnc[metroComboBox1.SelectedIndex](i.Real));
                else
                {
                    X = FFT.Generate(N, Dt, i => _fnc[0](i.Real), i => _fnc[1](i.Real));
                }
                var directFft = _fft.Fft(X.ToArray(), false);
                var inverseFft = _fft.Fft(directFft,true);
                _fft.DrawGraph(chart1, X.Select(x => x.Real).ToArray(), N);
                _fft.DrawGraph(chart2, directFft.Select(x => Math.Pow(x.Magnitude, 2) / N / N).ToArray(), N/2, Dv);
                _fft.DrawGraph(chart3, inverseFft.Select(x => x.Real/N).ToArray(), N);
            }
            catch (ArgumentOutOfRangeException)
            {

                MessageBox.Show(@"Заданный аргумент превышает допустимый диапазон значений");
            }
             
        }
    }
}
