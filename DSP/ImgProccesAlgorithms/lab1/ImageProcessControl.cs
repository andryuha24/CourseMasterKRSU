using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Drawing;
using MetroFramework.Forms;
using MetroFramework.Controls;
using System.Windows.Forms;

using System.Data;
using System.Drawing.Imaging;

namespace lab1
{
    public class ImageProcessControl
    {
        private int sizeMatrix = 3;
        public static double coeff = 1.0 / 9;
        
        private double[,] MultMatrixNumb(double[,]a, double n)
        {
            for(int i=0; i< sizeMatrix; i++)
            {
                for (int j = 0; j < sizeMatrix; j++)
                    a[i, j] *= n;
            }
            return a;
        }

        public Image Smoothing(Image img, MetroGrid datagrid)
        {
            Bitmap bmp = new Bitmap(img);
            double[,] matrix = new double[3, 3] { { 1, 1, 1 }, { 1, 1, 1, }, { 1, 1, 1 } };
            matrix = MultMatrixNumb(matrix, coeff);
            FillGridView(datagrid, matrix);
            bmp = FilterForImage(bmp, matrix);
            return bmp;
        }
        private void FillGridView(MetroGrid datagrid, double[,]matrix)
        {
            datagrid.Rows.Clear();
            datagrid.ColumnCount = sizeMatrix;
            for (int r = 0; r < sizeMatrix; r++)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(datagrid);
                for (int c = 0; c < sizeMatrix; c++)
                {
                    row.Cells[c].Value = Math.Round(matrix[r, c],3);
                }
                datagrid.Rows.Add(row);
            }
        }

        public Image EdgeEmphasize(Image img, MetroGrid datagrid, int ver)
        {
            Bitmap bmp = new Bitmap(img);
            double[,]matrixEdge1 = new double[3, 3] { { -1, -1, -1 }, { -1, 9, -1, }, { -1, -1, -1 } };
            double[,] matrixEdge2 = new double[3, 3] { { -1, -1, -1 }, { -1, 17, -1, }, { -1, -1, -1 } };
            matrixEdge2 = MultMatrixNumb(matrixEdge2, coeff);
            double[,] matrixEdge3 = new double[3, 3] { { 0, -1, 0 }, { -1, 5, -1, }, { 0, -1, 0 } };
            double[,] tempMatrix = new double[3, 3];
            switch(ver)
            {
                case 1: tempMatrix = matrixEdge1; break;
                case 2: tempMatrix = matrixEdge2; break;
                case 3: tempMatrix = matrixEdge3; break;
            }
            FillGridView(datagrid, tempMatrix);
            bmp = FilterForImage(bmp, tempMatrix);
            return bmp;
        }

        public Image AllocateEdge(Image img, MetroGrid datagrid, int ver)
        {
            Bitmap bmp = new Bitmap(img);
            double[,] matrixEdge1 = new double[3, 3] { { 0, -1, 0 }, { -1, 4, -1, }, { 0, -1, 0 } };
            double[,] matrixEdge2 = new double[3, 3] { { -1, -1, -1 }, { -1, 8, -1, }, { -1, -1, -1 } };
            double[,] tempMatrix = new double[3, 3];
            switch (ver)
            {
                case 1: tempMatrix = matrixEdge1; break;
                case 2: tempMatrix = matrixEdge2; break;
            }
            FillGridView(datagrid, tempMatrix);
            bmp = FilterForImage(bmp, tempMatrix);
            return bmp;
        }

        public Image MedianFilterImage(Image img, MetroGrid datagrid)
        {
            Bitmap bmp = new Bitmap(img);
            datagrid.Rows.Clear();
            bmp = MedianFilter(bmp, 5);
            return bmp;
        }

        public Image StampingImage(Image img, MetroGrid datagrid, int ver)
        {
            Bitmap bmp = new Bitmap(img);
            double[,] matrixEdge1 = new double[3, 3] { { 0, 1, 0 }, { -1, 0, 1, }, { 0, -1, 0 } };
            double[,] matrixEdge2 = new double[3, 3] { { 0, -1, 0 }, { 1, 0, -1, }, { 0, 1, 0 } };
            double[,] tempMatrix = new double[3, 3];
            switch (ver)
            {
                case 1: tempMatrix = matrixEdge1; break;
                case 2: tempMatrix = matrixEdge2; break;
            }
            FillGridView(datagrid, tempMatrix);
            bmp = FilterForImage(bmp, tempMatrix);
            bmp = StampingImageSecond(bmp);
            return bmp;
        }

        public Image WatercolorImage(Image img, MetroGrid datagrid)
        {
            Bitmap bmp = new Bitmap(img);
            datagrid.Rows.Clear();
            double[,] matrixEdge = new double[3, 3] { { -1, -1, -1 }, { -1, 9, -1, }, { -1, -1, -1 } };
           
            bmp = MedianFilter(bmp, 5);
            bmp = FilterForImage(bmp, matrixEdge);
            return bmp;
        }


        private double[,] FillMatrix(int var)
        {
            double[,] procMatrix = new double[sizeMatrix, sizeMatrix];

            for (int i = 0; i < sizeMatrix; i++)
            {
                for (int j = 0; j < sizeMatrix; j++)
                {
                    procMatrix[i, j] = coeff*1;
                }   
            }
            return procMatrix;
        }

