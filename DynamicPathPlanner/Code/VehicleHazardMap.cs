/*      VehicleHazardMap Class
 *	    AUTHOR: STEWART TAYLOR
 *------------------------------------
 * This class contains the data the vehicle stores for it's own map
 * It is what the rover uses for pathfinding. This keeps it seperate from the simulated enivorment data
 *
 * Last Updated: 16/03/2013
*/

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


        #region GET

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

        public int[,] getMap()
        {
            return knownMap;
        }

        public int getValue(int x, int y)
        {

            return knownMap[x, y];
        }

        #endregion



        #region SET

        public void setHazardMap(int x, int y)
        {
            minX = x;
            maxX = x;
            minY = y;
            maxY = y;
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

        #endregion

        public VehicleHazardMap(int width, int height)
        {
            knownMap = new int[width, height];
        }



    }
}
