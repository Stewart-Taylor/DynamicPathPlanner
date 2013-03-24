/*      ResultsManager Class
 *	    AUTHOR: STEWART TAYLOR
 *------------------------------------
 * This class is used to export results from the simulation
 * The data produced can then be used for more in depth analysis
 * Creates a new folder with a Details file, full simulation log, images, map data , path data
 *
 * Last Updated: 23/03/2013
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace DynamicPathPlanner.Code
{
    class ResultManager
    {

        private String resultFolderPath = "Results";
        private String simulationName;
        private String detailsFilename = "Details.txt";
        private String logFilename = "Log.txt";


        public ResultManager(String name)
        {
            simulationName = name;

            createResultFolder();
            createSimulationResultsFolder();
        }


        public void createResultFolder()
        {
            if (!Directory.Exists(resultFolderPath))
            {
                Directory.CreateDirectory(resultFolderPath);
            }
        }

        public void createSimulationResultsFolder()
        {
            if (!Directory.Exists(resultFolderPath + "/" + simulationName))
            {
                Directory.CreateDirectory(resultFolderPath + "/" + simulationName);
            }

        }

        public void createSimulationDetails(String environment, int areaSize, float distanceStep, String slopeAlgorithm, int hazardSectorSize , int startX , int startY , int targetX , int targetY ,int simulationSteps ,int knownSteps, int optimalSteps , int likeness)
        {
            string path = Path.Combine(resultFolderPath + "/" + simulationName, detailsFilename);

            String date = DateTime.Today.ToString("dd/MM/yyyy");

            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine("TEST");
                sw.WriteLine("Simulation Details File");
                sw.WriteLine("Created " + date); 
                sw.WriteLine("---------------------------------------------");
                sw.WriteLine("SETTINGS");
                sw.WriteLine("---------------------------------------------");
                sw.WriteLine("Environment: " + environment);
                sw.WriteLine("Elevation Area Size: " + areaSize);
                sw.WriteLine("Distance Step: " + distanceStep);
                sw.WriteLine("Slope Algortihm: " + slopeAlgorithm);
                sw.WriteLine("Hazard Sector Size: " + hazardSectorSize);
                sw.WriteLine("Start Position: (" + startX + "," + startY + ")");
                sw.WriteLine("Target Position: (" + targetX + "," + targetY + ")");
                sw.WriteLine("---------------------------------------------");
                sw.WriteLine("Results");
                sw.WriteLine("---------------------------------------------");
                sw.WriteLine("Simulation Steps: " + simulationSteps);
                sw.WriteLine("Simulation Known Steps: " + knownSteps);
                sw.WriteLine("Optimal Steps: " + optimalSteps);
                sw.WriteLine("Path Likeness: " + likeness + "%");
                sw.WriteLine("---------------------------------------------");
            }

        }

        public void createSimulationLog(List<String> entries)
        {
            string path = Path.Combine(resultFolderPath + "/" + simulationName, logFilename);
          
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine("---------------------------------------------");
                sw.WriteLine("SIMULATION LOG");
                sw.WriteLine("---------------------------------------------");
                foreach (String s in entries)
                {
                    sw.WriteLine(s);
                }
            }
        }

        public void createSimulationImages()
        {


            //     Bitmap bmp1 = new Bitmap(typeof(Button), "Button.bmp");
            //     bmp1.Save(@"c:\button.png", ImageFormat.Png);
        }

        public void createSimulationData()
        {
            createElevationFile();
            createSlopeFile();
            createHazardFile();
            createInternalMapFile();
            createPathFile();
        }

        private void createElevationFile()
        {

        }

        private void createSlopeFile()
        {

        }

        private void createHazardFile()
        {

        }

        private void createInternalMapFile()
        {

        }

        private void createPathFile()
        {

        }

    

    }
}
