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

        public VehicleSensorManager(Vehicle v, NavigationMapManager m)
        {
            vehicle = v;
            navigationMap = m;

        }

        public int getWidth()
        {
            return navigationMap.getAreaSize();
        }

        public int getHeight()
        {
            return navigationMap.getAreaSize();
        }


        public double getTileValue(int x , int y)
        {
            double value = navigationMap.getHazardModel()[x,y];
            return value;
        }
    }
}
