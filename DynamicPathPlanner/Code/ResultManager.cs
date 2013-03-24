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
using System.Drawing.Imaging;

namespace DynamicPathPlanner.Code
{
    class ResultManager
    {

        private String resultFolderPath = "Results";
        private String imageFolder = "Images";
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

            // Images Folder
            if (!Directory.Exists(resultFolderPath + "/" + simulationName + "/" + imageFolder))
            {
                Directory.CreateDirectory(resultFolderPath + "/" + simulationName + "/" + imageFolder);
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

        public void createSimulationImages(Bitmap aerial, Bitmap elevation, Bitmap slope, Bitmap hazard, Bitmap aerialPath, Bitmap elevationPath, Bitmap slopePath, Bitmap hazardPath, Bitmap aerialCompare, Bitmap elevationCompare, Bitmap slopeCompare, Bitmap hazardCompare, Bitmap internalMap)
        {

            aerial.Save((resultFolderPath + "/" + simulationName + "/" + imageFolder + "/Aerial.png"), ImageFormat.Png);
            elevation.Save((resultFolderPath + "/" + simulationName + "/" + imageFolder + "/Elevation.png"), ImageFormat.Png);
            slope.Save((resultFolderPath + "/" + simulationName + "/" + imageFolder + "/Slope.png"), ImageFormat.Png);
            hazard.Save((resultFolderPath + "/" + simulationName + "/" + imageFolder + "/Hazard.png"), ImageFormat.Png);
            aerialPath.Save((resultFolderPath + "/" + simulationName + "/" + imageFolder + "/aerialPath.png"), ImageFormat.Png);
            elevationPath.Save((resultFolderPath + "/" + simulationName + "/" + imageFolder + "/elevationPath.png"), ImageFormat.Png);
            slopePath.Save((resultFolderPath + "/" + simulationName + "/" + imageFolder + "/slopePath.png"), ImageFormat.Png);
            hazardPath.Save((resultFolderPath + "/" + simulationName + "/" + imageFolder + "/hazardPath.png"), ImageFormat.Png);
            aerialCompare.Save((resultFolderPath + "/" + simulationName + "/" + imageFolder + "/aerialCompare.png"), ImageFormat.Png);
            elevationCompare.Save((resultFolderPath + "/" + simulationName + "/" + imageFolder + "/elevationCompare.png"), ImageFormat.Png);
            slopeCompare.Save((resultFolderPath + "/" + simulationName + "/" + imageFolder + "/slopeCompare.png"), ImageFormat.Png);
            hazardCompare.Save((resultFolderPath + "/" + simulationName + "/" + imageFolder + "/hazardCompare.png"), ImageFormat.Png);
            internalMap.Save((resultFolderPath + "/" + simulationName + "/" + imageFolder + "/internalMap.png"), ImageFormat.Png);

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
