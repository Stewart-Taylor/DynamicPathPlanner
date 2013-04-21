/*      VehicleHazardMap Class
 *	    AUTHOR: STEWART TAYLOR
 *------------------------------------
 * This class contains the data the vehicle stores for it's own internal map
 * It is what the rover uses for pathfinding. This keeps it seperate from the simulated enivorment data
 * 
 * 
 * Last Updated: 21/04/2013
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
        private int mapPad = 4; // Used to provide access to a region slightly outside what is known

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
            if ((v - mapPad) > 0)
            {
                minX = v - mapPad;
            }
            else
            {
                minX = 0;
            }
        }

        public void setMaxX(int v)
        {
            if ((v + mapPad) < getWidth())
            {
                maxX = v + mapPad;
            }
            else
            {
                maxX = getWidth();
            }
        }

        public void setMinY(int v)
        {
            if ((v - mapPad) > 0)
            {
                minY = v - mapPad;
            }
            else
            {
                minY = 0;
            }
        }

        public void setMaxY(int v)
        {
            if ((v + mapPad) < getHeight())
            {
                maxY = v + mapPad;
            }
            else
            {
                maxY = getHeight();
            }
        }

        public void setNode(int x, int y, int value)
        {
            if (value >= knownMap[x, y])
            {
                knownMap[x, y] = value;

                if (x < minX + mapPad)
                {
                    setMinX(x);
                }

                if (x > maxX - mapPad)
                {
                    setMaxX(x);
                }

                if (y < minY + mapPad)
                {
                    setMinY(y);
                }

                if (y > maxY - mapPad)
                {
                    setMaxY(y);
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
