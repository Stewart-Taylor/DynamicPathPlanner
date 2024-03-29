﻿/*      ElevationModel Class
 *	    AUTHOR: STEWART TAYLOR
 *------------------------------------
 * This class is used to generate an elevation model
 * The model will be fetched using the Elevation Loader
 *
 * Last Updated: 21/04/2013
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Media;
using System.IO;
using System.Windows.Media.Imaging;

namespace DynamicPathPlanner.Code
{
    class ElevationModel
    {
        private double maxHeight = -1;
        private double minHeight = 0;
        private int width;
        private int height;
        private double[,] elevationModel;

        private ElevationLoader loader = new ElevationLoader();
        private Bitmap image;

        #region GET

        public double[,] getModel()
        {
            return elevationModel;
        }

        public Bitmap getBitmap()
        {
            return image;
        }

        private float getHeight(int red, int green)
        {
            float height = 0;
            height = ((red * 256 + green));

            return height;
        }

        public ImageSource getImageSource()
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();
            ImageSource img = bi;

            return img;
        }

        #endregion

        public ElevationModel()
        {

        }

        public void load(String environment, float distance, int size)
        {
            elevationModel = loader.load_DEM(environment, distance, size);
            width = elevationModel.GetLength(0);
            height = elevationModel.GetLength(1);

            processModel();
            generateBitmap();
        }

        private void processModel()
        {
            for (int x =0; x < width; x++)
            {
                for (int y =0; y < height; y++)
                {

                     elevationModel[x, y] = elevationModel[x, y] * 100;
                }
            }
        }

        public void load_PANGU_DEM(int distance, int size)
        {
            elevationModel = loader.getPANGU_DEM("", distance , size);
            width = elevationModel.GetLength(0);
            height = elevationModel.GetLength(0);

            generateBitmap();
        }

        private void generateBitmap()
        {
            setMaxHeight();
            setMinHeight();
            Bitmap bitmap = new Bitmap(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    double heightTemp = elevationModel[x, y];
                    int value = getHeightColor(heightTemp);

                    System.Drawing.Color tempColor = System.Drawing.Color.FromArgb(255, value, value, value);
                    bitmap.SetPixel(x, y, tempColor);
                }
            }
            image = bitmap;
        }


        public int getHeightColor(double height)
        {
            double valueR = height - minHeight;
            double valueT = (valueR / (maxHeight - minHeight));
            double tempNumber = valueT * 255f;

            return (int)tempNumber;
        }

        public int getColorValue(String hex)
        {
            int tempNumber = int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
            return tempNumber;
        }

        private void setMaxHeight()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (elevationModel[x, y] > maxHeight)
                    {
                        maxHeight = elevationModel[x, y];
                    }
                }
            }
        }

        private void setMinHeight()
        {
            minHeight = elevationModel[0, 0];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (elevationModel[x, y] < minHeight)
                    {
                        minHeight = elevationModel[x, y];
                    }
                }
            }
        }

    }
}