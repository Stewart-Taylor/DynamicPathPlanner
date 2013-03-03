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


        public double[,] load_DEM(String environment, int distance ,int size)
        {
            double[,] d;


            if (cacheExists(environment, distance, size))
            {
                d = loadCache(environment, distance, size);
            }
            else
            {
                d = getPANGU_DEM(environment, distance, size);
            }


            return d;

        }

        private bool cacheExists(String environment, int distance, int size)
        {
            String filepath = cacheFolder + environment + separator + distance + separator + size + extension;

            if (File.Exists(filepath))
            {
                return true;
            }

            return false;
        }

        private double[,] loadCache(String environment, int distance, int size)
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
                    x =0;
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
            catch (Exception e)
            {
             //   Console.WriteLine("The file could not be read:");
             //   Console.WriteLine(e.Message);
            }


            return null;
        }


        public double[,] getPANGU_DEM(String environment, int distance , int size)
        {
            double[,] d = PANGU_Manager.getElevationModel(distance, size, size);

            createCache(environment, distance, size, d);

            return d;
        }


        private void createCache(String environment, int distance, int size , double[,] d)
        {
            String filepath = cacheFolder + environment + separator + distance + separator + size + extension;

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filepath))
            {
                for (int y = 0; y < d.GetLength(1); y++)
                {
                    for (int x = 0; x < d.GetLength(0); x++)
                    {
                        file.Write(d[x, y].ToString());
                        file.Write(",");
                    }
                    file.WriteLine();
                }
            
            }

        }


        public double[,] getPPM_DEM()
        {

            return null;
        }

    }
}
