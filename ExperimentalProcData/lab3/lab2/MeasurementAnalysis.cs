using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using  MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearRegression;

namespace lab2
{
    public class MeasurementAnalysis
    {

        public double DispersionOfPerturbations(double[] yList, List<double[]> xLists, double[] bList, int m, bool isPolynom)
        {
            var y = DenseMatrix.OfColumnArrays(yList);
            var x  = Matrix<double>.Build.Dense(xLists[0].Length, 1, (i, j) => 1.0);
            var newM = isPolynom ? m : xLists.Count;
            for (var i = 1; i < newM + 1; i++)
            {
                var temp = isPolynom ? xLists[0].Select(elem => Math.Pow(elem, i)).ToArray() : xLists[i - 1];
                x = x.Append(DenseMatrix.OfColumnArrays(temp));
            }
            var b = DenseMatrix.OfColumnArrays(bList);
            var s = (y - x * b).Transpose() * (y - x * b) / (xLists[0].Length - newM);
            return s.ToColumnMajorArray()[0];
        }


        public Matrix<double> ErrorMatrix(List<double[]> xLists)
        {
            var x = Matrix<double>.Build.Dense(xLists[0].Length, 1, (i, j) => 1.0);
            for (var i = 1; i < xLists.Count + 1; i++)
            {
                x = x.Append(DenseMatrix.OfColumnArrays(xLists[i - 1]));
            }
            var s = (x.Transpose()*x).Inverse();
            return s;
        }

        public double DistributionOfStudent(double alfa, int v)
        {
            var lambda = GetQuantile(alfa);
            var sqrLambda = Math.Pow(lambda, 2);
            var q1 = ((sqrLambda + 1) * lambda) / 4;
            var q2 = (((5 * sqrLambda + 16) * sqrLambda + 3) * lambda) / 96;
            var q3 = ((((3 * sqrLambda + 19) * sqrLambda + 17) * sqrLambda - 15) * lambda) / 384;
            var q4 = (((((79 * sqrLambda + 776) * sqrLambda + 1482) * sqrLambda - 1920) * sqrLambda - 945) * lambda) / 92160;
            var sd = lambda + ((q1 / v) + (q2 / Math.Pow(v, 2)) + (q3 / Math.Pow(v, 3)) + (q4 / Math.Pow(v, 4)));
            return sd;
        }


        private double ExpectationValue(List<double> list)
        {
            var value = list.Sum();
            return (value / list.Count);
        }
        private double GetQuantile(double alfa)
        {
            double[] c = { 2.515517, 0.8028538, 0.01032 };
            double[] d = { 1.432788, 0.189269, 0.001308 };
            var t = Math.Sqrt(Math.Log(Math.Pow(alfa, -2)));
            var sqrT = Math.Pow(t, 2);
            return Math.Abs(t - ((c[0] + c[1] * t + c[2] * sqrT) / (1 + d[0] * t + d[1] * sqrT + d[2] * Math.Pow(t, 3))));
        }
        public double FisherQuantile(double v1, double v2, double alfa)
        {
            var lambda = GetQuantile(alfa);
            var h = 2*(v1 - 1)*(v2 - 1)/(v1 + v2 - 2);
            var l = (Math.Pow(lambda, 2) - 3)/6;
            var w = (lambda*Math.Sqrt(h + l))/h - (1/(v1 - 1) - 1/(v2 - 1))*(l + 5/6.0 - 2/(3*h));
            return Math.Exp(2*w);
        }
        public double Dispersion(List<double> yList)
        {
            var expectationValue = ExpectationValue(yList);
            var value = yList.Sum(item => Math.Pow((item - expectationValue), 2));
            return (value / (yList.Count-1));
        }

        public double[] LinearRegression(List<double[]> xLists , double[] yList)
        {
            var y = DenseVector.OfEnumerable(yList);
            var x = Matrix<double>.Build.Dense(xLists[0].Length, 1, (i, j) => 1.0);
            for (var i = 1; i < xLists.Count+1; i++)
            {
                x = x.Append(DenseMatrix.OfColumnArrays(xLists[i-1]));
            }
            return MultipleRegression.NormalEquations(x, y).ToArray();
        }
    }
}
