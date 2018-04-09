using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Windows.Forms.DataVisualization.Charting;

namespace lab1
{
    public class DFT
    {

        public static List<Complex> Generate(int N, double discrete, Func<Complex, Complex> func)
        {
            return Enumerable.Range(0, N).Select(n => func(discrete * n)).ToList();
        }


        public Complex[] DftDirect(Complex[] x, int N)
        {
          
           Complex[] X = new Complex[N];

            for (int k = 0; k < N; k++)
            {
                X[k] = 0;

                for (int n = 0; n < N-1; n++)
                {
                    X[k] += x[n] * Complex.Exp(-Complex.ImaginaryOne * 2.0 * Math.PI * (k * n) / N);
                }
            }

            return X;
        }

        public double[] IDft(Complex[] X)
        {
            int N = X.Length;
            double[] x = new double[N];

            for (int n = 0; n < N; n++)
            {
                Complex sum = 0;

                for (int k = 0; k < N; k++)
                {
                    sum += X[k] * Complex.Exp(Complex.ImaginaryOne * 2.0 * Math.PI * (k * n) /N);
                }

                x[n] = sum.Real/Convert.ToDouble(N); 
            }

            return x;
        }

        public void DrawGraph(Chart chart, double[] arr, int n)
        {
            for (int i = 0; i < n; i++)
            {
                chart.Series[0].Points.AddXY(i, arr[i]);
            }
        }
        public void DrawGraph(Chart chart, double[] arr, int n, double v)
        {
            for (int i = 0; i < n; i++)
            {
                chart.Series[0].Points.AddXY(i * v, arr[i]);
            }
        }


    }
}
