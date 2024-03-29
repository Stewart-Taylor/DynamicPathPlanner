﻿/*      SlopeModel Class
 *	    AUTHOR: STEWART TAYLOR
 *------------------------------------
 * This class is used to generate a slope model
 * It also generates A visual output of the slope model
 *
 * Last Updated: 21/04/2013
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.IO;
using System.Windows.Media.Imaging;
using System.Drawing;
using DynamicPathPlanner.Code.Slope;

namespace DynamicPathPlanner.Code
{
    class SlopeModel
    {
        private int width;
        private int height;
        private double lowestGradient;
        private double highestGradient;
        private double[,] model;
        private double[,] slopeModel;
        private Bitmap slopeBitmap;
        private string algorithmType;

        #region GET

        public String getSlopeAlgorithm()
        {
            return algorithmType;
        }

        public double[,] getModel()
        {
            return slopeModel;
        }

        public Bitmap getBitmap()
        {
            return slopeBitmap;
        }

        public ImageSource getImageSource()
        {
            MemoryStream ms = new MemoryStream();
            slopeBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();

            return bi;
        }

        #endregion

        public SlopeModel(double[,] modelT, string type)
        {
            model = modelT;
            algorithmType = type;
            algorithmType = algorithmType.ToUpper();
            width = model.GetLength(0);
            height = model.GetLength(1);

            generateSlopeModel();
        }


        private void generateSlopeModel()
        {
            slopeModel = new double[model.GetLength(0), model.GetLength(1)];

            SlopeAlgrotithm slopeAlgortihm;

            if (algorithmType == SlopeMax.ALGORITHM)
            {
                slopeAlgortihm = new SlopeMax(model);
            }
            else if (algorithmType == SlopeAverage.ALGORITHM)
            {
                slopeAlgortihm = new SlopeAverage(model);
            }
            else if (algorithmType == SlopeHorn.ALGORITHM)
            {
                slopeAlgortihm = new SlopeHorn(model);
            }
            else
            {
                slopeAlgortihm = new SlopeAverage(model);
            }

            slopeModel = slopeAlgortihm.getSlopeModel();

            generateSlopeModelImage();
        }


        private void generateSlopeModelImage()
        {
            Bitmap bitmap = new Bitmap(model.GetLength(0), model.GetLength(1));
            getGradientLimits();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    System.Drawing.Color tempColor = getSlopeColorValue(slopeModel[x, y]);
                    bitmap.SetPixel(x, y, tempColor);
                }
            }

            slopeBitmap = bitmap;
        }

        //Must be called before image generation
        private void getGradientLimits()
        {
            double low = slopeModel[0, 0];
            double high = slopeModel[0, 0];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (low > slopeModel[x, y])
                    {
                        low = slopeModel[x, y];
                    }

                    if (high < slopeModel[x, y])
                    {
                        high = slopeModel[x, y];
                    }
                }
            }

            lowestGradient = low;
            highestGradient = high;
        }


        private System.Drawing.Color getSlopeColorValue(double gradient)
        {
            System.Drawing.Color color = System.Drawing.Color.White;
            gradient = Math.Abs(gradient);

            float green = 255;
            float red = 255;
            float blue = 0;

            if (gradient <= 1f)
            {
                red = (1f - (float)gradient) * 255;
            }
            else if (gradient <= 3f)
            {
                float percent = ((float)gradient) / (3f);
                green = (1f - percent) * 255;
            }
            else
            {
                green = 0;
            }

            color = System.Drawing.Color.FromArgb(255, (int)red, (int)green, (int)blue);

            return color;
        }




    }
}

