using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MathNet.Numerics;

namespace lab2
{
    public partial class Form1 : Form
    {
        private List<double> xList = new List<double>();
        private List<double> xList1 = new List<double>();
        private List<double> xList2 = new List<double>();
        private List<double> xList3 = new List<double>();
        private List<double> xList4 = new List<double>();
        private List<double> yList = new List<double>();
        private List<double> yList2 = new List<double>();
        private MeasurementAnalysis mAnalyser = new MeasurementAnalysis();
        private int m;
        public Form1()
        {
            InitializeComponent();
        }


        private void ShowExperimentalData(DataGridView dataGrid,  List<string> data, List<double> resultList,int column)
        {
            resultList.Clear();
            foreach (var elem in data)
                resultList.Add(double.Parse(elem));
            if(dataGrid.RowCount == 0)
                dataGrid.Rows.Add(resultList.Count);
            for (var i = 0; i < resultList.Count; i++)
            {
                dataGrid[column, i].Value = resultList[i];
            }
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


        private void yFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var data = LoadFile();
            if (data == null) return;
            ShowExperimentalData(dataGridValues, data,yList,1);
        }

        private void xFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var data = LoadFile();
            if (data == null) return;
            ShowExperimentalData(dataGridValues,data, xList, 0);
        }

        private void calculateButton_Click(object sender, EventArgs e)
        {
            if (dataGridResult.RowCount == 0) dataGridResult.Rows.Add(1);
           
            label2.Text = @" ";
            m = Convert.ToInt32(numK.Value);
            var b = Fit.Polynomial(xList.ToArray(), yList.ToArray(), m);
            var s = mAnalyser.DispersionOfPerturbations(yList.ToArray(), new List<double[]> { xList.ToArray() }, b, m, true);
            var alfa = 0.05;
            var d = mAnalyser.Dispersion(yList);

            double g, v1, v2;
            if (s >= d)
            {
                g = s / d;
                v1 = yList.Count - m;
                v2 = yList.Count - 1;
            }
            else
            {
                g = d / s;
                v2 = yList.Count - m;
                v1 = yList.Count - 1;
            }

            var fQuantile = mAnalyser.FisherQuantile(v1, v2, alfa);
            ShowBCoeff(b,bResult1);
            dataGridResult[0, 0].Value = Math.Round(s, 10);
            dataGridResult[1, 0].Value = Math.Round(d, 8);
            dataGridResult[2, 0].Value = Math.Round(g, 8);
            dataGridResult[3, 0].Value = Math.Round(fQuantile, 8);
            label2.Text = g <= fQuantile ? @"true" : @"false";
        }

        private void ShowBCoeff(double[] b, DataGridView bResult)
        {
            bResult.Rows.Clear();
            bResult.Rows.Add(b.Length);
            for (var i = 0; i < b.Length; i++)
            {
                bResult[0, i].Value = b[i];
            }
        }

        private void x1FileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var data = LoadFile();
            if (data == null) return;
            ShowExperimentalData(dataGridValues2, data, xList1, 0);
        }

        private void x2FileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var data = LoadFile();
            if (data == null) return;
            ShowExperimentalData(dataGridValues2, data, xList2, 1);
        }

        private void x3FileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var data = LoadFile();
            if (data == null) return;
            ShowExperimentalData(dataGridValues2, data, xList3, 2);
        }

        private void x4FileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var data = LoadFile();
            if (data == null) return;
            ShowExperimentalData(dataGridValues2, data, xList4, 3);
        }

        private void yFile2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var data = LoadFile();
            if (data == null) return;
            ShowExperimentalData(dataGridValues2, data, yList2, 4);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var alfa = 0.05;
            var xLists = new List<double[]> { xList1.ToArray(), xList2.ToArray(),
                                              xList3.ToArray(), xList4.ToArray()
                                             };
            if (dataGridResult2.RowCount == 0) dataGridResult2.Rows.Add(xLists.Count+1);
            double[] b;
            double s;
            var meaningfulParameters = new Dictionary<double[],bool>();
            label3.Text = CheckHypothesis(alfa, xLists, out b, out s).ToString();
            CheckSignificanceOfParametrs(alfa, xLists, b, s, meaningfulParameters);
            var k = meaningfulParameters.Count(x => x.Value == false);
     
            for (var i = 0; i < k; i++)
            {
                var tempMeaningfulParametrs = meaningfulParameters.Where(x => x.Value == false).ToList();
                var n = tempMeaningfulParametrs.Count() < 2 ? i - meaningfulParameters.Count : i;
                var newXlists = xLists.Where(x=>x!=tempMeaningfulParametrs[n].Key).ToList();
                if (!CheckHypothesis(alfa, newXlists, out b, out s))
                    continue;
                else
                {
                    CheckSignificanceOfParametrs(alfa, newXlists, b, s, meaningfulParameters);
                    xLists = xLists.Where(x => x != tempMeaningfulParametrs[n].Key).ToList();
                }
                   
            }

        }


        private bool CheckHypothesis(double alfa, List<double[]> xLists, out double[] b, out double s)
        {
            b = mAnalyser.LinearRegression(xLists, yList2.ToArray());
            var d = mAnalyser.Dispersion(yList2);
            s = mAnalyser.DispersionOfPerturbations(yList2.ToArray(), xLists, b, xLists.Count, false);
            double gMain, v1, v2;
            if (s >= d)
            {
                gMain = s / d;
                v1 = yList2.Count - xLists.Count;
                v2 = yList2.Count - 1;
            }
            else
            {
                gMain = d / s;
                v2 = yList2.Count - xLists.Count;
                v1 = yList2.Count - 1;
            }
            var fQuantile = mAnalyser.FisherQuantile(v1, v2, alfa);
            dataGridResult2[4, 0].Value = Math.Round(d,8);
            dataGridResult2[5, 0].Value = Math.Round(fQuantile, 8);
            return gMain <= fQuantile;
        }

        private void CheckSignificanceOfParametrs(double alfa, List<double[]> xLists, double[] b, double s, Dictionary<double[],bool> meaningfulParameters)
        {
            meaningfulParameters.Clear();
            var errorMatrix = mAnalyser.ErrorMatrix(xLists);
            var g = new List<double>();
            for (var i = 0; i < errorMatrix.ColumnCount; i++)
            {
                g.Add(b[i] / Math.Sqrt(s * errorMatrix[i, i]));
            }

            var v = xList1.Count - xLists.Count;
            var tQuantile = mAnalyser.DistributionOfStudent(alfa, v);

            ShowBCoeff(b, bResult2);
            dataGridResult2[0, 0].Value = Math.Round(s, 10);
            dataGridResult2[2, 0].Value = Math.Round(tQuantile, 8);
         
            for (var i = 0; i < g.Count; i++)
            {
                dataGridResult2[1, i].Value = Math.Round(g[i], 8);
                bool meaningful;
                if (Math.Abs(Math.Round(g[i], 8)) <= Math.Round(tQuantile, 8))
                {
                    dataGridResult2[3, i].Value = "False";
                    meaningful = false;
                }
                else
                {
                    dataGridResult2[3, i].Value = "True";
                    meaningful = true;
                }
                
                if (i > 0) meaningfulParameters.Add(xLists[i - 1], meaningful);

            }
        }
    }
}
