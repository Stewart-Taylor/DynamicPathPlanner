/*      NavigationMapManager Class
 *	    AUTHOR: STEWART TAYLOR
 *------------------------------------
 * This is used to manage all of the navigational data 
 * It is used to manage and generate the elevation, slope and hazard models
 * It provides helper methods to process and access this data
 * Provides a interface for the VehicleSensorManager to the environment
 *
 * Last Updated: 21/04/2013
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Drawing;
using System.IO;

namespace DynamicPathPlanner.Code
{
    class NavigationMapManager
    {
        private float distanceStep;
        private int areaSize;

        private String environmentText;
        private String environmentPath;

        private ElevationModel elevationModel;
        private SlopeModel slopeModel;
        public HazardModel hazardModel;

        #region GET

        public float getDistanceStep()
        {
            return distanceStep;
        }

        public int getAreaSize()
        {
            return areaSize;
        }

        public int[,] getHazardModel()
        {
            if (hazardModel == null)
            {
                return null;
            }

            return hazardModel.getModel();
        }

        public double[,] getHazardSlope()
        {
            return hazardModel.getHazardSlope();
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

        public Bitmap getElevationBitmap()
        {
            return elevationModel.getBitmap();
        }

        public Bitmap getSlopeBitmap()
        {
            return slopeModel.getBitmap();
        }

        public Bitmap getHazardBitmap()
        {
            return hazardModel.getBitmap();
        }

        public String getEnvironementString()
        {
            String name = Path.GetFileName(environmentText);
            return name;
        }

        public String getEnvironementPath()
        {
            return environmentPath;
        }

        public String getSlopeAlgorithm()
        {
            return slopeModel.getSlopeAlgorithm();
        }

        #endregion


        #region SET

        public void setEnvironmentText(String environment)
        {
            environmentText = environment;
        }

        public void setEnvironmentPath(String environment)
        {
            environmentPath = environment;
        }

        #endregion

        public NavigationMapManager()
        {

        }

        public void generateElevationModel(float distance , int size)
        {
            distanceStep = distance;
            areaSize = size;
            elevationModel = new ElevationModel();
            elevationModel.load(getEnvironementString(), distance, size);
        }

        public void generateSlopeModel(String type)
        {
            slopeModel = new SlopeModel(elevationModel.getModel(), type);
        }

        public void generateHazardModel(int size)
        {
            hazardModel = new HazardModel(slopeModel.getModel(), size);
        }

    }
}
