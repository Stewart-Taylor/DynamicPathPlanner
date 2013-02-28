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

            double a = getTopLeft(x, y);
            double b = getTopMiddle(x, y);
            double c = getTopRight(x, y);

            double d = getMiddleLeft(x, y);
            double f = getMiddleRight(x, y);

            double g = getBottomLeft(x, y);
            double h = getBottomMiddle(x, y);
            double i = getBottomRight(x, y);


            double x_cell_size = 0.1f;
            double y_cell_size = 0.1f;

            double deltaX = ((c + (2 * f) + i) - (a + (2 * d) + g)) / (8 * x_cell_size);
            double deltaY = ((g + (2 * h) + i) - (a + (2 * b) + c)) / (8 * y_cell_size);

            double value = deltaY / deltaX;

            return value;
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

