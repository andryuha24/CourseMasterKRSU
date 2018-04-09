using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Windows.Forms.DataVisualization.Charting;

namespace lab1
{
    public class FFT
    {
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
        public static List<Complex> Generate(int N, double discrete, Func<Complex, Complex> func)
        {
            return Enumerable.Range(0, N).Select(n => func(discrete * n)).ToList();
        }

        public static List<Complex> Generate(int N, double discrete, Func<Complex, Complex> func1, Func<Complex, Complex> func2)
        {
            return Enumerable.Range(0, N).Select(n => n < N / 2 ? func1(discrete * n) : func2(discrete * n)).ToList();
        }

        private static Complex w(int k, int N)
        {
            double arg = 2 * Math.PI * k / N;
            return new Complex(Math.Cos(arg), Math.Sin(arg));
        }

        public  Complex[] Fft(Complex[] x, bool inverse)
        {
            Complex[] X;
            int N = x.Length;
            switch (N)
            {
                case 1:
                    return x;
                case 2:
                    X = new Complex[2];
                    X[0] = x[0] + x[1];
                    X[1] = x[0] - x[1];
                    break;
                default:
                    Complex[] x_even = new Complex[N / 2];
                    Complex[] x_odd = new Complex[N / 2];
                    for (int i = 0; i < N / 2; i++)
                    {
                        x_even[i] = x[2 * i];
                        x_odd[i] = x[2 * i + 1];
                    }
                    Complex[] X_even = Fft(x_even, inverse);
                    Complex[] X_odd = Fft(x_odd, inverse);
                    X = new Complex[N];
                    var inv = inverse ? 1 : -1;
                    for (int i = 0; i < N / 2; i++)
                    {
                        X[i] = X_even[i] + w(i*inv, N)*X_odd[i];
                        X[i + N/2] = X_even[i] - w(i*inv, N)*X_odd[i];
                    }
                    break;
            }
            return X;
        }

    }
}
