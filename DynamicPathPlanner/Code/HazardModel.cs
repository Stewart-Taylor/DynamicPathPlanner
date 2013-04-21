/*      HazardModel Class
 *	    AUTHOR: STEWART TAYLOR
 *------------------------------------
 * This class generates a hazard map
 * It requires the data from the slope model
 * A sector size is also required to determine the resolution
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
using System.Drawing.Imaging;

namespace DynamicPathPlanner.Code
{
    class HazardModel
    {
        private int sectorSize;
        private int slopeWidth;
        private int slopeHeight;
        private int width;
        private int height;

        private int[,] hazardModel;
        public double[,] hazardModelImage;
        private double[,] slopeModel;

        private Bitmap hazardBitmap;

        #region GET

        public int[,] getModel()
        {
            return hazardModel;
        }

        public double[,] getHazardSlope()
        {
            return hazardModelImage;
        }

        public Bitmap getBitmap()
        {
            return hazardBitmap;
        }

        public int getSectorSize()
        {
            return sectorSize;
        }

        public int getWidth()
        {
            return width;
        }

        public int getHeight()
        {
            return height;
        }

        #endregion

        public HazardModel(double[,] slope, int sectorSizeT)
        {
            slopeModel = slope;
            sectorSize = sectorSizeT;
            slopeWidth = slopeModel.GetLength(0);
            slopeHeight = slopeModel.GetLength(1);

            generateHazardModel();
            generateHazardImage();
        }


        private void addHazardPadding()
        {
            int hazardThreshold = 20;
            int padValue = 2;

            int width = hazardModel.GetLength(0);
            int height = hazardModel.GetLength(1);

            int[,] padMap = hazardModel;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (getMaxAdjacentValue(x, y) > hazardThreshold)
                    {
                        if (hazardModel[x, y] < padValue)
                        {
                            padMap[x, y] = padValue;
                        }
                        else
                        {
                            padMap[x, y] = hazardModel[x, y];
                        }
                    }
                }
            }

            hazardModel = padMap;
        }


        private int getMaxAdjacentValue(int x , int y)
        {
            int topLeft = getTileValue(x-1, y-1);
            int topMid = getTileValue(x, y-1);
            int topRight = getTileValue(x+1, y-1);

            int midLeft = getTileValue(x-1, y);
            int midRight = getTileValue(x+1, y);

            int botLeft = getTileValue(x-1, y+1);
            int botMid = getTileValue(x, y+1);
            int botRight = getTileValue(x+1, y+1);

            int high = 0;

            if (topLeft >= high) { high = topLeft; }
            if (topMid >= high) { high = topMid; }
            if (topRight >= high) { high = topRight; }
            if (midLeft >= high) { high = midLeft; }
            if (midRight >= high) { high = midRight; }
            if (botLeft >= high) { high = botLeft; }
            if (botMid >= high) { high = botMid; }
            if (botRight >= high) { high = botRight; }

            return high;
        }

        private int getTileValue(int x , int y)
        {
            int value = 0;

            if( (x > 0) && ( x < width))
            {
                if ( (y>0) && (y < height))
                {
                    return hazardModel[x,y];
                }
            }

            return value;
        }

        private void generateHazardModel()
        {
            width = slopeWidth / sectorSize;
            height = slopeHeight / sectorSize;
            hazardModel = new int[width, height];
            double[,] tempModel = new double[width, height];
            hazardModelImage = new double[width, height];

            int slopeX = 0;
            int slopeY = 0;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    tempModel[x, y] = slopeModel[slopeX, slopeY];

                    for (int a = 0; a < sectorSize; a++)
                    {
                        for (int b = 0; b < sectorSize; b++)
                        {
                            if (tempModel[x, y] < slopeModel[slopeX + a, slopeY + b])
                            {
                                tempModel[x, y] = slopeModel[slopeX + a, slopeY + b];
                            }
                        }
                    }

                    slopeY += sectorSize;
                }
                slopeY = 0;
                slopeX += sectorSize;
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    hazardModelImage[x, y] = tempModel[x, y];
                    hazardModel[x, y] = getHazardValue(tempModel[x, y]);
                }
            }
        }


        private int getHazardValue(double value)
        {
            if (value <= 0.8)
            {
                return 1;
            }
            else if (value <= 1)
            {
                return 4;
            }
            else
            {
                return 100;
            }
        }

        private void generateHazardImage()
        {

            int slopeX = 0;
            int slopeY = 0;

            Bitmap bitmap = new Bitmap(slopeWidth, slopeHeight);
            BitmapHelper bMap = new BitmapHelper(bitmap);
            bMap.LockBitmap();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    System.Drawing.Color tempColor = getHazardColorValue(hazardModelImage[x, y]);

                    bMap.SetPixel(slopeX, slopeY, tempColor);
                    for (int a = 0; a < sectorSize; a++)
                    {
                        for (int b = 0; b < sectorSize; b++)
                        {
                            bMap.SetPixel(slopeX + a, slopeY + b, tempColor);
                        }
                    }

                    slopeY += sectorSize;
                }
                slopeY = 0;
                slopeX += sectorSize;
            }

            bMap.UnlockBitmap();

            hazardBitmap = bMap.Bitmap;
        }


        private System.Drawing.Color getHazardColorValue(double gradient)
        {
            System.Drawing.Color color = System.Drawing.Color.White;
            gradient = Math.Abs(gradient);

            float green = 255;
            float red = 255;
            float blue = 0;

            if (gradient <= 1f)
            {
                red = (1 - (float)gradient) * 255;
            }
            else if (gradient <= 3f)
            {
                double percent = (gradient) / (3f);
                green = (1 - (float)percent) * 255;
            }
            else
            {
                green = 0;
            }

            return System.Drawing.Color.FromArgb(255, (int)red, (int)green, (int)blue);
        }



        public ImageSource getImageSource()
        {
            MemoryStream ms = new MemoryStream();
            hazardBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();

            return bi;
        }

    }
}