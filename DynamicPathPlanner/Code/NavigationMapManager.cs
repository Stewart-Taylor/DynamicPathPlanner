/*      NavigationMapManager Class
 *	    AUTHOR: STEWART TAYLOR
 *------------------------------------
 * This is used to manage all of the navigational data 
 * It is used to manage and generate the elevation,slope and hazard models
 *
 * Last Updated: 03/03/2013
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Drawing;

namespace DynamicPathPlanner.Code
{
    class NavigationMapManager
    {

        private ElevationModel elevationModel;
        private SlopeModel slopeModel;
        public HazardModel hazardModel;

        private int hazardSectorSize = 10;
        private String environmentText;

        public NavigationMapManager()
        {



        }

        public void setEnvironmentText(String environment)
        {
            environmentText = environment;
        }
        public void generateElevationModel(float distance , int size)
        {
            elevationModel = new ElevationModel();
         //   elevationModel.load_PANGU_DEM(distance , size);
            elevationModel.load(environmentText, distance, size);
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
            if (hazardModel == null)
            {
                return null;
            }
            return hazardModel.getModel();
        }

        public double[,] getElevationModel()
        {
            if (elevationModel == null)
            {
                return null;
            }

            return elevationModel.getModel();
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

        public Bitmap getHazardBitmap()
        {
            return hazardModel.getBitmap();
        }

        public double[,] getSlopeModel()
        {
            if (slopeModel == null)
            {
                return null;
            }
            return slopeModel.getModel();

        }

        public int getHazardSectorSize()
        {
            return hazardModel.getSectorSize();
        }

        public int getHazardWidth()
        {
            return hazardModel.getWidth();
        }

        public int getHazardHeight()
        {
            return hazardModel.getHeight();
        }

    }
}
