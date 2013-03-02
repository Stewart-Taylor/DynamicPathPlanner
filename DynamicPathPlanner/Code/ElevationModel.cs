﻿/*      ElevationModel Class
 *	    AUTHOR: STEWART TAYLOR
 *------------------------------------
 * This class is used to provide an elevation model
 * 
 *
 * Last Updated: 28/02/2013
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

        private double[,] elevationModel;


        private ElevationLoader loader = new ElevationLoader();

        private String format;
        private int width;
        private int height;
        private String[] hexMap;
        private int imageStartIndex;

        private bool p6Valid = true;
        private Bitmap image;

        private float verticalHeight = 0.1f; // 0.1m

        private double maxHeight = -1;
        private double minHeight = 0;




        public ElevationModel()
        {


        }



        public void load_PANGU_DEM()
        {
            elevationModel = loader.getPANGU_DEM();
            width = elevationModel.GetLength(0);
            height = elevationModel.GetLength(0);

            generateBitmap();
        }


        public void load_PPM_DEM()
        {
            elevationModel = loader.getPPM_DEM();
            width = elevationModel.GetLength(0);
            height = elevationModel.GetLength(0);

            generateBitmap();
        }


        public double[,] getModel()
        {
            return elevationModel;
        }
   

        private float getHeight(int red, int green)
        {
            float height = 0;
            height = ((red * 256 + green));

            return height;
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


        public Bitmap getBitmap()
        {
            return image;
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