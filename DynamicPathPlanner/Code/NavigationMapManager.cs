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
        public HazardModel hazardModel;

        private int hazardSectorSize = 10;

        public NavigationMapManager()
        {



        }


        public void generateElevationModel()
        {
            elevationModel = new ElevationModel();
            elevationModel.load_PANGU_DEM();
        }

        public void generateSlopeModel(String type)
        {
            slopeModel = new SlopeModel(elevationModel.getModel(), type);
        }

        public void generateHazardModel(int size)
        {
            hazardModel = new HazardModel(slopeModel.getModel(), size);
        }


        public double[,] getHazardModel()
        {
            return hazardModel.getModel();
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
            return hazardModel.getImageSource();
        }

        public double[,] getSlopeModel()
        {
            return slopeModel.getModel();
        }

    }
}
