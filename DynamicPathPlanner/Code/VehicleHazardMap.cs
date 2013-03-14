using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicPathPlanner.Code
{
    class VehicleHazardMap
    {

        private int[,] knownMap;


        public VehicleHazardMap(int width, int height)
        {
            knownMap = new int[width, height];
        }


        public int getWidth()
        {
            return knownMap.GetLength(0);
        }

        public int getHeight()
        {
            return knownMap.GetLength(1);
        }

        public void setNode(int x, int y , int value)
        {
            if (value >= knownMap[x, y])
            {
                knownMap[x, y] = value;
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
