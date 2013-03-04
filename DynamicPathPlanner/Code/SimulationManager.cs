/*      SimulationManager Class
 *	    AUTHOR: STEWART TAYLOR
 *------------------------------------
 * This class is to run the pathfinding simulation
 * It handles the results of the simulation
 * and generates visual output
 *
 * Last Updated: 04/03/2013
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.ComponentModel;
using System.Drawing;

namespace DynamicPathPlanner.Code
{
    class SimulationManager
    {
        private NavigationMapManager mapManager;
        private ElevationLoader elevationLoader;
        private SlopeModel slopeModel;
        private HazardModel hazardModel;
        private Pathfinder pathfinder;

        private Bitmap skyBitamp;
        private Bitmap elevationBitmap;
        private Bitmap slopeBitmap;
        private Bitmap hazardBitmap;
        private Bitmap skyPathBitamp;
        private Bitmap elevationPathBitmap;
        private Bitmap slopePathBitmap;
        private Bitmap hazardPathBitmap;


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

        private ImageSource imageSource;

        private String timeTaken;

        private bool pathGenerated = false;

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

    


        
        public void setSimulation(NavigationMapManager m)
        {
            mapManager = m;
            elevationBitmap = (Bitmap)mapManager.getElevationBitmap().Clone();
            slopeBitmap = (Bitmap)mapManager.getSlopeBitmap().Clone(); 
            hazardBitmap = (Bitmap)mapManager.getHazardBitmap().Clone(); 
            
            hazardModel = m.hazardModel; // REMOVE
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
                catch (Exception) { }
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


        private void generatePathImage()
        {


        }

        public void startSimulation(int startX , int startY , int endX , int endY)
        {
            rover = new Vehicle(hazardModel.getModel() , hazardModel.hazardModelImage);

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
            rover = new Vehicle(hazardModel.getModel() , hazardModel.hazardModelImage);

            System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();

            rover.traverseMapDstar(startX, startY, endX, endY);

            sw.Stop();

            rover.generatePathImage();
            steps = rover.getSteps();
            imageSource = rover.getPathImage();

            timeTaken = sw.Elapsed.TotalSeconds + " Seconds";

            stepTraverseStarted = false;
        }



        public void nextStep(int startX , int startY , int endX , int endY)
        {
            if (stepTraverseStarted == false)
            {
                rover = new Vehicle(hazardModel.getModel() , hazardModel.hazardModelImage);
                rover.startTraverse(startX, startY, endX, endY);
                stepTraverseStarted = true;
            }
            else
            {
                rover.traverseMapStep();
                imageSource = rover.getPathImage();
            }
        }


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
          //  rover = new Vehicle(hazardModel.getHazardModel());
         //   rover.traverseMap(startX, startY, endX, endY);
        }


        public ImageSource getElevationModelImage()
        {
          //  return elevationLoader.getImageSource();
            return null;
        }


        public Bitmap getElevationPathImage()
        {
            if( pathGenerated == true)
            {
                return getElevationPathBitmap();
            }
            return elevationBitmap;
        }


        private Bitmap getElevationPathBitmap()
        {
            if (elevationPathBitmap == null)
            {
                elevationPathBitmap = (Bitmap)elevationBitmap.Clone();

                foreach (PathNode p in rover.getPathPoints())
                {
                    System.Drawing.Color color = System.Drawing.Color.Blue;
                    elevationPathBitmap.SetPixel(p.x*10, p.y*10, color);
                }
            }

            return elevationPathBitmap;
        }


        public ImageSource getElevationImage()
        {
          //  return 
            return null;
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

    }
}