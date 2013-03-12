/*      SimulationManager Class
 *	    AUTHOR: STEWART TAYLOR
 *------------------------------------
 * This class is used to run the pathfinding simulation
 * It handles the results of the simulation
 * and generates visual output
 *
 * Last Updated: 10/03/2013
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

        private Random r = new Random();

        private bool stepTraverseStarted = false;

        private Vehicle rover;
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

        #region GET

        public String getTimeTaken()
        {
            return timeTaken;
        }

        public int getSteps()
        {
            return steps;
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
            rover = new Vehicle( mapManager,hazardModel.getModel(), hazardModel.hazardModelImage, areaSize, areaSize);

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
            rover = new Vehicle(mapManager, hazardModel.getModel(), hazardModel.hazardModelImage, areaSize, areaSize);

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
            rover = new Vehicle(mapManager, hazardModel.getModel(), hazardModel.hazardModelImage, areaSize, areaSize);

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
                    rover = new Vehicle(mapManager, hazardModel.getModel(), hazardModel.hazardModelImage, areaSize, areaSize);
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


        public Bitmap getElevationPathImage()
        {
            if( pathGenerated == true)
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

        public Bitmap getSkyPathImage()
        {
            if (pathGenerated == true)
            {
                return getSkyPathBitmap();
            }
            return skyBitmap;
        }

        public void resetSimulation()
        {

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
                    System.Drawing.Color color = System.Drawing.Color.Blue;
                    int x = p.x * hazardSectorSize;
                    int y = p.y * hazardSectorSize;

                    comboPathBitmap.SetPixel(p.x * hazardSectorSize, p.y * hazardSectorSize, color);
                    System.Drawing.Pen blackPen = new System.Drawing.Pen(System.Drawing.Color.Blue, 5);

                    using (var graphics = Graphics.FromImage(comboPathBitmap))
                    {
                        if ((xPrev != -1) && (yPrev != -1))
                            graphics.DrawLine(blackPen, x, y, xPrev, yPrev);
                    }

                    xPrev = x;
                    yPrev = y;
                }
            }

            return comboPathBitmap;
        }


        private Bitmap getElevationPathBitmap()
        {
            if (elevationPathBitmap == null)
            {
                elevationPathBitmap = (Bitmap)elevationBitmap.Clone();

                int xPrev = -1;
                int yPrev = -1;

                foreach (PathNode p in rover.getPathPoints())
                {
                    System.Drawing.Color color = System.Drawing.Color.Blue;
                    int x = p.x * hazardSectorSize;
                    int y = p.y * hazardSectorSize;

                    elevationPathBitmap.SetPixel(p.x * hazardSectorSize, p.y * hazardSectorSize, color);
                    System.Drawing.Pen blackPen = new System.Drawing.Pen(System.Drawing.Color.Blue, 5);

                    using (var graphics = Graphics.FromImage(elevationPathBitmap))
                    {
                        if( (xPrev != -1) && (yPrev != -1))
                        graphics.DrawLine(blackPen, x, y, xPrev, yPrev);
                    }

                    xPrev = x;
                    yPrev = y;
                }
            }

            return elevationPathBitmap;
        }

        private Bitmap getSlopePathBitmap()
        {
            if (slopePathBitmap == null)
            {
                slopePathBitmap = (Bitmap)slopeBitmap.Clone();

                int xPrev = -1;
                int yPrev = -1;

                foreach (PathNode p in rover.getPathPoints())
                {
                    System.Drawing.Color color = System.Drawing.Color.Blue;
                    int x = p.x * hazardSectorSize;
                    int y = p.y * hazardSectorSize;

                    slopePathBitmap.SetPixel(p.x * hazardSectorSize, p.y * hazardSectorSize, color);
                    System.Drawing.Pen blackPen = new System.Drawing.Pen(System.Drawing.Color.Blue, 5);

                    using (var graphics = Graphics.FromImage(slopePathBitmap))
                    {
                        if ((xPrev != -1) && (yPrev != -1))
                            graphics.DrawLine(blackPen, x, y, xPrev, yPrev);
                    }

                    xPrev = x;
                    yPrev = y;
                }
            }

            return slopePathBitmap;
        }

        private Bitmap getHazardPathBitmap()
        {
            if (hazardPathBitmap == null)
            {
                hazardPathBitmap = (Bitmap)hazardBitmap.Clone();

                int xPrev = -1;
                int yPrev = -1;

                foreach (PathNode p in rover.getPathPoints())
                {
                    System.Drawing.Color color = System.Drawing.Color.Blue;
                    int x = p.x * hazardSectorSize;
                    int y = p.y * hazardSectorSize;

                    hazardPathBitmap.SetPixel(p.x * hazardSectorSize, p.y * hazardSectorSize, color);
                    System.Drawing.Pen blackPen = new System.Drawing.Pen(System.Drawing.Color.Blue, 5);

                    using (var graphics = Graphics.FromImage(hazardPathBitmap))
                    {
                        if ((xPrev != -1) && (yPrev != -1))
                            graphics.DrawLine(blackPen, x, y, xPrev, yPrev);
                    }

                    xPrev = x;
                    yPrev = y;
                }
            }

            return hazardPathBitmap;
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
                    System.Drawing.Color color = System.Drawing.Color.Blue;
                    int x = (int)((float)p.x * ((float)hazardSectorSize / ((float)areaSize / (float)skyPathBitmap.Width)));
                    int y = (int)((float)p.y * ((float)hazardSectorSize / ((float)areaSize / (float)skyPathBitmap.Height)));

                    skyPathBitmap.SetPixel(x, y, color);
                    System.Drawing.Pen blackPen = new System.Drawing.Pen(System.Drawing.Color.Blue, 5);

                    using (var graphics = Graphics.FromImage(skyPathBitmap))
                    {
                        if ((xPrev != -1) && (yPrev != -1))
                            graphics.DrawLine(blackPen, x, y, xPrev, yPrev);
                    }

                    xPrev = x;
                    yPrev = y;
                }
            }

            return skyPathBitmap;
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

    }
}