using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace DynamicPathPlanner.Code
{
    class VehicleHazardMap
    {

        private int[,] knownMap;

        private int minX;
        private int maxX;
        private int minY;
        private int maxY;

        public VehicleHazardMap(int width, int height)
        {
            knownMap = new int[width, height];
        }

        public void setHazardMap(int x, int y)
        {
            minX = x;
            maxX = x;
            minY = y;
            maxY = y;
        }

        public int getMinX()
        {
            return minX;
        }

        public int getMaxX()
        {
            return maxX;
        }

        public int getMinY()
        {
            return minY;
        }

        public int getMaxY()
        {
            return maxY;
        }


        public void setMinX(int v)
        {
            minX = v;
        }

        public void setMaxX(int v)
        {
            maxX = v;
        }

        public void setMinY(int v)
        {
            minY = v;
        }

        public void setMaxY(int v)
        {
            maxY = v;
        }

        public int getAdjustWidth()
        {
            return maxX - minX;
        }

        public int getAdjustHeight()
        {
            return maxY - minY;
        }

        public int getWidth()
        {
            return knownMap.GetLength(0);
        }

        public int getHeight()
        {
            return knownMap.GetLength(1);
        }

        public void setNode(int x, int y, int value)
        {
            if (value >= knownMap[x, y])
            {
                knownMap[x, y] = value;

                if (x < minX)
                {
                    minX = x;
                }

                if (x > maxX)
                {
                    maxX = x;
                }

                if (y < minY)
                {
                    minY = y;
                }

                if (y > maxY)
                {
                    maxY = y;
                }

            }
        }

        public int[,] getMap()
        {
            return knownMap;
        }

        public int getValue(int x, int y)
        {

            return knownMap[x, y];
        }

    }
}
