/*      ElevationLoader Class
 *	    AUTHOR: STEWART TAYLOR
 *------------------------------------
 * This class is used to load in digital elevation models (DEM)
 * It will first attempt to load a DEM from the file cache
 * If no copy exists in the cache it will fetch the elevation model from PANGU
 * Fetching the DEM from PANGU can be slow, hence the cache system.
 *
 * Last Updated: 09/03/2013
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DynamicPathPlanner.Code
{
    class ElevationLoader
    {
        private String cacheFolder = "DEM/";
        private char separator = '_';
        private String extension = ".dem";

        public ElevationLoader()
        {

        }

        public double[,] load_DEM(String environment, float distance ,int size)
        {
            double[,] model;

            if (cacheExists(environment, distance, size))
            {
                model = loadCache(environment, distance, size);
            }
            else
            {
                model = getPANGU_DEM(environment, distance, size);
            }

            return model;
        }

        private bool cacheExists(String environment, float distance, int size)
        {
            String filepath = cacheFolder + environment + separator + distance + separator + size + extension;

            if (File.Exists(filepath))
            {
                return true;
            }

            return false;
        }

        private double[,] loadCache(String environment, float distance, int size)
        {
            String filepath = cacheFolder + environment + separator + distance + separator + size + extension;
            double[,] d = new double[size,size];

            try
            {
                System.IO.StreamReader file = new System.IO.StreamReader(filepath);
                String line;
                int x = 0;
                int y= 0;

                while ((line = file.ReadLine()) != null)
                {
                    x = 0;
                    string[] values = line.Split(',');

                    for (int i = 0; i < size; i++)
                    {
                        String temp = values[i];
                        double tempValue = double.Parse(temp);
                        d[x, y] = tempValue;
                        x++;
                    }
                    y++;
                }

                return d;
            }
            catch 
            {
              // Error With cache
            }

            return null;
        }

        public double[,] getPANGU_DEM(String environment, float distance, int size)
        {
            double[,] d = PANGU_Manager.getElevationModel(distance, size, size);
            createCache(environment, distance, size, d);

            return d;
        }

        private void createCache(String environment, float distance, int size, double[,] model)
        {
            String filepath = cacheFolder + environment + separator + distance + separator + size + extension;

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filepath))
            {
                for (int y = 0; y < model.GetLength(1); y++)
                {
                    for (int x = 0; x < model.GetLength(0); x++)
                    {
                        file.Write(model[x, y].ToString());
                        file.Write(",");
                    }
                    file.WriteLine();
                }
            }
        }

        // TODO
        public double[,] getPPM_DEM()
        {
            return null;
        }

    }
}
