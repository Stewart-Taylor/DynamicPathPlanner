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

namespace DynamicPathPlanner
{
    class InterfaceManager
    {
        private SimulationManager simulationManager = new SimulationManager();
        private LogManager logManager = new LogManager();
        private PANGU_Manager panguManager = new PANGU_Manager();
        private NavigationMapManager navigationMapManager = new NavigationMapManager();

        public InterfaceManager()
        {


        }

        public bool connectToPANGU(String hostname, int portNumbert)
        {
            //if connection was established connect

            return true;
        }






    }
}
