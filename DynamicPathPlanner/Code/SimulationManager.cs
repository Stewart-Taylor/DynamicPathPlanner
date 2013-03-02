/*      SimulationManager Class
 *	    AUTHOR: STEWART TAYLOR
 *------------------------------------
 * This class is to run the pathfinding simulation
 * 
 *
 * Last Updated: 02/03/2013
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.ComponentModel;

namespace DynamicPathPlanner.Code
{
    class SimulationManager
    {
        private NavigationMapManager mapManager;
        private ElevationLoader elevationLoader;
        private SlopeModel slopeModel;
        private HazardModel hazardModel;
        private Pathfinder pathfinder;

        private Random r = new Random();

        private bool stepTraverseStarted = false;

        private Vehicle rover;
        private int steps;

        private ImageSource imageSource;

        private String timeTaken;

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
            hazardModel = m.hazardModel; // REMOVE
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

    }
}