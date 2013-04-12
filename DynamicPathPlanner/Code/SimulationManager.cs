/*      SimulationManager Class
 *	    AUTHOR: STEWART TAYLOR
 *------------------------------------
 * This class is used to run the pathfinding simulation
 * It handles the results of the simulation
 * and generates visual output
 *
 * Last Updated: 25/03/2013
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace DynamicPathPlanner.Code
{
    class SimulationManager
    {
        private NavigationMapManager mapManager;
        private SlopeModel slopeModel;
        private HazardModel hazardModel;
        private Pathfinder pathfinder;

        private Bitmap skyBitmap;
        private Bitmap elevationBitmap;
        private Bitmap slopeBitmap;
        private Bitmap hazardBitmap;
        private Bitmap skyPathBitmap;
        private Bitmap elevationPathBitmap;
        private Bitmap slopePathBitmap;
        private Bitmap hazardPathBitmap;
        private Bitmap comboPathBitmap;
        private Bitmap aerialCompareBitmap;
        private Bitmap elevationCompareBitmap;
        private Bitmap slopeCompareBitmap;
        private Bitmap hazardCompareBitmap;

        private Random r = new Random();

        private bool stepTraverseStarted = false;

        private Vehicle rover;
        private Vehicle compareRover;
        private Vehicle compareRoverD;
        private int steps;

        private int startX;
        private int startY;
        private int targetX;
        private int targetY;
        private String searchAlgortihm;
        private bool knownMap;
        private int hazardSectorSize;
        private int areaSize;
        private ImageSource imageSource;
        private String timeTaken;
        private bool pathGenerated = false;
        private bool simulationInProgress = false;
        private bool stepSet = false;
        private float roverSize;
        private float roverSlope;

        private float pathLikeness = 0f;

        private float likenessDknownToA = 0f;
        private float likenessDunknownToA = 0f;
        private float likenessDunknownToDknown = 0f;

        private bool compareCompleted;

        private System.Drawing.Color pathColor = System.Drawing.Color.Blue;
        private System.Drawing.Color compareColor = System.Drawing.Color.DarkCyan;
        private System.Drawing.Color compareDColor = System.Drawing.Color.LightCoral;
        private System.Drawing.Color startColor = System.Drawing.Color.MediumAquamarine;
        private System.Drawing.Color targetColor = System.Drawing.Color.MediumAquamarine;

        #region GET

        public String getTimeTaken()
        {
            return timeTaken;
        }

        public int getSteps()
        {
            return steps;
        }

        public Bitmap getElevationPathImage()
        {
            if (pathGenerated == true)
            {
                return getElevationPathBitmap();
            }
            return elevationBitmap;
        }

        public Bitmap getSlopePathImage()
        {
            if (pathGenerated == true)
            {
                return getSlopePathBitmap();
            }
            return slopeBitmap;
        }

        public Bitmap getHazardPathImage()
        {
            if (pathGenerated == true)
            {
                return getHazardPathBitmap();
            }
            return hazardBitmap;
        }

        public Bitmap getComboPathImage()
        {
            comboPathBitmap = null;
            if (pathGenerated == true)
            {
                return getComboPathBitmap();
            }
            return elevationBitmap;
        }

        public Bitmap getAerialPathImage()
        {
            if (pathGenerated == true)
            {
                return getSkyPathBitmap();
            }
            return skyBitmap;
        }

        public Bitmap getAerialCompareImage()
        {
            if (compareCompleted == true)
            {
                return getAerialCompareBitmap();
            }
            return skyBitmap;
        }

        public Bitmap getElevationCompareImage()
        {
            if (compareCompleted == true)
            {
                return getElevationCompareBitmap();
            }
            return elevationBitmap;
        }

        public Bitmap getSlopeCompareImage()
        {
            if (compareCompleted == true)
            {
                return getSlopeCompareBitmap();
            }
            return slopeBitmap;
        }

        public Bitmap getHazardCompareImage()
        {
            if (compareCompleted == true)
            {
                return getHazardCompareBitmap();
            }
            return hazardBitmap;
        }

        public List<PathNode> getPathModel()
        {
            return pathfinder.getPath();
        }

        public ImageSource getSlopeModelImage()
        {
            return slopeModel.getImageSource();
        }

        public ImageSource getHazardModelImage()
        {
            return hazardModel.getImageSource();
        }

        public ImageSource getPathImage()
        {
            return pathfinder.getPathImage();
        }

        public ImageSource getVehicleImage()
        {
            return imageSource;
        }

        public ImageSource getVehicleInternalMapImage()
        {
            return rover.getPathImage();
        }

        public Bitmap getVehicleInternalBitmap()
        {
            return rover.getPathBitmap();
        }

        public ImageSource getRoverCamImage()
        {
            return rover.getRoverCam();
        }

        public bool isComplete()
        {
            if (rover != null)
            {
                return rover.reachedTarget();
            }
            return false;
        }

        public int getStartX()
        {
            return startX;
        }

        public int getStartY()
        {
            return startY;
        }

        public int getTargetX()
        {
            return targetX;
        }

        public int getTargetY()
        {
            return targetY;
        }

        public String getAlgorithm()
        {
            return searchAlgortihm;
        }

        public bool getKnownMap()
        {
            return knownMap;
        }

        public int getOptimalSteps()
        {
            return compareRover.getPathPoints().Count;
        }

        public int getDKnownSteps()
        {
            return compareRoverD.getPathPoints().Count;
        }

        public int getPathLikeness()
        {
            return (int)pathLikeness;
        }

        public float getPathLikenessDknownToA()
        {
            return (int)likenessDknownToA;
        }

        public float getPathLikenessDunknownToA()
        {
            return (int)likenessDunknownToA;
        }

        public float getPathLikenessDunknownToDknown()
        {
            return (int)likenessDunknownToDknown;
        }

        private Bitmap getElevationCompareBitmap()
        {
            if (elevationCompareBitmap == null)
            {
                elevationCompareBitmap =generateCompareBitmap(elevationBitmap);
            }

            return elevationCompareBitmap;
        }

        private Bitmap getSlopeCompareBitmap()
        {
            if (slopeCompareBitmap == null)
            {
                slopeCompareBitmap = generateCompareBitmap(slopeBitmap);
            }

            return slopeCompareBitmap;
        }


        private Bitmap getHazardCompareBitmap()
        {
            if (hazardCompareBitmap == null)
            {
                hazardCompareBitmap = generateCompareBitmap(hazardBitmap);
            }

            return hazardCompareBitmap;
        }

        private Bitmap getElevationPathBitmap()
        {
            if (elevationPathBitmap == null)
            {
                elevationPathBitmap = generatePathBitmap(elevationBitmap);
            }

            return elevationPathBitmap;
        }

        private Bitmap getSlopePathBitmap()
        {
            if (slopePathBitmap == null)
            {
                slopePathBitmap = generatePathBitmap(slopeBitmap);
            }

            return slopePathBitmap;
        }

        private Bitmap getHazardPathBitmap()
        {
            if (hazardPathBitmap == null)
            {
                hazardPathBitmap = generatePathBitmap(hazardBitmap);
            }

            return hazardPathBitmap;
        }

        public float getRoverSize()
        {
            return roverSize;
        }

        public float getRoverSlope()
        {
            return roverSlope;
        }

        public int[,] getRoverInternalMap()
        {
            return rover.getInternalMap();
        }

        public List<int[]> getPath()
        {
            return rover.getPath();
        }

        #endregion


        #region SET

        public void setRoverSize(float size)
        {
            roverSize = size;
        }

        public void setRoverSlope(float slope)
        {
            roverSlope = slope;
        }


        #endregion


        public SimulationManager()
        {
          
        }


        public void setSimulation(NavigationMapManager navMap , Bitmap skyView)
        {
            mapManager = navMap;

            elevationBitmap = (Bitmap)mapManager.getElevationBitmap().Clone();
            slopeBitmap = (Bitmap)mapManager.getSlopeBitmap().Clone(); 
            hazardBitmap = (Bitmap)mapManager.getHazardBitmap().Clone();
            skyBitmap = (Bitmap)skyView.Clone();

            hazardSectorSize = mapManager.getHazardSectorSize();
            areaSize = mapManager.getAreaSize();
            
            hazardModel = mapManager.hazardModel; // REMOVE
        }

        public void setSimulationValues(int xStart , int yStart, int endX , int endY , String algorithm, bool known)
        {
            startX = xStart;
            startY = yStart;
            targetX = endX;
            targetY = endY;
            searchAlgortihm = algorithm;
            knownMap = known;
        }


        public void startSimulation()
        {
            startSimulationDSTAR(startX, startY, targetX, targetY);
            pathGenerated = true;
        }

        public void generatePath(int startX, int startY, int targetX, int targetY)
        {
            if ((startX + startY + targetX + targetY) == 0)
            {
                pathfinder = new Pathfinder(hazardModel.getModel(), hazardModel.hazardModelImage, 0, 0, r.Next(hazardModel.getModel().GetLength(0)), r.Next(hazardModel.getModel().GetLength(1)));
            }
            else
            {
                try
                {
                    pathfinder = new Pathfinder(hazardModel.getModel(), hazardModel.hazardModelImage, startX, startY, targetX, targetY);
                }
                catch (Exception){ }
            }
        }


        public void runCompareSimulation()
        {
            if (rover.reachedTarget())
            {
                compareRover = new Vehicle(mapManager);
                compareRover.fullEnvironment();
                compareRover.traverseCompare(startX, startY, targetX, targetY);

                compareRoverD = new Vehicle(mapManager);
                compareRoverD.fullEnvironment();
                compareRoverD.traverseMapDstar(startX, startY, targetX, targetY);

                likenessDknownToA = comparePaths(compareRover.getPathPoints(), compareRoverD.getPathPoints());
                likenessDunknownToA = comparePaths(compareRover.getPathPoints(), rover.getPathPoints());
                likenessDunknownToDknown = comparePaths(compareRoverD.getPathPoints(), rover.getPathPoints());

                compareCompleted = true;
            }
        }

        private float comparePaths(List<PathNode> path1 , List<PathNode> path2)
        {
            int count = 0;
            int length = path1.Count;
            List<PathNode> usedPoints = new List<PathNode>();

            foreach (PathNode p in path2)
            {
                foreach (PathNode a in path1)
                {
                    bool isUsed = false;

                    foreach (PathNode u in usedPoints)
                    {
                        if (u == a)
                        {
                            isUsed = true;
                        }
                    }

                    if (isUsed == false)
                    {
                        if ((p.x == a.x) && (p.x == a.y))
                        {
                            count++;
                            usedPoints.Add(a);
                        }
                    }
                }
            }

           float likeness = (float)count / (float)length;
           likeness = likeness * 100f;

            return likeness;
        }

        public void startSimulationKnownMap(int startX, int startY, int endX, int endY)
        {
            Pathfinder pathfinder = new Pathfinder(hazardModel.getModel() , hazardModel.hazardModelImage, startX, startY, endX, endY);

            System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();

            pathfinder.findPath(startX, startY, endX, endY);

            sw.Stop();

            pathfinder.generatePathImage();
            steps = pathfinder.getPath().Count;
            imageSource = pathfinder.getPathImage();

            timeTaken = sw.Elapsed.TotalSeconds + " Seconds";

            stepTraverseStarted = false;
        }

        public void startSimulation(int startX , int startY , int endX , int endY)
        {
            rover = new Vehicle(mapManager);

            System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();

            rover.traverseMap(startX, startY, endX, endY);

            sw.Stop();

            rover.generatePathImage();
            steps = rover.getSteps();
            imageSource = rover.getPathImage();

            timeTaken = sw.Elapsed.TotalSeconds + " Seconds";

            stepTraverseStarted = false;
        }


        public void startSimulationDSTAR(int startX, int startY, int endX, int endY)
        {
            rover = new Vehicle(mapManager);

            System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();

            rover.traverseMapDstar(startX, startY, endX, endY);

            sw.Stop();

            rover.generatePathImage();
            steps = rover.getSteps();
            imageSource = rover.getPathImage();

            timeTaken = sw.Elapsed.TotalSeconds + " Seconds";

            stepTraverseStarted = false;
        }

        public void simulationStepSetUp()
        {
            rover = new Vehicle(mapManager);

            simulationInProgress = true;
            stepSet = true;

            rover.startTraverse(startX,startY ,targetX,targetY);
        }


        public bool isStepSet()
        {
            return stepSet;
        }

        public void simulationStep()
        {
            if (simulationInProgress == true)
            {
                if (rover.reachedTarget() == false)
                {
                    rover.traverseMapDStep();
                    imageSource = rover.getPathImage();
                }
                else
                {
                    rover.generatePathImage();
                    steps = rover.getSteps();
                    imageSource = rover.getPathImage();
                    if (rover.reachedTarget())
                    {
                        pathGenerated = true;
                    }
                }
            }
        }

        public bool simulationComplete()
        {
            if (simulationInProgress == false)
            {
                if (rover.getPathPoints() != null)
                {
                    if (rover.getPathPoints().Count > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        public void nextStep(int startX , int startY , int endX , int endY)
        {
            if (pathGenerated == false)
            {
                if (stepTraverseStarted == false)
                {
                    rover = new Vehicle(mapManager);
                    rover.startTraverse(startX, startY, endX, endY);
                    stepTraverseStarted = true;
                }
                else
                {
                    rover.traverseMapDStep();
                    imageSource = rover.getPathImage();
                    if (rover.reachedTarget())
                    {
                        pathGenerated = true;
                    }
                }
            }
        }




        private Bitmap getComboPathBitmap()
        {
            if (comboPathBitmap == null)
            {
                comboPathBitmap = (Bitmap)skyBitmap.Clone();

                float alpha = 0.2f;

                for (int a = 0; a < comboPathBitmap.Width; a++)
                {
                    for (int b = 0; b < comboPathBitmap.Height; b++)
                    {
                        System.Drawing.Color c2 = comboPathBitmap.GetPixel(a, b);
                        System.Drawing.Color c1 = hazardBitmap.GetPixel(a, b);

                        float red1 = (c1.R /255f);
                        float red2 = (c2.R /255f);
                        float redNew = alpha * red1 + (1 - alpha) * red2;

                        float green1 = (c1.G / 255f);
                        float green2 = (c2.G / 255f);
                        float greenNew = alpha * green1 + (1 - alpha) * green2;

                        float blue1 = (c1.B / 255f);
                        float blue2 = (c2.B / 255f);
                        float blueNew = alpha * blue1 + (1 - alpha) * blue2;
                       
                        
                        int red = (int)(redNew * 255f);
                        int green = (int)(greenNew * 255f);
                        int blue = (int)(blueNew * 255f);

                        System.Drawing.Color newCol = System.Drawing.Color.FromArgb(red,green,blue);
                        comboPathBitmap.SetPixel(a, b, newCol);
                    }
                }

                int xPrev = -1;
                int yPrev = -1;

                foreach (PathNode p in rover.getPathPoints())
                {
                    int x = p.x * hazardSectorSize;
                    int y = p.y * hazardSectorSize;

                    comboPathBitmap.SetPixel(p.x * hazardSectorSize, p.y * hazardSectorSize, pathColor);
                    System.Drawing.Pen pen = new System.Drawing.Pen(pathColor, 5);

                    using (var graphics = Graphics.FromImage(comboPathBitmap))
                    {
                        if ((xPrev != -1) && (yPrev != -1))
                            graphics.DrawLine(pen, x, y, xPrev, yPrev);
                    }

                    xPrev = x;
                    yPrev = y;
                }
            }

            return comboPathBitmap;
        }



        private Bitmap generatePathBitmap(Bitmap oBitmap)
        {
                Bitmap pathBitmap = (Bitmap)oBitmap.Clone();

                int xPrev = -1;
                int yPrev = -1;

                foreach (PathNode p in rover.getPathPoints())
                {
                    int x = p.x * hazardSectorSize;
                    int y = p.y * hazardSectorSize;

                    pathBitmap.SetPixel(p.x * hazardSectorSize, p.y * hazardSectorSize, pathColor);
                    System.Drawing.Pen blackPen = new System.Drawing.Pen(pathColor, 5);

                    using (var graphics = Graphics.FromImage(pathBitmap))
                    {
                        if ((xPrev != -1) && (yPrev != -1))
                            graphics.DrawLine(blackPen, x, y, xPrev, yPrev);
                    }

                    xPrev = x;
                    yPrev = y;
                }

                int sX = startX * hazardSectorSize;
                int sY = startY * hazardSectorSize;
                int tX = targetX * hazardSectorSize;
                int tY = targetY * hazardSectorSize;

                int size = (int)((float)areaSize * 0.02f);
                for (int a = (sX - (size / 2)); a < (sX + (size / 2)); a++)
                {
                    for (int b = (sY - (size / 2)); b < (sY + (size / 2)); b++)
                    {
                        if ((a > 0) && (b > 0))
                        {
                            if ((a < pathBitmap.Width) && (b < pathBitmap.Height))
                            {
                                pathBitmap.SetPixel(a, b, startColor);
                            }
                        }
                    }
                }

                size = (int)((float)areaSize * 0.02f);
                for (int a = (tX - (size / 2)); a < (tX + (size / 2)); a++)
                {
                    for (int b = (tY - (size / 2)); b < (tY + (size / 2)); b++)
                    {
                        if ((a > 0) && (b > 0))
                        {
                            if ((a < pathBitmap.Width) && (b < pathBitmap.Height))
                            {
                                pathBitmap.SetPixel(a, b, targetColor);
                            }
                        }
                    }
                }
            

            return pathBitmap;

        }




        private Bitmap getSkyPathBitmap()
        {
            if (skyPathBitmap == null)
            {
                skyPathBitmap = (Bitmap)skyBitmap.Clone();

                int xPrev = -1;
                int yPrev = -1;

                foreach (PathNode p in rover.getPathPoints())
                {
                    int x = (int)((float)p.x * ((float)hazardSectorSize / ((float)areaSize / (float)skyPathBitmap.Width)));
                    int y = (int)((float)p.y * ((float)hazardSectorSize / ((float)areaSize / (float)skyPathBitmap.Height)));

                    skyPathBitmap.SetPixel(x, y, pathColor);
                    System.Drawing.Pen blackPen = new System.Drawing.Pen(pathColor, 5);

                    using (var graphics = Graphics.FromImage(skyPathBitmap))
                    {
                        if ((xPrev != -1) && (yPrev != -1))
                            graphics.DrawLine(blackPen, x, y, xPrev, yPrev);
                    }

                    xPrev = x;
                    yPrev = y;
                }

                int sX = (int)((float)startX * ((float)hazardSectorSize / ((float)areaSize / (float)skyPathBitmap.Width)));
                int sY = (int)((float)startY * ((float)hazardSectorSize / ((float)areaSize / (float)skyPathBitmap.Height)));
                int tX = (int)((float)targetX * ((float)hazardSectorSize / ((float)areaSize / (float)skyPathBitmap.Width)));
                int tY = (int)((float)targetY * ((float)hazardSectorSize / ((float)areaSize / (float)skyPathBitmap.Height)));

                int size = (int)((float)areaSize * 0.01f);
                for (int a = (sX - (size / 2)); a < (sX + (size / 2)); a++)
                {
                    for (int b = (sY - (size / 2)); b < (sY + (size / 2)); b++)
                    {
                        if ((a > 0) && (b > 0))
                        {
                            if ((a < skyPathBitmap.Width) && (b < skyPathBitmap.Height))
                            {
                                skyPathBitmap.SetPixel(a, b, startColor);
                            }
                        }
                    }
                }

                size = (int)((float)areaSize * 0.01f);
                for (int a = (tX - (size / 2)); a < (tX + (size / 2)); a++)
                {
                    for (int b = (tY - (size / 2)); b < (tY + (size / 2)); b++)
                    {
                        if ((a > 0) && (b > 0))
                        {
                            if ((a < skyPathBitmap.Width) && (b < skyPathBitmap.Height))
                            {
                                skyPathBitmap.SetPixel(a, b, targetColor);
                            }
                        }
                    }
                }
            }

            return skyPathBitmap;
        }


        private Bitmap getAerialCompareBitmap()
        {
            if (aerialCompareBitmap == null)
            {
                aerialCompareBitmap = (Bitmap)skyBitmap.Clone();

                int xPrev = -1;
                int yPrev = -1;

                foreach (PathNode p in rover.getPathPoints())
                {
                    int x = (int)((float)p.x * ((float)hazardSectorSize / ((float)areaSize / (float)aerialCompareBitmap.Width)));
                    int y = (int)((float)p.y * ((float)hazardSectorSize / ((float)areaSize / (float)aerialCompareBitmap.Height)));

                    aerialCompareBitmap.SetPixel(x, y, pathColor);
                    System.Drawing.Pen blackPen = new System.Drawing.Pen(pathColor, 5);

                    using (var graphics = Graphics.FromImage(aerialCompareBitmap))
                    {
                        if ((xPrev != -1) && (yPrev != -1))
                            graphics.DrawLine(blackPen, x, y, xPrev, yPrev);
                    }

                    xPrev = x;
                    yPrev = y;
                }

                xPrev = -1;
                yPrev = -1;

                foreach (PathNode p in compareRover.getPathPoints())
                {
                    int x = (int)((float)p.x * ((float)hazardSectorSize / ((float)areaSize / (float)aerialCompareBitmap.Width)));
                    int y = (int)((float)p.y * ((float)hazardSectorSize / ((float)areaSize / (float)aerialCompareBitmap.Height)));

                    aerialCompareBitmap.SetPixel(x, y, compareColor);
                    System.Drawing.Pen blackPen = new System.Drawing.Pen(compareColor, 5);

                    using (var graphics = Graphics.FromImage(aerialCompareBitmap))
                    {
                        if ((xPrev != -1) && (yPrev != -1))
                            graphics.DrawLine(blackPen, x, y, xPrev, yPrev);
                    }

                    xPrev = x;
                    yPrev = y;
                }

                xPrev = -1;
                yPrev = -1;

                foreach (PathNode p in compareRoverD.getPathPoints())
                {
                    int x = (int)((float)p.x * ((float)hazardSectorSize / ((float)areaSize / (float)aerialCompareBitmap.Width)));
                    int y = (int)((float)p.y * ((float)hazardSectorSize / ((float)areaSize / (float)aerialCompareBitmap.Height)));

                    aerialCompareBitmap.SetPixel(x, y, compareDColor);
                    System.Drawing.Pen blackPen = new System.Drawing.Pen(compareDColor, 5);

                    using (var graphics = Graphics.FromImage(aerialCompareBitmap))
                    {
                        if ((xPrev != -1) && (yPrev != -1))
                            graphics.DrawLine(blackPen, x, y, xPrev, yPrev);
                    }

                    xPrev = x;
                    yPrev = y;
                }

                int sX = (int)((float)startX * ((float)hazardSectorSize / ((float)areaSize / (float)aerialCompareBitmap.Width)));
                int sY = (int)((float)startY * ((float)hazardSectorSize / ((float)areaSize / (float)aerialCompareBitmap.Height)));
                int tX = (int)((float)targetX * ((float)hazardSectorSize / ((float)areaSize / (float)aerialCompareBitmap.Width)));
                int tY = (int)((float)targetY * ((float)hazardSectorSize / ((float)areaSize / (float)aerialCompareBitmap.Height)));

                int size = (int)((float)areaSize * 0.01f);
                for (int a = (sX - (size / 2)); a < (sX + (size / 2)); a++)
                {
                    for (int b = (sY - (size / 2)); b < (sY + (size / 2)); b++)
                    {
                        if ((a > 0) && (b > 0))
                        {
                            if ((a < aerialCompareBitmap.Width) && (b < aerialCompareBitmap.Height))
                            {
                                aerialCompareBitmap.SetPixel(a, b, startColor);
                            }
                        }
                    }
                }

                size = (int)((float)areaSize * 0.01f);
                for (int a = (tX - (size / 2)); a < (tX + (size / 2)); a++)
                {
                    for (int b = (tY - (size / 2)); b < (tY + (size / 2)); b++)
                    {
                        if ((a > 0) && (b > 0))
                        {
                            if ((a < aerialCompareBitmap.Width) && (b < aerialCompareBitmap.Height))
                            {
                                aerialCompareBitmap.SetPixel(a, b, targetColor);
                            }
                        }
                    }
                }
            }
            return aerialCompareBitmap;
        }


        private Bitmap generateCompareBitmap(Bitmap oBitmap)
        {
                Bitmap compareBitmap = (Bitmap)oBitmap.Clone();

                int xPrev = -1;
                int yPrev = -1;

                foreach (PathNode p in rover.getPathPoints())
                {
                    int x = (int)((float)p.x * ((float)hazardSectorSize / ((float)areaSize / (float)compareBitmap.Width)));
                    int y = (int)((float)p.y * ((float)hazardSectorSize / ((float)areaSize / (float)compareBitmap.Height)));

                    compareBitmap.SetPixel(x, y, pathColor);
                    System.Drawing.Pen blackPen = new System.Drawing.Pen(pathColor, 5);

                    using (var graphics = Graphics.FromImage(compareBitmap))
                    {
                        if ((xPrev != -1) && (yPrev != -1))
                            graphics.DrawLine(blackPen, x, y, xPrev, yPrev);
                    }

                    xPrev = x;
                    yPrev = y;
                }

                xPrev = -1;
                yPrev = -1;

                foreach (PathNode p in compareRover.getPathPoints())
                {
                    int x = (int)((float)p.x * ((float)hazardSectorSize / ((float)areaSize / (float)compareBitmap.Width)));
                    int y = (int)((float)p.y * ((float)hazardSectorSize / ((float)areaSize / (float)compareBitmap.Height)));

                    compareBitmap.SetPixel(x, y, compareColor);
                    System.Drawing.Pen blackPen = new System.Drawing.Pen(compareColor, 5);

                    using (var graphics = Graphics.FromImage(compareBitmap))
                    {
                        if ((xPrev != -1) && (yPrev != -1))
                            graphics.DrawLine(blackPen, x, y, xPrev, yPrev);
                    }

                    xPrev = x;
                    yPrev = y;
                }

                xPrev = -1;
                yPrev = -1;

                foreach (PathNode p in compareRoverD.getPathPoints())
                {
                    int x = (int)((float)p.x * ((float)hazardSectorSize / ((float)areaSize / (float)compareBitmap.Width)));
                    int y = (int)((float)p.y * ((float)hazardSectorSize / ((float)areaSize / (float)compareBitmap.Height)));

                    compareBitmap.SetPixel(x, y, compareDColor);
                    System.Drawing.Pen blackPen = new System.Drawing.Pen(compareDColor, 5);

                    using (var graphics = Graphics.FromImage(compareBitmap))
                    {
                        if ((xPrev != -1) && (yPrev != -1))
                            graphics.DrawLine(blackPen, x, y, xPrev, yPrev);
                    }

                    xPrev = x;
                    yPrev = y;
                }

                int sX = startX * hazardSectorSize;
                int sY = startY * hazardSectorSize;
                int tX = targetX * hazardSectorSize;
                int tY = targetY * hazardSectorSize;

                int size = (int)((float)areaSize * 0.02f);
                for (int a = (sX - (size / 2)); a < (sX + (size / 2)); a++)
                {
                    for (int b = (sY - (size / 2)); b < (sY + (size / 2)); b++)
                    {
                        if ((a > 0) && (b > 0))
                        {
                            if ((a < compareBitmap.Width) && (b < compareBitmap.Height))
                            {
                                compareBitmap.SetPixel(a, b, startColor);
                            }
                        }
                    }
                }

                size = (int)((float)areaSize * 0.02f);
                for (int a = (tX - (size / 2)); a < (tX + (size / 2)); a++)
                {
                    for (int b = (tY - (size / 2)); b < (tY + (size / 2)); b++)
                    {
                        if ((a > 0) && (b > 0))
                        {
                            if ((a < compareBitmap.Width) && (b < compareBitmap.Height))
                            {
                                compareBitmap.SetPixel(a, b, targetColor);
                            }
                        }
                    }
                }

            
            return compareBitmap;
        }

        public void updateRoverCam(float pitch, float yaw)
        {
            rover.updateRoverImage(pitch, yaw);
        }


    }
}