/*      PANGU_Manager Class
 *	    AUTHOR: STEWART TAYLOR
 *------------------------------------
 * This class is used as an easy way to simply fetch data from a PANGU server
 * The PANGU Connector contains the wrapper functionality 
 *
 * Last Updated: 16/03/2013
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
        private static bool connectionExists = false;
        private static String panguProcessName = "viewer";

        private static PANGU_Connector connector = new PANGU_Connector();

        public static bool connect(String hostname, int port)
        {
            if (connectionExists == true)
            {
                return true;
            }

            if (connector.connect(hostname, port) == true)
            {
                connectionExists = true;
                //interfaceManager.addLogEntry("Connected");
                return true;
            }
            return false;
        }


        //Best method name ever!
        public static void killPANGU()
        {
            foreach (System.Diagnostics.Process myProc in System.Diagnostics.Process.GetProcesses())
            {
                if (myProc.ProcessName == panguProcessName)
                {
                    myProc.Kill();
                    connectionExists = false;
                }
            }
        }

        public static double[,] getElevationModel(float distance, int width, int height)
        {
            double[,] elevationModel = connector.getDEM(distance, width, height);

            return elevationModel;
        }

        public static ImageSource getSkyView(float distance, int size)
        {
            Bitmap skyBitmap = getSkyBitmap(distance, size);
            MemoryStream ms = new MemoryStream();
            skyBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();

            return bi;
        }


        public static Bitmap getImageView(int x, int y , int z , float yaw ,float pitch ,float roll  )
        {
            Bitmap bitmap = connector.getImage(x, y, z, yaw, pitch, roll , 30.0f);

            return bitmap;
        }

        public static Bitmap getImageView(int x, int y, int z, float yaw, float pitch, float roll, float fov )
        {
            Bitmap bitmap = connector.getImage(x, y, z, yaw, pitch, roll, fov);

            return bitmap;
        }


        public static float getPointHeight(int x , int y)
        {
            float value = connector.getHeightPoint(x, y);
            return value;
        }


        public static Bitmap getSkyBitmap(float distance, int size)
        {
            float x = 0;
            float y = 0;
            float z = 0;
            float yaw = 90;
            float pitch = -90;
            float roll = 0;
            float fov = 30;
            float height;

            fov = ((float)Math.PI / 180f) * fov;
            height = (float)size / (2f * (float)Math.Tan(fov / 2f));
            z = height * 0.1f; // Convert to unit distance from pixel distance

            Bitmap skyBitmap = connector.getImage(x, y, z, yaw, pitch, roll ,30.0f);

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
