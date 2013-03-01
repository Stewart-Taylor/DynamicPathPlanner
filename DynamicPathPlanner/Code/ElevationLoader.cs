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


        public double[,] getPANGU_DEM()
        {
            double[,] d = PANGU_Manager.getElevationModel(1 ,256,256);

            return d;
        }

        public double[,] getPPM_DEM()
        {

            return null;
        }

    }
}
