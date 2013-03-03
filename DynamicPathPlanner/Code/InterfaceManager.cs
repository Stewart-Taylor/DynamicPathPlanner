/*      InterfaceManager Class
 *	    AUTHOR: STEWART TAYLOR
 *------------------------------------
 * This class is used to connect the interface code to the main system
 * It seperates how the interface works from how the system works
 *
 * Last Updated: 02/03/2013
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DynamicPathPlanner.Code;
using System.Windows.Media;
using System.Diagnostics;

namespace DynamicPathPlanner
{
    class InterfaceManager
    {
        private SimulationManager simulationManager = new SimulationManager();
        private LogManager logManager = new LogManager();
        private NavigationMapManager navigationMapManager = new NavigationMapManager();

        public InterfaceManager()
        {

        }

        public bool connectToPANGU()
        {

            String hostname = "localhost";
            int portNumber = 10363;
  

            Process[] pname = Process.GetProcessesByName("viewer");
            if (pname.Length == 0)
            {
                startPANGU();
            }



            //if connection was established connect

           if (PANGU_Manager.connect(hostname, portNumber) == true)
           {

              return true;
          }


            return false;
        }


        private void startPANGU()
        {
            String filename = "C:/Users/Stewart/Desktop/Pangu3.30/Pangu3.30/models/PathPlanner_Model/viewer.bat";

            System.Diagnostics.Process proc = new System.Diagnostics.Process(); // Declare New Process
            proc.StartInfo.FileName = filename;
            proc.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName("C:/Users/Stewart/Desktop/Pangu3.30/Pangu3.30/models/PathPlanner_Model/");
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.UseShellExecute = false;

            proc.Start();
            System.Threading.Thread.Sleep(1000);
        }

        public void disconnectFromPANGU()
        {
            PANGU_Manager.endConnection();
        }


        public void addLogEntry(String entry)
        {
            logManager.addEntry(entry);
        }


        public void generateElevationModel()
        {
            navigationMapManager = new NavigationMapManager();
            navigationMapManager.generateElevationModel();
        }

        public void generateSlopeModel(String type)
        {
            navigationMapManager.generateSlopeModel(type);
        }

        public void generateHazardModel(int size)
        {
            navigationMapManager.generateHazardModel(size);
        }

        public void generateModels()
        {
            navigationMapManager = new NavigationMapManager();
            navigationMapManager.generateElevationModel();
            navigationMapManager.generateSlopeModel("Horn");
            navigationMapManager.generateHazardModel(10);
            

        }


        public void startSimulation()
        {
            simulationManager.setSimulation(navigationMapManager);
         //   simulationManager.startSimulation();
            simulationManager.startSimulationDSTAR(1, 1, 5, 20);
        }

        //REMOVE
        public ImageSource getElevationModelImage()
        {
            return navigationMapManager.getElevationImage();
        }


        public ImageSource getSlopeModelImage()
        {
            return navigationMapManager.getSlopeImage();
        }

        public ImageSource getHazardModelImage()
        {
            return navigationMapManager.getHazardImage();
        }

        //REMOVE
        public ImageSource getSkyview()
        {
            return PANGU_Manager.getSkyView();
        }

        public ImageSource getRoverMap()
        {
            return simulationManager.getVehicleImage();
        }

        public bool isSlopeMapGenerated()
        {
            if (navigationMapManager.getSlopeModel() != null)
            {
                return true;
            }
            return false;
        }

    }
}
