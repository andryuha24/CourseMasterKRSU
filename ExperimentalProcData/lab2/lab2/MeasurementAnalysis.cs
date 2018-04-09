using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace lab2
{
    public class MeasurementAnalysis
    {
        private readonly DataGridView _dgResults, _dgExpData;
        private readonly List<double> _experimentalDataList;
        private NumericUpDown numK;
        private int _k; 

        public List<double> AbsFreqList = new List<double>();
        public List<double> RelativeFreqList = new List<double>();
        public List<double> MidPoints = new List<double>();
        public List<double> HitRateList = new List<double>();

        public MeasurementAnalysis(DataGridView dataGridValues, DataGridView datagridRes, List<double> dataList, NumericUpDown k)
        {
            _dgResults = datagridRes;
            _dgExpData = dataGridValues;
            _dgResults.Rows.Clear();
            _dgResults.Rows.Add(4);
            _experimentalDataList = dataList;
            numK = k;
        }

        public void PreliminaryProcessing()
        {
            _dgResults[0, 0].Value = _experimentalDataList.Min();
            _dgResults[1, 0].Value = _experimentalDataList.Max();
            _dgResults[2, 0].Value = _experimentalDataList.Max() - _experimentalDataList.Min();
            ShowEvaluations();
            var expectedValue = ExpectationValue(_experimentalDataList);
            var dispersion = Dispersion(_experimentalDataList, expectedValue);
            var s = 0.0;
            var d = 0.0;
            for (var i = 0; i < _experimentalDataList.Count; i++)
            {
                var u = GetU(_experimentalDataList[i], expectedValue, dispersion);
                //s += u;
                //d += Math.Pow(u, 2);
                _dgExpData[1, i].Value = Math.Round(u, 4);
            }

            //Console.WriteLine(Math.Round(s,4)/ _experimentalDataList.Count + " "+Math.Round(d,4)/ _experimentalDataList.Count);
        }

        public void GetHistogrms()
        {
            AbsFreqList.Clear();
            RelativeFreqList.Clear();
            MidPoints.Clear();
            HitRateList.Clear();
            var min = _experimentalDataList.Min();
            var max = _experimentalDataList.Max();
            _k = Convert.ToInt32(numK.Value);
            var h = (max - min)/_k;

            var from = min - h;
            for (var i = 0; i < _k; i++)
            {
                from += h;
                var to = from + h;
                var m = GetNumInRange(_experimentalDataList, from, to);
                HitRateList.Add(m);
                MidPoints.Add(GetMidInRange(from, to));//сразу считаем среднее для интервалов

                AbsFreqList.Add(m / h);//абсоютные частоты
                RelativeFreqList.Add(m/(h * _experimentalDataList.Count));//относительные частоты
            }

            var theorFreq1 = TheoreticalFrequecyValuesFirst(MidPoints);
            _dgResults[9, 0].Value = Math.Round(XiSqr(theorFreq1, HitRateList), 4);

            var theorFreq2 = TheoreticalFrequecyValuesSecond(MidPoints);
            _dgResults[9, 1].Value = Math.Round(XiSqr(theorFreq2, HitRateList), 4);
            _dgResults[10, 0].Value = Math.Round(XiSqr(0.05, 10),4);
        }

        //количество элементов попавших в i-ый интервал
        private int GetNumInRange(List<double> lstValues, double from, double to)
        {
            return lstValues.Count(val => val >= from && val <= to);
        }
        

        private double GetMidInRange(double from, double to)
        {
            return (from + to) / 2;
        }

        public void DrawGraph(Chart chart, List<double> arr)
        {
            for (int i = 0; i < arr.Count; i++)
            {
                chart.Series[0].Points.AddXY(i, arr[i]);
                chart.Series[1].Points.AddXY(i, arr[i]);
            }
        }

        //оценки начальных элементов 
        private List<double> GetBeginningMoment(List<double> lstValues, int number)
        {
            var result = new List<double>();
            for (var i = 0; i < number; i++)
            {
                var value = lstValues.Sum(t => Math.Pow(t, i + 1));
                result.Add(value / lstValues.Count);
            }
            return result;
        }
      
        private void ShowEvaluations()
        {
            var beginingEvaluations = GetBeginningMoment(_experimentalDataList, 4);
            var offsetCenterEvaluations = GetOffsetCenterEvaluations(beginingEvaluations);
            var realCenterEvaluations = GetRealCenterEvaluations(_experimentalDataList, offsetCenterEvaluations);
            for (var i = 0; i < beginingEvaluations.Count; i++)
            {
                _dgResults[3, i].Value = Math.Round(beginingEvaluations[i], 4);
                _dgResults[4, i].Value = Math.Round(offsetCenterEvaluations[i], 4);
                _dgResults[5, i].Value = Math.Round(realCenterEvaluations[i], 4);
            }
            _dgResults[6, 0].Value = Math.Round(GetStdDev(realCenterEvaluations),4);
            _dgResults[7, 0].Value = Math.Round(GetAssymetryCoeff(realCenterEvaluations, GetStdDev(realCenterEvaluations)),4);
            _dgResults[8, 0].Value = Math.Round(GetExcessCoeff(realCenterEvaluations),4);


        }

        //смещенные  оценки центральных элементов 
        private List<double> GetOffsetCenterEvaluations(List<double> beginingEvaluations)
        {
            var result = new List<double>
            {
                beginingEvaluations[0],
                beginingEvaluations[1] - Math.Pow(beginingEvaluations[0], 2),
                beginingEvaluations[2]
                - 3*beginingEvaluations[0]*beginingEvaluations[1]
                + 2*Math.Pow(beginingEvaluations[0], 3),
                beginingEvaluations[3]
                - 4*beginingEvaluations[0]*beginingEvaluations[2]
                + 6*Math.Pow(beginingEvaluations[0], 2)*beginingEvaluations[1]
                - 3*Math.Pow(beginingEvaluations[0], 4)
            };
            return result;
        }

        //несмещенные  оценки центральных элементов 
        private List<double> GetRealCenterEvaluations(List<double> lstValues, List<double> offsetCenter)
        {
            var result = new List<double>();
            var n = lstValues.Count;
            var value = 0.0;
            for (var i = 0; i < n; i++)
            {
                value += Math.Pow(lstValues[i] - offsetCenter[0], 2);
            }
            result.Add(offsetCenter[0]);
            result.Add(value / (n - 1));
            result.Add((Math.Pow(n, 2) * offsetCenter[2]) / ((n - 1) * (n - 2)));
            result.Add(
                (n * ((Math.Pow(n, 2) - 2 * n + 3) * offsetCenter[3]) -
                 3 * n * (2 * n - 3) * Math.Pow(offsetCenter[1], 2))/
                ((n - 1) * (n - 2) * (n - 3))
                );
            return result;
        }

        //среднеквадратическое отклонение
        private double GetStdDev(List<double> realCenterEvaluations)
        {
            return Math.Sqrt(realCenterEvaluations[1]);
        }

        //коэффициент ассиметрии
        private double GetAssymetryCoeff(List<double> realCenterEvaluations, double stdDev)
        {
            return realCenterEvaluations[2]/Math.Pow(stdDev,3);
        }

        //коэффициент эксцесса
        private double GetExcessCoeff(List<double> realCenterEvaluations)
        {
            return (realCenterEvaluations[3] / Math.Pow(realCenterEvaluations[1], 2));
        }

        private double ExpectationValue(List<double> list)
        {
            var value = list.Sum();
            return (value / list.Count);
        }

        private double Dispersion(List<double> list, double expectationValue)
        {
            var value = list.Sum(item => Math.Pow((item - expectationValue), 2));
            return (value / list.Count);
        }

        private double GetU(double maxDeviation, double expectationValue, double dispersion)
        {
            return (maxDeviation - expectationValue) / Math.Sqrt(dispersion);
        }

        private double DensityOfGaussian(double normedValue)
        {
            return (1 / Math.Sqrt(2 * Math.PI)) * (Math.Exp(-(Math.Pow(normedValue, 2)) / 2));
        }

        public double GaussianFunction(double normedValue)
        {
            double[] d = { 4986.7347e-5, 2114.1006e-5, 327.76263e-5, 38.0036e-6, 48.8906e-6, 53.83e-7 };
            var fui = 1 - 0.5 * Math.Pow((1 + d[0] * normedValue + d[1] * Math.Pow(normedValue, 2) 
                                        + d[2] * Math.Pow(normedValue, 3) + d[3] * Math.Pow(normedValue, 4) + d[4] 
                                        * Math.Pow(normedValue, 5) + d[5] * Math.Pow(normedValue, 6)), -16);
            return fui;
        }


        private double Dispersion(List<double> x, List<double> m )
        {
            var temp1 = x.Select((t, i) => m[i]*Math.Pow(t, 2)).Sum();
            var temp2 = x.Select((t, i) => m[i] * t).Sum();
            var n = _experimentalDataList.Count;
            var s2 = 1.0/(n - 1)*(temp1 - (1.0/n)*Math.Pow(temp2, 2));
            return s2;
        }

        private double ExpectationValue(List<double> x, List<double> m)
        {
            var n = _experimentalDataList.Count;
            var M = (1.0 / n)*x.Select((t, i) => m[i] * t).Sum();
            return M;
        }

        private List<double> TheoreticalFrequecyValuesFirst(List<double> midList)
        {
            var theorFreqList = new List<double>();
            var m = ExpectationValue(midList, HitRateList);
            var s = Dispersion(midList, HitRateList);
            var min = _experimentalDataList.Min();
            var max = _experimentalDataList.Max();
            _k = Convert.ToInt32(numK.Value);
            var h = (max - min) / _k;
            var n = _experimentalDataList.Count;
            var temp = new List<double>();

            foreach (double x in midList)
            {
                var u = GetU(x, m, s);
                temp.Add(u);
                theorFreqList.Add(n*h* DensityOfGaussian(u)/Math.Sqrt(s));
            }
            return theorFreqList;
        }

        private List<double> TheoreticalFrequecyValuesSecond(List<double> midList)
        {
            var theorFreqList = new List<double>();
            _k = Convert.ToInt32(numK.Value);
            var fui = new double[_k + 1];
            var m = ExpectationValue(midList, HitRateList);
            var s = Dispersion(midList, HitRateList);
            var min = _experimentalDataList.Min();
            var max = _experimentalDataList.Max();

            var h = (max - min) / _k;
            var n = _experimentalDataList.Count;

            var normedValues = midList.Select(x => GetU(x, m, s)).ToList();
            fui[0] = 0;
            for (var i = 0; i < _k-1; i++)
            {
                fui[i + 1] = GaussianFunction(normedValues[i]);
            }
            fui[_k] = GaussianFunction(double.PositiveInfinity);
            for (var i = 0; i < _k; i++)
            {
                theorFreqList.Add(n * (fui[i+1] - fui[i]));
            }
            return theorFreqList;
        }
        private double XiSqr(List<double> theorFreq, List<double> freq)
        {
            return theorFreq.Select((t, i) => Math.Pow((freq[i] - t), 2)/t).Sum();
        }

        private double LambdaQ(double alpha)
        {
            double[] c = { 2.515517, 0.8028538, 0.01032 };
            double[] d = { 1.432788, 0.189269, 0.001308 };
            var t = Math.Sqrt(Math.Log(Math.Pow(alpha, -2)));
            var res = t - ((c[0] + c[1] * t + c[2] * Math.Pow(t, 2)) / (1 + d[0] * t + d[1] * Math.Pow(t, 2) + d[2] * Math.Pow(t, 3)));
            return res;
        }

        public double XiSqr(double q, int v)
        {
            var lambdaq = LambdaQ(q);
            var res = v * Math.Pow((1 - (2.0 / (9 * v)) + lambdaq * Math.Sqrt(2.0 / (9 * v))), 3);
            return res;
        }


    }
}
