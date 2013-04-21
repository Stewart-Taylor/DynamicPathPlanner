/*      PANGU_Manager Class
 *	    AUTHOR: STEWART TAYLOR
 *------------------------------------
 * This class is used as an easy way to simply fetch data from a PANGU server
 * The PANGU Connector contains the wrapper functionality 
 *
 * Last Updated: 21/04/2013
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

        #region GET

        public static double[,] getElevationModel(float distance, int width, int height)
        {
            return connector.getDEM(distance, width, height);
        }

        public static Bitmap getImageView(int x, int y, int z, float yaw, float pitch, float roll)
        {
            return connector.getImage(x, y, z, yaw, pitch, roll, 30.0f);
        }

        public static Bitmap getImageView(int x, int y, int z, float yaw, float pitch, float roll, float fov)
        {
            return connector.getImage(x, y, z, yaw, pitch, roll, fov);
        }


        public static float getPointHeight(int x, int y)
        {
            return connector.getHeightPoint(x, y);
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

            return connector.getImage(x, y, z, yaw, pitch, roll, 30.0f);
        }

        #endregion

        public static bool connect(String hostname, int port)
        {
            if (connectionExists == true)
            {
                return true;
            }

            if (connector.connect(hostname, port) == true)
            {
                connectionExists = true;
                return true;
            }
            return false;
        }

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

        public static void endConnection()
        {
            if (connector.getConnectionStatus() == true)
            {
                connector.disconnect();
            }
        }

    }
}
