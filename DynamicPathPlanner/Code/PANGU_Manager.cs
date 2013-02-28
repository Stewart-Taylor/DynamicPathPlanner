/*      PANGU_Manager Class
 *	    AUTHOR: STEWART TAYLOR
 *------------------------------------
 * This class is used as an easy way to simply fetch data from a PANGU server
 * The PANGU Connector contains the wrapper functionality 
 *
 * Last Updated: 28/02/2013
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicPathPlanner.Code
{
    class PANGU_Manager
    {

        private PANGU_Connector connector = new PANGU_Connector();
        private InterfaceManager interfaceManager = new InterfaceManager();

        public PANGU_Manager(InterfaceManager iManager)
        {
            interfaceManager = iManager;

        }



        public bool connect(String hostname, int port)
        {
            if (connector.connect(hostname, port) == true)
            {
                interfaceManager.addLogEntry("Connected");
                return true;
            }

            return false;
        }


    }
}
