/*      SlopeHorn Class
 *	    AUTHOR: STEWART TAYLOR
 *------------------------------------
 * This class is used to generate a slope model from elevation data
 * It uses the HORN method. 
 *
 * Last Updated: 23/04/2013
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicPathPlanner.Code.Slope
{
    class SlopeHorn : SlopeAlgrotithm
    {
        public const String ALGORITHM = "HORN";

        public SlopeHorn(double[,] model)
            : base(model)
        {
            heightMap = model;
            width = heightMap.GetLength(0);
            height = heightMap.GetLength(1);

            generateSlopeModel();
        }

        protected override double calculateSlopeValue(int x, int y)
        {
            double horzSize = 0.1f;

            double a = getTopLeft(x, y) * horzSize;
            double b = getTopMiddle(x, y) * horzSize;
            double c = getTopRight(x, y) * horzSize;

            double d = getMiddleLeft(x, y) * horzSize;
            double f = getMiddleRight(x, y) * horzSize;

            double g = getBottomLeft(x, y) * horzSize;
            double h = getBottomMiddle(x, y) * horzSize;
            double i = getBottomRight(x, y) * horzSize;

            double fx = (i - g + 2*(f - d) + c - a);
            double fy = (a - g + 2 * (b - h) + c - i);

            fx = Math.Abs(fx);
            fy = Math.Abs(fy);

            double slope = Math.Atan( Math.Pow( Math.Pow(fx, 2) + Math.Pow(fy, 2), 0.5));

            return slope;
        }

        #region slope Heights

        private double getTopLeft(int x, int y)
        {
            if ((x > 0) && (y > 0))
            {
                return heightMap[x - 1, y - 1];
            }
            else
            {
                return heightMap[x, y];
            }
        }

        private double getTopMiddle(int x, int y)
        {
            if (y > 0)
            {
                return heightMap[x, y - 1];
            }
            else
            {
                return heightMap[x, y];
            }
        }

        private double getTopRight(int x, int y)
        {
            if ((x < width - 1) && (y > 0))
            {
                return heightMap[x + 1, y - 1];
            }
            else
            {
                return heightMap[x, y];
            }
        }

        private double getMiddleLeft(int x, int y)
        {
            if (x > 0)
            {
                return heightMap[x - 1, y];
            }
            else
            {
                return heightMap[x, y];
            }
        }

        private double getMiddleRight(int x, int y)
        {
            if (x < width - 1)
            {
                return heightMap[x + 1, y];
            }
            else
            {
                return heightMap[x, y];
            }
        }

        private double getBottomLeft(int x, int y)
        {
            if ((x > 0) && (y < height - 1))
            {
                return heightMap[x - 1, y + 1];
            }
            else
            {
                return heightMap[x, y];
            }
        }

        private double getBottomMiddle(int x, int y)
        {
            if (y < height - 1)
            {
                return heightMap[x, y + 1];
            }
            else
            {
                return heightMap[x, y];
            }
        }

        private double getBottomRight(int x, int y)
        {

            if ((x < width - 1) && (y < height - 1))
            {
                return heightMap[x + 1, y + 1];
            }
            else
            {
                return heightMap[x, y];
            }
        }

        #endregion

    }
}

