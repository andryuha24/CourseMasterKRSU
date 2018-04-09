using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace lab2
{
    public partial class Form1 : Form
    {
        private List<double> values = new List<double>();
        private MeasurementAnalysis mAnalyser;
        public Form1()
        {
            InitializeComponent();
        
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var data = LoadFile();
            if (data == null) return;
            ShowExperimentalData(data);
            mAnalyser = new MeasurementAnalysis(dataGridValues,dataGridResult, values, numK);
            mAnalyser.PreliminaryProcessing();
        }

        private void ShowExperimentalData(List<string> data)
        {
            values.Clear();
            dataGridValues.Rows.Clear();
            foreach (var elem in data)
                values.Add(Convert.ToDouble(elem));
            values.Sort();

            dataGridValues.Rows.Add(values.Count);
            for (var i = 0; i < values.Count; i++)
            {
                dataGridValues[0, i].Value = values[i];
            }
            numK.Value = Convert.ToDecimal(1 + 3.31 * Math.Log10(values.Count));
        }

        

        private List<string> LoadFile()
        {
            var dlgRes = openFileDialog.ShowDialog();
            if (dlgRes != DialogResult.OK)
                return null;
            if (string.IsNullOrEmpty(openFileDialog.FileName))
                return null;
            var dataFromFile = File.ReadAllLines(openFileDialog.FileName).ToList();
            return dataFromFile;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChartSettings();
            if (mAnalyser == null) return;
            mAnalyser.GetHistogrms();
            mAnalyser.DrawGraph(chart1, mAnalyser.RelativeFreqList);
            mAnalyser.DrawGraph(chart2, mAnalyser.AbsFreqList);
        }

        private void ChartSettings()
        {
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            chart1.Series[1].ChartType = SeriesChartType.Line;
            chart2.Series[0].Points.Clear();
            chart2.Series[1].Points.Clear();
            chart2.Series[1].ChartType = SeriesChartType.Line;
        }
    }
}
