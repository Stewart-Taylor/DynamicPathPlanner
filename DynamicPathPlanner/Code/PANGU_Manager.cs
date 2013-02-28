/*      PANGU_Manager Class
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
    class PANGU_Manager
    {

        private PANGU_Connector connector = new PANGU_Connector();
        private InterfaceManager interfaceManager;

        public PANGU_Manager(InterfaceManager iManager)
        {
            interfaceManager = iManager;

        }



        public bool connect(String hostname, int port)
        {
            if (connector.connect(hostname, port) == true)
            {
                interfaceManager.addLogEntry("Connected");
                return true;
            }

            return false;
        }


        public double[,] getElevationModel(int distance, int width , int height)
        {
            double[,] elevationModel;

            elevationModel = connector.getDEM(distance, width, height);

            return elevationModel;
        }

        public ImageSource getSkyView()
        {
            Bitmap skyBitmap = connector.getImage(0, 0, 7000, 0, -90, 0);
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


        public void endConnection()
        {
            if (connector.getConnectionStatus() == true)
            {
                connector.disconnect();
            }
        }

    }
}
