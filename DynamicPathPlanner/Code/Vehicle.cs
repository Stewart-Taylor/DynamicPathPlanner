﻿/*      Vehicle Class
 *	    AUTHOR: STEWART TAYLOR
 *------------------------------------
 * This class contains the vehicle logic for the simulation
 * It will access the sensorManager to get enviorment data (Simulates Sensors)
 * It will use it's own internal hazard map for pathfinding data only!
 *
 * Last Updated: 21/04/2013
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

        private float realPositionX;
        private float realPositionY;
        private float roverAngle;

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
        private int hazardSectorSize;

        private int cameraPitch = -10;
        private int cameraYaw = 0;
        private int cameraRoll = 0;
        private int cameraHeight = 17;

        private bool roverCamEnabled = true;

        private bool atNextPoint = false;

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

        public List<int[]> getPath()
        {
            List<int[]> pList = new List<int[]>();

            foreach (PathNode p in takenPath)
            {
                int[] t = new int[2];
                t[0] = p.x;
                t[1] = p.y;
                pList.Add(t);
            }

            return pList;
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

        public Bitmap getPathBitmap()
        {
            return pathBitmap;
        }

        public int[,] getInternalMap()
        {
            return map.getMap();
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
            hazardSectorSize = mapManager.getHazardSectorSize();
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

            //Add In start Node
            PathNode startNode = new PathNode();
            startNode.x = startX;
            startNode.y = startY;
            takenPath.Add(startNode);

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
                        sensorManager.updateOwnLocation(nextNode.x, nextNode.y);
                        break;
                    }
                } while (true);
            } while (atTarget == false);

            sensorManager.updateFrontView(positionX, positionY, previousX, previousY);
            generatePathImage();
            generateRoverImage();
        }

        public void traverseMapDStep()
        {
            if (atTarget == false)
            {
                tScan();

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
                    else
                    {
                        takenPath.Add(nextNode); // Add Start To taken Path
                    }

                    if (sensorManager.isAreaSafe(nextNode) == true)
                    {
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
                        sensorManager.updateOwnLocation(nextNode.x, nextNode.y);
                    }
                }
            }

            sensorManager.updateFrontView(positionX, positionY, previousX, previousY);
            generateRoverImage();
            generatePathImage();
        }


        private void tScan()
        {
            foreach (PathNode p in givenPath)
            {
                if (sensorManager.isAreaSafe(p) == false )
                {
                    givenPath.Clear();
                    break;
                }
            }
        }

        public void generatePathImage()
        {
            if ((map.getAdjustWidth() > 0) && (map.getAdjustHeight() > 0))
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

        //Used while rover is moving
        private void generateRoverImage()
        {
            if (roverCamEnabled == true)
            {
                int x  = getCameraX();
                int y = getCameraY();
                int z = getCameraZ(x, y);
                float deltaX = (float)positionX - (float)previousX;
                float deltaY = (float)positionY - (float)previousY;
                float yaw = -(float)Math.Atan(deltaY / deltaX) * 180f / (float)Math.PI;

                camBitmap = PANGU_Manager.getImageView(y, x, z, yaw, cameraPitch, cameraRoll, 70.0f);
            }
        }

        private int getCameraX()
        {
            //   positionX = 32;
            //   int x = (int)((float)positionX - (((areaSize/hazardSectorSize / 2f))) );
            //  int x = (int)( (float)(positionX -16f) * 2f );
            //    int  x = (int)(((float)positionX - ((float)areaSize )) * distanceStep);
            //  x = 32;

            int x = positionX + (int)((float)map.getWidth() / 2f);
            x = (int)((float)x - (((areaSize / 2f)) * distanceStep));
            x = (int)((float)x * 2f);

            return x;
        }

        private int getCameraY()
        {
            // positionY = 32;
            // int y = (int)((float)positionY - (((areaSize/hazardSectorSize / 2f)) ));
            // int y = (int)((float)(positionY - 16f) * 2f);
            // int y = (int)((float)positionY - (((float)areaSize )) * distanceStep);
            // y = 32;

            int y = positionY + (int)((float)map.getHeight() / 2f);
            y = (int)((float)y - (((areaSize / 2f)) * distanceStep));
            y = (int)((float)y * 2f);

            return y;
        }

        private int getCameraZ(int x , int y)
        {
            float z = 1;
            z = (int)PANGU_Manager.getPointHeight(x, y);
            z += 15;
            return (int)z;
        }

        //Manual camera control
        public void updateRoverImage(float pitch , float yaw)
        {
            if (roverCamEnabled == true)
            {
                int x = getCameraX();
                int y = getCameraY();
                int z = getCameraZ(x, y);
    
                camBitmap = PANGU_Manager.getImageView(y, x, z, -yaw, pitch, cameraRoll, 70.0f);
            }
        }

        //Used to give rover full knowledge of enviroment. Used for compare tests
        public void fullEnvironment()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    double value = sensorManager.getSlopeValue(x, y);
                    int v = 1;
                    if( value > 1f)
                    {
                        v = 99999999;
                    }
                    setNode(x, y, v);
                }
            }
        }

    }
}
