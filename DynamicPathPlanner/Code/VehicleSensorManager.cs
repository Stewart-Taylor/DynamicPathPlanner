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
    }
}
