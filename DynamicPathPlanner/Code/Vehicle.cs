/*      Vehicle Class
 *	    AUTHOR: STEWART TAYLOR
 *------------------------------------
 * This class contains the vehicle logic for the simulation
 * It will access the sensorManager to get enviorment data (Simulates Sensors)
 * It will use it's own internal hazard map for pathfinding only!
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
using System.Collections;

namespace DynamicPathPlanner.Code
{
    class Vehicle
    {
        private VehicleSensorManager sensorManager;
        private VehicleHazardMap map;

        private List<PathNode> takenPath = new List<PathNode>();
        private List<PathNode> givenPath = new List<PathNode>();

        private int positionX;
        private int positionY;
        private int previousX;
        private int previousY;

        private int startX;
        private int startY;
        private int targetX;
        private int targetY;

        private int steps = 0;
        private int stepLimit = 2600;
        private bool atTarget = false;

        private Bitmap pathBitmap;
        private Bitmap camBitmap;

        private int width;
        private int height;

        private int areaSize;
        private float distanceStep;

        private int cameraPitch = -10;
        private int cameraHeight = 17;

        private bool roverCamEnabled = false;


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

        public List<PathNode> getPathPoints()
        {
            return takenPath;
        }

        public bool reachedTarget()
        {
            return atTarget;
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

        #endregion

        #region SET

        public void setRoverCam(bool isEnabled)
        {
            roverCamEnabled = isEnabled;
        }

        public void setStepLimit(int limit)
        {
            stepLimit = limit;
        }

        public void setNode(int x, int y, int value)
        {
            map.setNode(x, y, value);
        }


        #endregion

        public Vehicle(NavigationMapManager mapManager)
        {
            sensorManager = new VehicleSensorManager(this, mapManager);
            areaSize = mapManager.getAreaSize();
            distanceStep = mapManager.getDistanceStep();
            width = mapManager.getHazardModel().GetLength(0);
            height = mapManager.getHazardModel().GetLength(1);
            map = new VehicleHazardMap(width, height);
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

            map = new VehicleHazardMap(width, height);
            map.setHazardMap(startX, startY);
            atTarget = false;

            pathBitmap = new Bitmap(map.getWidth(), map.getHeight());

        /*    BitmapHelper b = new BitmapHelper(pathBitmap);
            b.LockBitmap();
            for (int x = 0; x < map.getWidth(); x++)
            {
                for (int y = 0; y < map.getHeight(); y++)
                {

                            System.Drawing.Color tempColor = getVehicleColorValue(map.getValue(x, y), x, y);
                            b.SetPixel(x, y, tempColor);
                }
            }
            b.UnlockBitmap();
            pathBitmap = b.Bitmap;
         */ 
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

            atTarget = false;

            SearchAlgorithm search;

            do
            {
                search = new A_Star(map.getMap(), positionX, positionY, targetX, targetY);
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

                    if (sensorManager.isAreaSafe(nextNode) == true)
                    {
                        previousX = positionX;
                        previousY = positionY;
                        positionX = nextNode.x;
                        positionY = nextNode.y;

                        takenPath.Add(nextNode);

                        sensorManager.updateOwnLocation(positionX, positionY);
                    }
                    else
                    {
                        sensorManager.updateOwnLocation(nextNode.x, nextNode.y);
                        break;
                    }

                } while (true);


            } while (atTarget == false);
        }


        public void traverseCompare(int startXt, int startYt, int endXt, int endYt)
        {
            startX = startXt;
            startY = startYt;
            targetX = endXt;
            targetY = endYt;

            positionX = startX;
            positionY = startY;

            atTarget = false;

            SearchAlgorithm search;


            search = new A_Star(map.getMap(), positionX, positionY, targetX, targetY);
            givenPath = search.getPath();

            takenPath = givenPath;
            atTarget = true;

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

             atTarget = false;

            D_Star search = new D_Star(map.getMap(), positionX, positionY, targetX, targetY);

            do
            {
                search.updateStart(positionX, positionY);
                search.replan(map.getMap());

                givenPath = search.getPath();

                do
                {
                    sensorManager.updateFrontView(positionX, positionY, previousX, previousY);
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

                    if (sensorManager.isAreaSafe(nextNode) == true)
                    {
                    //    knownMap[nextNode.x, nextNode.y] += 1;
                        map.setNode(nextNode.x, nextNode.y, 40);

                        previousX = positionX;
                        previousY = positionY;

                        positionX = nextNode.x;
                        positionY = nextNode.y;
                        takenPath.Add(nextNode);

                        sensorManager.updateOwnLocation(positionX, positionY);
                    }
                    else
                    {
                        search.updateVertex(nextNode.x, nextNode.y);
                        sensorManager.updateOwnLocation(nextNode.x, nextNode.y);
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
                sensorManager.updateFrontView(positionX, positionY, previousX, previousY);

                steps++;

                if (givenPath.Count == 0)
                {
                    D_Star search = new D_Star(map.getMap(), positionX, positionY, targetX, targetY);
                    search.updateStart(positionX, positionY);
                    search.replan(map.getMap());
                    givenPath = search.getPath();
                }

                if (steps >= stepLimit)
                {
                    atTarget = true;
                }

                if ((positionX == targetX) && (positionY == targetY))
                {
                    atTarget = true;
                }
                if (givenPath.Count == 0)
                {
                    atTarget = true;
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

                    if (sensorManager.isAreaSafe(nextNode) == true)
                    {
                       // knownMap[nextNode.x, nextNode.y] += 1;
                        map.setNode(nextNode.x, nextNode.y, 40);

                        previousX = positionX;
                        previousY = positionY;

                        positionX = nextNode.x;
                        positionY = nextNode.y;
                        takenPath.Add(nextNode);

                        sensorManager.updateOwnLocation(positionX, positionY);
                    }
                    else
                    {
                        givenPath.Clear();
                        map.setNode(nextNode.x, nextNode.y, 99999);
                        sensorManager.updateFrontView(positionX, positionY, previousX, previousY);
                        sensorManager.updateOwnLocation(nextNode.x, nextNode.y);
                    }
                }
            }

            generateRoverImage();
            generatePathImage();
        }


        public void generatePathImage()
        {

             pathBitmap = new Bitmap(map.getAdjustWidth(), map.getAdjustHeight());

                BitmapHelper b = new BitmapHelper(pathBitmap);
                b.LockBitmap();

                for (int x = map.getMinX(); x < map.getMaxX(); x++)
                {
                    for (int y = map.getMinY(); y < map.getMaxY(); y++)
                    {
                        float t = (float)(x - map.getMinX()) / map.getAdjustWidth();
                        int a = (int)Math.Round((double)t * (double)map.getAdjustWidth());

                        t = (float)(y - map.getMinY()) / map.getAdjustHeight();
                        int ba = (int)Math.Round((double)t * (double)map.getAdjustHeight());

                        System.Drawing.Color tempColor = getVehicleColorValue(map.getValue(x, y), x, y);
                        b.SetPixel(a, ba, tempColor);
                    }
                }

                b.UnlockBitmap();

                pathBitmap = b.Bitmap;
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
            }
            else
            {
                gradient = sensorManager.getSlopeValue(x, y);
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
                green = 0; red = 0; blue = 0;

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
            if (roverCamEnabled == true)
            {
                int x = (int)(((float)positionX - ((float)areaSize / 2f)) * distanceStep);
                int y = (int)((float)positionY - (((float)areaSize / 2f)) * distanceStep);
                int z = cameraHeight;
                float yaw = 0;
                int pitch = cameraPitch;
                int roll = 0;

                float deltaX = (float)positionX - (float)previousX;
                float deltaY = (float)positionY - (float)previousY;

                yaw = (float)Math.Atan(deltaY / deltaX) * 180f / (float)Math.PI;
                yaw -= 45;

                camBitmap = PANGU_Manager.getImageView(x, y, z, yaw, pitch, roll, 70.0f);
            }
        }

        //Used to give rover full knowledge of enviroment. Used for compare tests
        public void fullEnvironment()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    sensorManager.updateOwnLocation(x, y);
                }
            }
        }

    }
}
