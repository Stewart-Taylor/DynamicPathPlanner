using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicPathPlanner.Code
{
    class SlopeMax : SlopeAlgrotithm
    {
        public const String ALGORITHM = "MAX";

        public SlopeMax(double[,] model)
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


            double high = 0f;

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


    }
}
