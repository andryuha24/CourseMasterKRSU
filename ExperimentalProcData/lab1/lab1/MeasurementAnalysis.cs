using System;
using System.Collections.Generic;
using System.Linq;

namespace lab1
{
    class MeasurementAnalysis
    {
        //мат.ожидание
        public double ExpectationValue(List<double> list)
        {
            var value = list.Sum();
            return (value / list.Count);
        }

        //дисперсия
        public double Dispersion(List<double> list, double expectationValue)
        {
            var value = list.Sum(item => Math.Pow((item - expectationValue), 2));
            return (value / list.Count);
        }

        public double GetU(double maxDeviation, double expectationValue, double dispersion)
        {
            return Math.Abs((maxDeviation - expectationValue) / Math.Sqrt(dispersion));
        }

        public  double GetQuantile(double alfa)
        {
            double[] c = { 2.515517, 0.8028538, 0.01032 };
            double[] d = { 1.432788, 0.189269, 0.001308 };
            var t = Math.Sqrt(Math.Log(Math.Pow(alfa, -2)));
            var sqrT = Math.Pow(t, 2);
            return Math.Abs(t - ((c[0] + c[1] * t + c[2] * sqrT) / (1 + d[0] * t + d[1] * sqrT + d[2] * Math.Pow(t, 3))));
        }

        public  double DistributionOfStudent(double lambda, int v)
        {
            var sqrLambda = Math.Pow(lambda, 2);
            var q1 = ((sqrLambda + 1) * lambda) / 4;
            var q2 = (((5 * sqrLambda + 16) * sqrLambda + 3) * lambda) / 96;
            var q3 = ((((3 * sqrLambda + 19) * sqrLambda + 17) * sqrLambda - 15) * lambda) / 384;
            var q4 = (((((79 * sqrLambda + 776) * sqrLambda + 1482) * sqrLambda - 1920) * sqrLambda - 945) * lambda) / 92160;
           var sd = lambda + ((q1 / v) + (q2 / Math.Pow(v, 2)) + (q3 / Math.Pow(v, 3)) + (q4 / Math.Pow(v, 4)));
            return sd;
        }

        public  double GetK(double item, List<double> list)
        {
            var _list = new List<double>(list);
            _list.Remove(item);
            var m = ExpectationValue(_list);
            var s = Math.Sqrt(Dispersion(_list, m));
            return Math.Abs((item - m) / s);
        }

    }
}
