using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicPathPlanner.Code
{
    class SlopeAverage : SlopeAlgrotithm
    {
        public const String ALGORITHM = "AVERAGE";

        public SlopeAverage(double[,] model)
            : base(model)
        {
            heightMap = model;
            width = heightMap.GetLength(0);
            height = heightMap.GetLength(1);

            generateSlopeModel();
        }


        protected override double calculateSlopeValue(int x, int y)
        {

            double topLeft = getTopLeftSlope(x, y);
            double topMid = getTopMiddleSlope(x, y);
            double topRight = getTopRightSlope(x, y);

            double midLeft = getMiddleLeftSlope(x, y);
            double midRight = getMiddleRightSlope(x, y);

            double botLeft = getBottomLeftSlope(x, y);
            double botMid = getBottomMiddleSlope(x, y);
            double botRight = getBottomRightSlope(x, y);

            double total = topLeft + topRight + topMid + midLeft + midRight + botLeft + botMid + botRight;

            double value = total / 8f;

            return value;
        }


    }
}
