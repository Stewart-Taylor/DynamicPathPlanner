/*      VehicleSensorManager Class
 *	    AUTHOR: STEWART TAYLOR
 *------------------------------------
 * This class manages the system sensors
 * It allows the vehicle to simulate how it would obtain data from its environment
 *
 * Last Updated: 16/03/2013
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicPathPlanner.Code
{
    class VehicleSensorManager
    {
        private NavigationMapManager navigationMap;
        private Vehicle vehicle;
        private int[,] realMap;

        private int width;
        private int height;

        #region GET

        public int getWidth()
        {
            return width;
        }

        public int getHeight()
        {
            return height;
        }


        public double getSlopeValue(int x, int y)
        {
            double value = navigationMap.getHazardSlope()[x, y];
            return value;
        }

        #endregion

        public VehicleSensorManager(Vehicle v, NavigationMapManager m)
        {
            vehicle = v;
            navigationMap = m;
            realMap = navigationMap.getHazardModel();
            width = navigationMap.getHazardModel().GetLength(0);
            height = navigationMap.getHazardModel().GetLength(1);
        }

        public void updateFrontView(int positionX, int positionY, int previousX, int previousY)
        {
            if ((previousX != 0) && (previousY != 0))
            {
                int directionX = positionX - previousX;
                int directionY = positionY - previousY;

                if ((directionX == -1) && (directionY == -1)) { updateFacingTopLeft(positionX, positionY); }
                else if ((directionX == 0) && (directionY == -1)) { updateFacingTopMiddle(positionX, positionY); }
                else if ((directionX == 1) && (directionY == -1)) { updateFacingTopRight(positionX, positionY); }
                else if ((directionX == -1) && (directionY == 0)) { updateFacingMiddleLeft(positionX, positionY); }
                else if ((directionX == 1) && (directionY == 0)) { updateFacingMiddleRight(positionX, positionY); }
                else if ((directionX == -1) && (directionY == 1)) { updateFacingBottomLeft(positionX, positionY); }
                else if ((directionX == 0) && (directionY == 1)) { updateFacingBottomMiddle(positionX, positionY); }
                else if ((directionX == 1) && (directionY == 1)) { updateFacingBottomRight(positionX, positionY); }
            }

         //  areaScan(positionX, positionY, 5);
        }

        private void areaScan(int positionX, int positionY, int size)
        {
            for (int x = -size; x < size; x++)
            {
                for (int y = -size; y < size; y++)
                {
                    updateNode(positionX + x, positionY + y);
                }
            }
        }

        private void updateNode(int x, int y)
        {
            if ((x >= 0) && (y >= 0))
            {
                if ((x < width) && (y < height))
                {
                    int value = realMap[x, y];
                    vehicle.setNode(x, y, value);
                }
            }
        }

        public void updateOwnLocation(int x, int y)
        {
            updateNode(x, y);
        }

        public bool isAreaSafe(PathNode node)
        {
            if (realMap[node.x,node.y] <= 5.0)
            {
                return true;
            }

            return false;
        }

        private void updateFacingTopLeft(int positionX , int positionY)
        {
            updateNode(positionX + 1, positionY -1);
            updateNode(positionX , positionY -1);
            updateNode(positionX -1, positionY - 1);
            updateNode(positionX -1, positionY );
            updateNode(positionX - 1, positionY + 1);
        }

        private void updateFacingTopMiddle(int positionX, int positionY)
        {
            updateNode(positionX - 1, positionY );
            updateNode(positionX +1, positionY );
            updateNode(positionX + 1, positionY - 1);
            updateNode(positionX , positionY -1);
            updateNode(positionX - 1, positionY - 1);
        }

        private void updateFacingTopRight(int positionX, int positionY)
        {
            updateNode(positionX -1, positionY - 1);
            updateNode(positionX, positionY - 1);
            updateNode(positionX + 1, positionY - 1);
            updateNode(positionX + 1, positionY);
            updateNode(positionX + 1, positionY + 1);
        }

        private void updateFacingMiddleLeft(int positionX, int positionY)
        {
            updateNode(positionX, positionY + 1);
            updateNode(positionX, positionY + 1);
            updateNode(positionX - 1, positionY - 1);
            updateNode(positionX - 1, positionY);
            updateNode(positionX - 1, positionY + 1);
        }

        private void updateFacingMiddleRight(int positionX, int positionY)
        {
            updateNode(positionX , positionY + 1);
            updateNode(positionX, positionY +1);
            updateNode(positionX + 1, positionY - 1);
            updateNode(positionX + 1, positionY);
            updateNode(positionX + 1, positionY +1);
        }

        private void updateFacingBottomLeft(int positionX, int positionY)
        {
            updateNode(positionX - 1, positionY + 1);
            updateNode(positionX , positionY + 1);
            updateNode(positionX + 1, positionY + 1);
            updateNode(positionX + 1, positionY );
            updateNode(positionX - 1, positionY );
        }

        private void updateFacingBottomMiddle(int positionX, int positionY)
        {
            updateNode(positionX +1, positionY );
            updateNode(positionX - 1, positionY);
            updateNode(positionX - 1, positionY +1);
            updateNode(positionX , positionY +1);
            updateNode(positionX +1, positionY +1);
        }

        private void updateFacingBottomRight(int positionX, int positionY)
        {
            updateNode(positionX, positionY + 1);
            updateNode(positionX - 1, positionY + 1);
            updateNode(positionX + 1, positionY + 1);
            updateNode(positionX + 1, positionY);
            updateNode(positionX + 1, positionY - 1);
        }


    }
}
