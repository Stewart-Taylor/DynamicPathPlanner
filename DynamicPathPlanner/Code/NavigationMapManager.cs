using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace DynamicPathPlanner.Code
{
    class NavigationMapManager
    {

        private ElevationModel elevationModel;
        private SlopeModel slopeModel;
        private HazardModel hazardModel;
      

        public NavigationMapManager()
        {



        }


        public void generateElevationModel()
        {
            elevationModel = new ElevationModel();
            elevationModel.load_PANGU_DEM();
        }

        public void generateSlopeModel()
        {
            slopeModel = new SlopeModel(elevationModel.getModel(), SlopeMax.ALGORITHM);
        }

        public void generateHazardModel()
        {
            hazardModel = new HazardModel();
        }



        public ImageSource getElevationImage()
        {
            return elevationModel.getImageSource();
        }

        public ImageSource getSlopeImage()
        {
            return slopeModel.getImageSource();
        }

        public ImageSource getHazardImage()
        {
           // return hazardModel.getImageSource();
            return null;
        }


    }
}
