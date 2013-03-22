using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DynamicPathPlanner.Code
{
    class ResultManager
    {

        private String resultFolderPath = "Results";
        private String simulationName;



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

        public void createSimulationDetails()
        {
    

        }

        public void createSimulationLog()
        {


        }

        public void createSimulationImages()
        {



        }

        public void createSimulationData()
        {


        }

    

    }
}
