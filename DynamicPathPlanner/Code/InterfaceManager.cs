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
        private PANGU_Manager panguManager;
        private NavigationMapManager navigationMapManager = new NavigationMapManager();

        public InterfaceManager()
        {
            panguManager = new PANGU_Manager(this);

        }

        public bool connectToPANGU(String hostname, int portNumber)
        {
            //if connection was established connect
            if ((hostname == "localhost") && (portNumber == 10363))
            {
                if (panguManager.connect(hostname, portNumber) == true)
                {

                    return true;
                }
            }

            return false;
        }

        public void disconnectFromPANGU()
        {
            panguManager.endConnection();
        }


        public void addLogEntry(String entry)
        {
            logManager.addEntry(entry);
        }


        public ImageSource getE()
        {
            ElevationModel e = new ElevationModel(panguManager.getElevationModel(10 ,128,128));

            return e.getImageSource();
        }

    }
}
