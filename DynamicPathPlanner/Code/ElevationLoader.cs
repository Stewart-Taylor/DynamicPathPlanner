using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicPathPlanner.Code
{
    class ElevationLoader
    {

        public ElevationLoader()
        {


        }


        public double[,] getPANGU_DEM(int distance , int size)
        {
            double[,] d = PANGU_Manager.getElevationModel(distance, size, size);

            return d;
        }

        public double[,] getPPM_DEM()
        {

            return null;
        }

    }
}
