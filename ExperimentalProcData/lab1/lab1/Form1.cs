using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab1
{
    public partial class Form1 : Form
    {
        private readonly double[] _data = {
                528.2, 542.8, 531.2, 597.5, 555.9, 553.2, 539.2, 548.2, 523.5, 561.0,
                546.8, 545.8, 538.4, 541.7, 547.6, 541.9, 551.0, 540.0, 569.8, 529.3,
                524.2, 558.5, 544.0, 545.4, 539.6, 525.5, 592.2, 536.8, 519.9, 505.1,
                536.0, 584.5, 540.3, 544.5, 535.0, 551.3, 558.3, 525.5, 554.7, 542.1
            };

        private readonly MeasurementAnalysis _mAnalyser = new MeasurementAnalysis();
        public Form1()
        {
            InitializeComponent();
        }
        DataTable dt = new DataTable();
        private void Form1_Load(object sender, EventArgs e)
        {
            List<double> lstValue = new List<double>(_data);
            var expectedValue = _mAnalyser.ExpectationValue(lstValue);
            var dispersion = _mAnalyser.Dispersion(lstValue, expectedValue);

            dataGridView1.Rows.Add(lstValue.Count-1);
            var quantileT = Math.Round(_mAnalyser.DistributionOfStudent(_mAnalyser.GetQuantile(0.05), lstValue.Count), 5);
            for (var i = 0; i < lstValue.Count; i++)
            {
                dataGridView1[0, i].Value = i+1;
                dataGridView1[1, i].Value = lstValue[i];
                dataGridView1[2,i].Value = Math.Round(_mAnalyser.GetU(lstValue[i], expectedValue, dispersion), 5);

                var u = _mAnalyser.GetU(lstValue[i], expectedValue, dispersion);
                dataGridView1[3, i].Value = u > quantileT ? "Промах" : "Непромах";
                dataGridView1[2, i].Value = u;

                var k = _mAnalyser.GetK(lstValue[i],lstValue);
                dataGridView1[5, i].Value = k > 4 ? "Промах" : "Непромах";
                dataGridView1[4, i].Value = k;
            }
        } 
    }
}
