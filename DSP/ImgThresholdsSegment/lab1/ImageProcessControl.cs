using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using ZedGraph;

namespace lab1
{
    public class ImageProcessControl
    {
   
        private int[] sumR = new int[256];
        private int[] sumG = new int[256];
        private int[] sumB = new int[256];
        private int[] sumI = new int[256];

        private Random rnd = new Random();

        public int[] getRChanal()
        {
            return sumR;
        }
        public int[] getGChanal()
        {
            return sumG;
        }
        public int[] getBChanal()
        {
            return sumB;
        }

        public int[] getIChanal()
        {
            return sumI;
        }



        public void drawChart(PictureBox srcImage, ZedGraphControl gPanel, int numCh, int wFilter)
        {
            Bitmap bmp = new Bitmap(srcImage.Image);
            HistogramByChannels(bmp, wFilter);
            int[] colorValue = null;
            Color fillColor  = Color.Black;
            switch(numCh)
            {
                case 1: colorValue = getRChanal(); fillColor = Color.Red; break;
                case 2: colorValue = getGChanal(); fillColor = Color.Green; break;
                case 3: colorValue = getBChanal(); fillColor = Color.Blue; break;
                case 4: colorValue = getIChanal(); fillColor = Color.Black; break;
            }

            GraphPane pane = gPanel.GraphPane;
            pane.CurveList.Clear();
            pane.GraphObjList.Clear();
            
            PointPairList list = new PointPairList();
            for(int i = 0; i<256; i++)
            {
                list.Add(i, colorValue[i]);
            }

            LineItem hist = pane.AddCurve("", list, Color.Black, SymbolType.None);
            hist.Line.Fill = new ZedGraph.Fill(fillColor);
            pane.XAxis.Scale.FontSpec.Size = 10;
            pane.YAxis.Scale.FontSpec.Size = 10;
            pane.XAxis.Scale.MajorStep = 15.0;
            pane.XAxis.Scale.MinorStep = 3.0;
            pane.XAxis.Scale.Min = 0;
            pane.XAxis.Scale.Max = 255;
            pane.Title.IsVisible = false;
            pane.Legend.IsVisible = false;
            pane.XAxis.Title.IsVisible = false;
            pane.YAxis.Title.IsVisible = false;

            gPanel.AxisChange();
            gPanel.Invalidate();
        }
        private void HistogramByChannels(Bitmap image, int wFilter)
        {

            //размер пикселя в байтах
            int pixelSize = Bitmap.GetPixelFormatSize(image.PixelFormat) / 8;
            BitmapData imageData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.WriteOnly, image.PixelFormat);
            // scan0 - указатель на первый пиксель
            // stride - длинна одной строки пикселей в байтах по всей ширине изображения
            int stride = imageData.Stride;
            IntPtr scan0 = imageData.Scan0;
            int I = 0;
            unsafe
            {
                byte* tempPixel;
                Array.Clear(sumR, 0, sumR.Length);
                Array.Clear(sumG, 0, sumG.Length);
                Array.Clear(sumB, 0, sumB.Length);


                for (int y = 0; y < imageData.Height; y++)
                {
                    for (int x = 0; x < imageData.Width; x++)
                    {
                        tempPixel = (byte*)scan0 + (y * stride) + (x * pixelSize);
                        sumR[*(tempPixel + 2)]++;
                        sumG[*(tempPixel + 1)]++;
                        sumB[*tempPixel]++;
                        I = (*(tempPixel + 2) + *(tempPixel + 1) +  *tempPixel)/3;
                        sumI[I]++;
                    }
                }
                SmoothHistogram(sumR, wFilter);
                SmoothHistogram(sumG, wFilter);
                SmoothHistogram(sumB, wFilter);
                SmoothHistogram(sumI, wFilter);
                image.UnlockBits(imageData);
            }
        }

        private void SmoothHistogram(int[] channel, int n)
        {
           
            double k = 1.0 / (2*n+1);
            for(int i=0; i<channel.Length; i++)
            {
                int s = 0;
                for (int j =i-n; j<i+n+1;j++)
                {
                    if ((i + j) >= 0 && (i + j < 256))
                    {
                        s += channel[i + j];
                    }
                }
                channel[i] = Convert.ToInt32(s*k);
            }
        }

        private List<KeyValuePair<int,Color>> LocalMin(int[] arr)
        {
            List<KeyValuePair<int, Color>> temp = new List<KeyValuePair<int, Color>>();
   
            int min = arr[0];
            for (int i = 1; i < arr.Length-2; i++)
            {
                if (arr[i] < arr[i - 1] && arr[i + 1] > arr[i])
                    temp.Add(new KeyValuePair<int,Color>(i, Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256))));
            }
            temp.Insert(0, new KeyValuePair<int, Color>(0,temp[0].Value));
            temp.Insert(temp.Count, new KeyValuePair<int, Color>(255, temp[temp.Count-1].Value));
            return temp;
        }

        public Bitmap PaintImage(Bitmap image, int ch)
        {
            byte newR,newB,newG;
            
            Bitmap srcImage = (Bitmap)image.Clone();
            //размер пикселя в байтах
            int pixelSize = Bitmap.GetPixelFormatSize(image.PixelFormat) / 8;
            BitmapData imageData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.WriteOnly, image.PixelFormat);
            BitmapData srcImageData = srcImage.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);
            // scan0 - указатель на первый пиксель
            // stride - длинна одной строки пикселей в байтах по всей ширине изображения
            int stride = srcImageData.Stride;
            IntPtr scan0 = srcImageData.Scan0;

            List<KeyValuePair<int, Color>> id_min = null;
            switch(ch)
            {
                case 0: id_min = LocalMin(sumB); break;
                case 1: id_min = LocalMin(sumG); break;
                case 2: id_min = LocalMin(sumR); break;
                case 3: id_min = LocalMin(sumI); break;
            }
           // var id_minRed = LocalMin(sumR);

            unsafe
            {
                byte* tempPixel;
                int I = 0;
                for (int y = 0; y < imageData.Height; y++)
                {
                    for (int x = 0; x < imageData.Width; x++)
                    {
                        tempPixel = (byte*)scan0 + (y * stride) + (x * pixelSize);
                        newB=newG = newR = 0;
                        I = (*(tempPixel + 2) + *(tempPixel + 1) + *tempPixel) / 3;
                        for (int i = 0; i< id_min.Count;i++)
                        {
                            if(ch!=3)
                            {
                                if (id_min[i].Key <= *(tempPixel + ch) && *(tempPixel + ch) <= id_min[i + 1].Key)
                                {
                                    newB = (id_min[i + 1].Value.B);
                                    newG = (id_min[i + 1].Value.G);
                                    newR = (id_min[i + 1].Value.R);
                                    break;
                                }
                            }
                            else
                            {
                                if (id_min[i].Key < I && I <= id_min[i + 1].Key)
                                {
                                    newB = (id_min[i + 1].Value.B);
                                    newG = (id_min[i + 1].Value.G);
                                    newR = (id_min[i + 1].Value.R);
                                    break;
                                }
                            }

                       
                          
                        }

                        byte* newpixel = (byte*)imageData.Scan0 + (y * imageData.Stride) + (x * pixelSize);
                        *newpixel = newB;
                        *(newpixel + 1) = newG;
                        *(newpixel + 2) = newR;
                    }
                }
                image.UnlockBits(imageData);
                srcImage.UnlockBits(srcImageData);
            }
            return image;
        }

    }
}
