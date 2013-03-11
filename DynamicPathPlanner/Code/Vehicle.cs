/*      Vehicle Class
 *	    AUTHOR: STEWART TAYLOR
 *------------------------------------
 * This class contains the vehicle logic for the simulation
 *
 * Last Updated: 09/03/2013
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
    class Vehicle
    {
        private VehicleSensorManager sensorManager;

        private List<PathNode> takenPath = new List<PathNode>();
        private List<PathNode> givenPath = new List<PathNode>();

        private double[,] knownMap;
        private double[,] realMap;
        private double[,] realImageMap;

        private int positionX;
        private int positionY;
        private int previousX;
        private int previousY;

        private int startX;
        private int startY;
        private int targetX;
        private int targetY;

        private int steps = 0;
        private int stepLimit = 1600;
        private bool atTarget = false;

        private Bitmap pathBitmap;
        private Bitmap camBitmap;


        #region GET

        public int getSteps()
        {
            return steps;
        }

        public int getX()
        {
            return positionY;
        }

        public int getY()
        {
            return positionY;
        }

        #endregion


        public Vehicle(double[,] realMapT, double[,] imageMap , int width , int height)
        {
            realMap = realMapT;
            realImageMap = imageMap;
            knownMap = new double[realMap.GetLength(0), realMap.GetLength(1)]; // find another way to get map dimensions dynamically
        }

        public void initializeSensorManager(NavigationMapManager mapManager)
        {
            sensorManager = new VehicleSensorManager(this, mapManager);
        }

        public void startTraverse(int startXt, int startYt, int endXt, int endYt)
        {
            steps = 0;

            startX = startXt;
            startY = startYt;
            targetX = endXt;
            targetY = endYt;

            positionX = startX;
            positionY = startY;

            knownMap = new double[realMap.GetLength(0), realMap.GetLength(1)];
            atTarget = false;

            pathBitmap = new Bitmap(knownMap.GetLength(0), knownMap.GetLength(1));
        }


        public void traverseMap(int startXt, int startYt, int endXt, int endYt)
        {
            steps = 0;

            startX = startXt;
            startY = startYt;
            targetX = endXt;
            targetY = endYt;

            positionX = startX;
            positionY = startY;

            bool atTarget = false;

            SearchAlgorithm search;

            do
            {
                search = new A_Star(knownMap, positionX, positionY, targetX, targetY);
                givenPath = search.getPath();

                do
                {
                    if (steps >= stepLimit)
                    {
                        atTarget = true;
                        break;
                    }

                    steps++;

                    if ((positionX == targetX) && (positionY == targetY))
                    {
                        atTarget = true;
                        break;
                    }
                    if (givenPath.Count == 0)
                    {
                        atTarget = true;
                        break;
                    }

                    PathNode nextNode = givenPath.Last();
                    givenPath.Remove(nextNode);

                    if (isNextNodeSafe(nextNode) == true)
                    {
                        previousX = positionX;
                        previousY = positionY;
                        positionX = nextNode.x;
                        positionY = nextNode.y;

                        takenPath.Add(nextNode);

                        updateOwnMap(positionX, positionY);
                    }
                    else
                    {
                        updateOwnMap(nextNode.x, nextNode.y);
                        break;
                    }

                } while (true);


            } while (atTarget == false);
        }

        public void traverseMapDstar(int startXt, int startYt, int endXt, int endYt)
        {
            steps = 0;

            startX = startXt;
            startY = startYt;
            targetX = endXt;
            targetY = endYt;

            positionX = startX;
            positionY = startY;

            bool atTarget = false;

            D_Star search = new D_Star(knownMap, positionX, positionY, targetX, targetY);

            do
            {
                search.updateStart(positionX, positionY);
                search.replan(knownMap);

                givenPath = search.getPath();

                do
                {
                    updateFrontView();
                    if (steps >= stepLimit)
                    {
                        atTarget = true;
                        break;
                    }
                    steps++;

                    if ((positionX == targetX) && (positionY == targetY))
                    {
                        atTarget = true;
                        break;
                    }
                    if (givenPath.Count == 0)
                    {
                        atTarget = true;
                        break;
                    }
                    PathNode nextNode = givenPath.Last();
                    givenPath.Remove(nextNode);

                    if (isNextNodeSafe(nextNode) == true)
                    {
                        previousX = positionX;
                        previousY = positionY;

                        positionX = nextNode.x;
                        positionY = nextNode.y;
                        takenPath.Add(nextNode);

                        updateOwnMap(positionX, positionY);
                    }
                    else
                    {
                        search.updateVertex(nextNode.x, nextNode.y);
                        updateOwnMap(nextNode.x, nextNode.y);
                        break;
                    }

                } while (true);
            } while (atTarget == false);

            generatePathImage();
        }


        public void traverseMapDStep()
        {
            if (atTarget == false)
            {
                if (givenPath.Count == 0)
                {
                    D_Star search = new D_Star(knownMap, positionX, positionY, targetX, targetY);

                    search.updateStart(positionX, positionY);
                    search.replan(knownMap);

                    givenPath = search.getPath();
                }


                updateFrontView();
                if (steps >= stepLimit)
                {
                    atTarget = true;
                    //   break;
                }
                steps++;

                if ((positionX == targetX) && (positionY == targetY))
                {
                    atTarget = true;
                    //  break;
                }
                if (givenPath.Count == 0)
                {
                    atTarget = true;
                    //   break;
                }
                if (atTarget == false)
                {
                    PathNode nextNode = givenPath.Last();
                    givenPath.Remove(nextNode);

                    if ((nextNode.x == positionX) && (nextNode.y == positionY))
                    {

                        if (givenPath.Count > 0)
                        {
                            nextNode = givenPath.Last();
                        }
                        if ((nextNode.x == previousX) && (nextNode.y == previousY))
                        {
                            if (givenPath.Count > 0)
                            {
                                nextNode = givenPath.Last();
                            }
                        }
                    }

                    if (isNextNodeSafe(nextNode) == true)
                    {
                        generateRoverImage();
                        previousX = positionX;
                        previousY = positionY;

                        positionX = nextNode.x;
                        positionY = nextNode.y;
                        takenPath.Add(nextNode);

                        updateOwnMap(positionX, positionY);
                    }
                    else
                    {
                        givenPath.Clear();
                        //  search.updateVertex(nextNode.x, nextNode.y);
                        knownMap[nextNode.x, nextNode.y] = 99999;
                        updateFrontView();
                        updateOwnMap(nextNode.x, nextNode.y);
                        // break;
                    }
                }
            }
           
            generatePathImage();
        }


        private bool isNextNodeSafe(PathNode node)
        {
            if (realMap[node.x, node.y] <= 5.0)
            {
                return true;
            }

            if (realMap[node.x, node.y] == knownMap[node.x, node.y])
            {
              //  return true; // means no safer path
            }
            return false;
        }


        private void updateOwnMap(int x, int y)
        {
            if ((x > 0) && (y > 0))
            {
                if ((x < knownMap.GetLength(0)) && (y < knownMap.GetLength(1)))
                {
                    knownMap[x, y] = realMap[x, y];

                    System.Drawing.Color tempColor = getVehicleColorValue(realImageMap[x, y], x, y);

                    pathBitmap.SetPixel(x, y, tempColor);
                }
            }
        }

        private void updateFrontView()
        {
            if ((previousX != 0) && (previousY != 0))
            {
              //  if (realMap[positionX, positionY] != knownMap[positionX, positionY])
              //  {
                    int directionX = positionX - previousX;
                    int directionY = positionY - previousY;

                    if ((directionX == -1) && (directionY == -1)) { updateFacingTopLeft(); }
                    else if ((directionX == 0) && (directionY == -1)) { updateFacingTopMiddle(); }
                    else if ((directionX == 1) && (directionY == -1)) { updateFacingTopRight(); }
                    else if ((directionX == -1) && (directionY == 0)) { updateFacingMiddleLeft(); }
                    else if ((directionX == 1) && (directionY == 0)) { updateFacingMiddleRight(); }
                    else if ((directionX == -1) && (directionY == 1)) { updateFacingBottomLeft(); }
                    else if ((directionX == 0) && (directionY == 1)) { updateFacingBottomMiddle(); }
                    else if ((directionX == 1) && (directionY == 1)) { updateFacingBottomRight(); }
              //  }
            }

            int size = 3;
            for (int x = -size; x < size; x++)
            {
                for (int y = -size; y < size; y++)
                {
                    updateTile(positionX + x, positionY + y);
                }
            }
        }

        private void updateTile(int x, int y)
        {
           // knownMap[x, y] = realMap[x, y];
            updateOwnMap(x, y);
        }

        private void updateFacingTopLeft()
        {
            updateTile(positionX - 1, positionY - 1);
            updateTile(positionX - 2, positionY);
            updateTile(positionX + 1, positionY + 1);
            updateTile(positionX - 2, positionY - 2);
        }

        private void updateFacingTopMiddle()
        {
            updateTile(positionX - 1, positionY - 1);
            updateTile(positionX, positionY - 1);
            updateTile(positionX + 1, positionY - 1);
            updateTile(positionX, positionY - 2);
        }

        private void updateFacingTopRight()
        {
            updateTile(positionX + 1, positionY - 1);
            updateTile(positionX, positionY - 2);
            updateTile(positionX + 2, positionY);
            updateTile(positionX + 2, positionY - 2);
        }

        private void updateFacingMiddleLeft()
        {
            updateTile(positionX - 1, positionY);
            updateTile(positionX - 1, positionY - 1);
            updateTile(positionX - 1, positionY + 1);
            updateTile(positionX - 2, positionY);
        }

        private void updateFacingMiddleRight()
        {
            updateTile(positionX + 1, positionY);
            updateTile(positionX + 1, positionY - 1);
            updateTile(positionX + 1, positionY + 1);
            updateTile(positionX + 2, positionY);
        }

        private void updateFacingBottomLeft()
        {
            updateTile(positionX - 1, positionY + 1);
            updateTile(positionX - 2, positionY);
            updateTile(positionX, positionY + 2);
            updateTile(positionX - 2, positionY + 2);
        }

        private void updateFacingBottomMiddle()
        {
            updateTile(positionX, positionY + 1);
            updateTile(positionX - 1, positionY + 1);
            updateTile(positionX + 1, positionY + 1);
            updateTile(positionX, positionY + 2);
        }

        private void updateFacingBottomRight()
        {
            updateTile(positionX + 1, positionY + 1);
            updateTile(positionX + 2, positionY);
            updateTile(positionX, positionY + 2);
            updateTile(positionX + 2, positionY + 2);
        }

        private void generateImageQuick()
        {
            Bitmap bitmap = new Bitmap(knownMap.GetLength(0), knownMap.GetLength(1));

            for (int x = 0; x < knownMap.GetLength(0); x++)
            {
                for (int y = 0; y < knownMap.GetLength(1); y++)
                {
                    System.Drawing.Color tempColor = getVehicleColorValue(realImageMap[x, y], x, y);
                    bitmap.SetPixel(x, y, tempColor);
                }
            }
            pathBitmap = bitmap;
        }


        public void generatePathImage()
        {
            Bitmap bitmap = new Bitmap(knownMap.GetLength(0), knownMap.GetLength(1));

            for (int x = 0; x < knownMap.GetLength(0); x++)
            {
                for (int y = 0; y < knownMap.GetLength(1); y++)
                {
                    System.Drawing.Color tempColor = getVehicleColorValue(knownMap[x, y], x, y);

                    bitmap.SetPixel(x, y, tempColor);
                }
            }
            pathBitmap = bitmap;
        }

        private System.Drawing.Color getVehicleColorValue(double gradient, int x, int y)
        {
            System.Drawing.Color color = System.Drawing.Color.White;

            gradient = Math.Abs(gradient);

            float green = 255;
            float red = 255;
            float blue = 0;

            bool notKnown = false;
            if (gradient == 0)
            {
                notKnown = true;
                //   gradient = realImageMap[x, y];
            }
            else
            {
                gradient = realImageMap[x, y];
            }

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
              //  green = ((float)green * 0.1f);
               // red = ((float)red * 0.1f);
                green = 0;
                red = 0;
                blue = 0;

                if (x % 3 == 0)
                {
                    blue = 40;
                }

                if (y % 3 == 0)
                {
                    blue = 40;
                }

            }

            foreach (PathNode n in takenPath)
            {
                if ((n.x == x) && (n.y == y))
                {
                    red = 50;
                    green = 170;
                    blue = 150;
                }
            }

            if ((targetX == x) && (targetY == y))
            {
                green = 255; red = 255; blue = 255;
            }

            if ((positionX == x) && (positionY == y))
            {
                green = 0; red = 0; blue = 255;
            }

            color = System.Drawing.Color.FromArgb(255, (int)red, (int)green, (int)blue);

            return color;
        }


        public ImageSource getRoverCam()
        {
            if (camBitmap != null)
            {
                Bitmap skyBitmap = camBitmap;
                MemoryStream ms = new MemoryStream();
                skyBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                ms.Position = 0;
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = ms;
                bi.EndInit();

                return bi;
            }

            return null;
        }


        private void generateRoverImage()
        {
            int x = (int) ( ((float)positionX  - (1024f/2f))*0.1f);
            int y = (int)((float)positionY - ((1024f / 2f)) * 0.1f);
            int z = 20;
            float yaw = 0;
            int pitch = -10;
            int roll = 0;

         

          //  float d = ((float)positionX - (float)previousX) - ((float)positionY - (float)previousY);
            float dir = -(float)Math.Atan2(((float)positionX - (float)previousX), ((float)positionY - (float)previousX));
             yaw = dir * 180f / (float)Math.PI;
             yaw += 70;
        

            camBitmap = PANGU_Manager.getImageView(x, y, z, yaw, pitch, roll);

        }

        public ImageSource getPathImage()
        {
            MemoryStream ms = new MemoryStream();
            pathBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = ms;
            bitmap.EndInit();

            return bitmap;
        }

        public List<PathNode> getPathPoints()
        {
            return takenPath;
        }

        public bool reachedTarget()
        {
            return atTarget;
        }

    }
}
