/*      SlopeAlgorithm Class
 *	    AUTHOR: STEWART TAYLOR
 *------------------------------------
 * This abstract provides a base for slope algorithms
 * 
 * Last Updated: 16/03/2013
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicPathPlanner.Code
{
    abstract class SlopeAlgrotithm
    {
        protected double[,] heightMap;
        protected double[,] slopeModel;
        protected int height;
        protected int width;

        public static String ALGORITHM;

        public SlopeAlgrotithm(double[,] model)
        {
            heightMap = model;
            width = heightMap.GetLength(0);
            height = heightMap.GetLength(1);

            generateSlopeModel();
        }


        protected void generateSlopeModel()
        {
            slopeModel = new double[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    slopeModel[x, y] = calculateSlopeValue(x, y);
                }
            }
        }

        protected virtual double calculateSlopeValue(int x, int y)
        {
            double topLeft = getTopLeftSlope(x, y);
            double topMid = getTopMiddleSlope(x, y);
            double topRight = getTopRightSlope(x, y);

            double midLeft = getMiddleLeftSlope(x, y);
            double midRight = getMiddleRightSlope(x, y);

            double botLeft = getBottomLeftSlope(x, y);
            double botMid = getBottomMiddleSlope(x, y);
            double botRight = getBottomRightSlope(x, y);

            double high = 0f;

            if (topLeft >= high) { high = topLeft; }
            if (topMid >= high) { high = topMid; }
            if (topRight >= high) { high = topRight; }
            if (midLeft >= high) { high = midLeft; }
            if (midRight >= high) { high = midRight; }
            if (botLeft >= high) { high = botLeft; }
            if (botMid >= high) { high = botMid; }
            if (botRight >= high) { high = botRight; }

            double total = topLeft + topRight + topMid + midLeft + midRight + botLeft + botMid + botRight;

            double value = high;

            return value;
        }


        private double getGradientValue(double x, double y)
        {
            double gradient = 0;

            if ((x > 0) && (y > 0))
            {
                gradient = x - y;
                gradient = Math.Abs(gradient);
            }

            return gradient;

        }


        #region slope Calcs

        protected double getTopLeftSlope(int x, int y)
        {
            double gradient = 0;

            if ((x > 0) && (y > 0))
            {
                gradient = getGradientValue(heightMap[x - 1, y - 1], heightMap[x, y]);
            }

            return gradient;
        }

        protected double getTopMiddleSlope(int x, int y)
        {
            double gradient = 0;

            if (y > 0)
            {
                gradient = getGradientValue(heightMap[x, y - 1], heightMap[x, y]);
            }

            return gradient;
        }

        protected double getTopRightSlope(int x, int y)
        {
            double gradient = 0;

            if ((x < width - 1) && (y > 0))
            {
                gradient = getGradientValue((float)heightMap[x + 1, y - 1], (float)heightMap[x, y]);
            }

            return gradient;
        }

        protected double getMiddleLeftSlope(int x, int y)
        {
            double gradient = 0;

            if (x > 0)
            {
                gradient = getGradientValue((float)heightMap[x - 1, y], (float)heightMap[x, y]);
            }

            return gradient;

        }

        protected double getMiddleRightSlope(int x, int y)
        {
            double gradient = 0;


            if (x < width - 1)
            {
                gradient = getGradientValue((float)heightMap[x + 1, y], (float)heightMap[x, y]);
            }

            return gradient;
        }

        protected double getBottomLeftSlope(int x, int y)
        {
            double gradient = 0;

            if ((x > 0) && (y < height - 1))
            {
                gradient = getGradientValue((float)heightMap[x - 1, y + 1], (float)heightMap[x, y]);
            }

            return gradient;
        }

        protected double getBottomMiddleSlope(int x, int y)
        {
            double gradient = 0;

            if (y < height - 1)
            {
                gradient = getGradientValue(heightMap[x, y + 1], heightMap[x, y]);
            }

            return gradient;
        }

        protected double getBottomRightSlope(int x, int y)
        {
            double gradient = 0;

            if ((x < width - 1) && (y < height - 1))
            {
                gradient = getGradientValue((float)heightMap[x + 1, y + 1], (float)heightMap[x, y]);
            }

            return gradient;
        }



        #endregion



        public double[,] getSlopeModel()
        {
            return slopeModel;
        }

    }
}
