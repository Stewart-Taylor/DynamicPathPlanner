﻿/*      PANGU_Manager Class
 *	    AUTHOR: STEWART TAYLOR
 *------------------------------------
 * This class is used as an easy way to simply fetch data from a PANGU server
 * The PANGU Connector contains the wrapper functionality 
 *
 * Last Updated: 28/02/2013
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Media;
using System.IO;
using System.Windows.Media.Imaging;

namespace DynamicPathPlanner.Code
{
    static class PANGU_Manager
    {

        private static PANGU_Connector connector = new PANGU_Connector();
        private static InterfaceManager interfaceManager;

        



        public static bool connect(String hostname, int port)
        {
            if (connector.connect(hostname, port) == true)
            {
                //interfaceManager.addLogEntry("Connected");
                return true;
            }

            return false;
        }


        public static double[,] getElevationModel(float distance, int width, int height)
        {
            double[,] elevationModel;

            elevationModel = connector.getDEM(distance, width, height);

            return elevationModel;
        }

        public static ImageSource getSkyView(float distance , int size)
        {

       //     Bitmap skyBitmap = connector.getImage(x, y, z, yaw, pitch, roll);
             Bitmap skyBitmap = getSkyBitmap( distance ,  size);
            MemoryStream ms = new MemoryStream();
            skyBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();

            ImageSource img = bi;

            return img;
        }


        public static Bitmap getSkyBitmap(float distance, int size)
        {
            float x = 0;
            float y = 0;
            float z = 0;
            float yaw = 90;
            float pitch = -90;
            float roll = 0;


            float constantValue = 1.96f;


            z = distance * size * constantValue;

            Bitmap skyBitmap = connector.getImage(x, y, z, yaw, pitch, roll);
            return skyBitmap;
        }


        public static void endConnection()
        {
            if (connector.getConnectionStatus() == true)
            {
                connector.disconnect();
            }
        }

    }
}
