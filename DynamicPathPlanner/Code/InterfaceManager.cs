/*      InterfaceManager Class
 *	    AUTHOR: STEWART TAYLOR
 *------------------------------------
 * This class is used to connect the interface code to the main system
 * It seperates how the interface works from how the system works
 *
 * Last Updated: 28/02/2013
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DynamicPathPlanner.Code;
using System.Windows.Media;

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

        public bool connectToPANGU(String hostname, int portNumber)
        {
            //if connection was established connect
            if ((hostname == "localhost") && (portNumber == 10363))
            {
                if (PANGU_Manager.connect(hostname, portNumber) == true)
                {

                    return true;
                }
            }

            return false;
        }

        public void disconnectFromPANGU()
        {
            PANGU_Manager.endConnection();
        }


        public void addLogEntry(String entry)
        {
            logManager.addEntry(entry);
        }

        public void generateModels()
        {
            navigationMapManager = new NavigationMapManager();
            navigationMapManager.generateElevationModel();
            navigationMapManager.generateSlopeModel();
            navigationMapManager.generateHazardModel();
            

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

    }
}
