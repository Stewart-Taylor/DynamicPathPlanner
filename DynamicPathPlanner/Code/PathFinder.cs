/*      PathFinder Class
 *	    AUTHOR: STEWART TAYLOR
 *------------------------------------
 * This class is used to provide a path to a specified target using the map it is given
 * Can use a search algorithm given to it to find the path
 *
 * Last Updated: 16/03/2013
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.IO;
using System.Windows.Media.Imaging;
using System.Drawing;

namespace DynamicPathPlanner.Code
{
    class Pathfinder
    {
        private int width;
        private int height;


        private Bitmap pathBitmap;
        private int[,] hazardModel;
        private double[,] hazardImageModel;

        private List<PathNode> pathNodes = new List<PathNode>();

        private int startX;
        private int startY;
        private int targetX;
        private int targetY;


        public int getSteps()
        {
            return pathNodes.Count;
        }

        public Pathfinder(int[,] hazardModelT, double[,] hazardImageModelT, int startXT, int startYT, int targetXT, int targetYT)
        {
            hazardModel = hazardModelT;
            hazardImageModel = hazardImageModelT;

            width = hazardModel.GetLength(0);
            height = hazardModel.GetLength(1);

            startX = startXT;
            startY = startYT;
            targetX = targetXT;
            targetY = targetYT;

            pathBitmap = new Bitmap(width, height);
        }


        public List<PathNode> getPath()
        {
            return pathNodes;
        }


        public void findPath(int startX, int startY, int targetX, int targetY)
        {
            SearchAlgorithm aStar = new A_Star(hazardModel, startX, startY, targetX, targetY);

            pathNodes = aStar.getPath();
        }


        public void generatePathImage()
        {
            Bitmap bitmap = new Bitmap(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    System.Drawing.Color tempColor = getPathColorValue(hazardImageModel[x, y], x, y);

                    bitmap.SetPixel(x, y, tempColor);
                }
            }

            pathBitmap = bitmap;
        }


        private System.Drawing.Color getPathColorValue(double gradient, int x, int y)
        {
            System.Drawing.Color color = System.Drawing.Color.White;

            gradient = Math.Abs(gradient);

            float green = 255;
            float red = 255;
            float blue = 0;

            bool notKnown = false;

            if (gradient <= 1f)
            {
                red = (1f - (float)gradient) * 255;
            }
            else if (gradient <= 3f)
            {
                float percent = ((float)gradient) / (3f);

                green = (1f - percent) * 255;
            }
            else
            {
                green = 0;
            }


            if (notKnown == true)
            {
                green = ((float)green * 0.3f);
                red = ((float)red * 0.3f);
            }

            foreach (PathNode n in pathNodes)
            {
                if ((n.x == x) && (n.y == y))
                {
                    red = 0;
                    green = 0;
                    blue = 255;
                }
            }

            if ((targetX == x) && (targetY == y))
            {
                green = 255; red = 255; blue = 255;
            }

            color = System.Drawing.Color.FromArgb(255, (int)red, (int)green, (int)blue);

            return color;
        }

        public ImageSource getPathImage()
        {
            MemoryStream ms = new MemoryStream();
            pathBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();

            ImageSource img = bi;

            return img;
        }


    }
}