        private Bitmap FilterForImage(Bitmap image, double[,] mask)
        {
            Bitmap srcImage = (Bitmap)image.Clone();

            int tempX, tempY;
            int offset = (int)Math.Sqrt(mask.Length) / 2;

            double sumR, sumG, sumB;

            //размер пикселя в байтах
            int pixelSize = Bitmap.GetPixelFormatSize(image.PixelFormat) / 8;

            BitmapData imageData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.WriteOnly, image.PixelFormat);
            BitmapData srcImageData = srcImage.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);

            // scan0 - указатель на первый пиксель
            // stride - длинна одной строки пикселей в байтах по всей ширине изображения
            int stride = srcImageData.Stride;
            IntPtr scan0 = srcImageData.Scan0;

            unsafe
            {
                byte* tempPixel;

                for (int y = offset; y < srcImageData.Height - offset; y++)
                {
                    for (int x = offset; x < srcImageData.Width - offset; x++)
                    {
                        sumR = sumG = sumB = 0;

                        //свертка
                        for (int mY = 0; mY < sizeMatrix; mY++)
                        {
                            for (int mX = 0; mX < sizeMatrix; mX++)
                            {
                                // пиксели вокруг
                                tempX = x + mX - offset;
                                tempY = y + mY - offset;
                                tempPixel = (byte*)scan0 + (tempY * stride) + (tempX * pixelSize);
                                sumR += *(tempPixel + 2) * mask[mY, mX];
                                sumG += *(tempPixel + 1) * mask[mY, mX];
                                sumB += *tempPixel * mask[mY, mX];

                            }
                        }
                        if (sumR < 0) sumR = 0;
                        if (sumG < 0) sumG = 0;
                        if (sumB < 0) sumB = 0;
                        if (sumR > 255) sumR = 255;
                        if (sumG > 255) sumG = 255;
                        if (sumB > 255) sumB = 255;

                        byte* newpixel = (byte*)imageData.Scan0 + (y * imageData.Stride) + (x * pixelSize);
                        *newpixel = (byte)sumB;
                        *(newpixel + 1) = (byte)sumG;
                        *(newpixel + 2) = (byte)sumR;
                    }
                }
                image.UnlockBits(imageData);
                srcImage.UnlockBits(srcImageData);
            }
            return image;
        }

        private Bitmap StampingImageSecond(Bitmap image)
        {
            Bitmap srcImage = (Bitmap)image.Clone();
            double sumR, sumG, sumB;

            //размер пикселя в байтах
            int pixelSize = Bitmap.GetPixelFormatSize(image.PixelFormat) / 8;
            BitmapData imageData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.WriteOnly, image.PixelFormat);
            BitmapData srcImageData = srcImage.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);
            int stride = srcImageData.Stride;
            IntPtr scan0 = srcImageData.Scan0;

            unsafe
            {
                byte* tempPixel;

                for (int y = 0; y < srcImageData.Height; y++)
                {
                    for (int x = 0; x < srcImageData.Width; x++)
                    {
                        sumR = sumG = sumB = 0;
                        tempPixel = (byte*)scan0 + (y * stride) + (x * pixelSize);
                        sumR += *(tempPixel + 2)+128;
                        sumG += *(tempPixel + 1)+128;
                        sumB += *tempPixel +128;

                        if (sumR < 0) sumR = 0;
                        if (sumG < 0) sumG = 0;
                        if (sumB < 0) sumB = 0;
                        if (sumR > 255) sumR = 255;
                        if (sumG > 255) sumG = 255;
                        if (sumB > 255) sumB = 255;

                        byte* newpixel = (byte*)imageData.Scan0 + (y * imageData.Stride) + (x * pixelSize);
                        *newpixel = (byte)sumB;
                        *(newpixel + 1) = (byte)sumG;
                        *(newpixel + 2) = (byte)sumR;
                    }
                }
                image.UnlockBits(imageData);
                srcImage.UnlockBits(srcImageData);
            }
            return image;
        }

        private Bitmap MedianFilter(Bitmap image, int n)
        {
            Bitmap srcImage = (Bitmap)image.Clone();

            int tempX, tempY;
            int offset = n / 2;
            //размер пикселя в байтах
            int pixelSize = Bitmap.GetPixelFormatSize(image.PixelFormat) / 8;

            BitmapData imageData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.WriteOnly, image.PixelFormat);
            BitmapData srcImageData = srcImage.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);

            // scan0 - указатель на первый пиксель
            // stride - длинна одной строки пикселей в байтах по всей ширине изображения
            int stride = srcImageData.Stride;
            IntPtr scan0 = srcImageData.Scan0;

            unsafe
            {
                byte* tempPixel;

                for (int y = offset; y < srcImageData.Height - offset; y++)
                {
                    for (int x = offset; x < srcImageData.Width - offset; x++)
                    {
                        List<double> sumR = new List<double>();
                        List<double> sumG = new List<double>();
                        List<double> sumB = new List<double>();

                        //свертка
                        for (int mY = 0; mY < n; mY++)
                        {
                            for (int mX = 0; mX < n; mX++)
                            {
                                // пиксели вокруг
                                tempX = x + mX - offset;
                                tempY = y + mY - offset;
                                tempPixel = (byte*)scan0 + (tempY * stride) + (tempX * pixelSize);
                                sumR.Add(*(tempPixel + 2));
                                sumG.Add(*(tempPixel + 1));
                                sumB.Add(*tempPixel);
                            }
                        }
                        sumR.Sort();
                        sumG.Sort();
                        sumB.Sort();
                        byte* newpixel = (byte*)imageData.Scan0 + (y * imageData.Stride) + (x * pixelSize);
                        *newpixel = (byte)sumB[sumB.Count/2];
                        *(newpixel + 1) = (byte)sumG[sumG.Count / 2];
                        *(newpixel + 2) = (byte)sumR[sumR.Count / 2];
                    }
                }
                image.UnlockBits(imageData);
                srcImage.UnlockBits(srcImageData);
            }
            return image;
        }



    }
}
