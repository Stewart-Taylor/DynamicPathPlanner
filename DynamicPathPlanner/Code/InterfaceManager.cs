/*      InterfaceManager Class
 *	    AUTHOR: STEWART TAYLOR
 *------------------------------------
 * This class is used to connect the interface code to the main system
 * It seperates how the interface works from how the system works
 *
 * Last Updated: 25/03/2013
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DynamicPathPlanner.Code;
using System.Windows.Media;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace DynamicPathPlanner
{
    class InterfaceManager
    {
        private SimulationManager simulationManager = new SimulationManager();
        private LogManager logManager;
        private NavigationMapManager navigationMapManager = new NavigationMapManager();

        private Bitmap roverSlideBitmap;

        private bool panguStartError = false;


        #region GET

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

        public ImageSource getQuickView()
        {
            Bitmap bitmap = PANGU_Manager.getImageView(0, -150, 200, 10, -40, 0);
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();

            return bi;
        }

        public ImageSource getAerialView()
        {
            return PANGU_Manager.getSkyView(navigationMapManager.getDistanceStep(), navigationMapManager.getAreaSize());
        }

        public ImageSource getRoverMap()
        {
            return simulationManager.getVehicleImage();
        }

        public ImageSource getRoverInternalMap()
        {
            return simulationManager.getVehicleInternalMapImage();
        }


        public bool isElevationMapGenerated()
        {
            if (navigationMapManager.getElevationModel() != null)
            {
                return true;
            }
            return false;
        }

        public bool isSlopeMapGenerated()
        {
            if (navigationMapManager.getSlopeModel() != null)
            {
                return true;
            }
            return false;
        }

        public bool isHazardMapGenerated()
        {
            if (navigationMapManager.getHazardModel() != null)
            {
                return true;
            }
            return false;
        }

        public ImageSource getResultsAerial()
        {
            Bitmap bitmap = simulationManager.getAerialCompareImage();
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();

            return bi;
        }

        public ImageSource getResultsElevation()
        {
            Bitmap bitmap = simulationManager.getElevationCompareImage();
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();

            return bi;
        }

        public ImageSource getResultsSlope()
        {
            Bitmap bitmap = simulationManager.getSlopeCompareImage();
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();

            return bi;
        }

        public ImageSource getResultsHazard()
        {
            Bitmap bitmap = simulationManager.getHazardCompareImage();
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();

            return bi;
        }

        public int getSimulationSteps()
        {
            return simulationManager.getSteps();
        }

        public int getOptimalSteps()
        {
            return simulationManager.getOptimalSteps();
        }

        public int getDKnownSteps()
        {
            return simulationManager.getDKnownSteps();
        }

        public float getPathLikeness()
        {
            return simulationManager.getPathLikeness();
        }

        public float getPathLikenessDknownToA()
        {
            return simulationManager.getPathLikenessDknownToA();
        }

        public float getPathLikenessDunknownToA()
        {
            return simulationManager.getPathLikenessDunknownToA();
        }

        public float getPathLikenessDunknownToDknown()
        {
            return simulationManager.getPathLikenessDunknownToDknown();
        }

        public int getHazardSectorSize()
        {
            return navigationMapManager.getHazardSectorSize();
        }

        public int getAreaSize()
        {
            return navigationMapManager.getAreaSize();
        }

        public ImageSource getRoverCam()
        {
            return simulationManager.getRoverCamImage();
        }

        public ImageSource getRoverSlideImage()
        {
            MemoryStream ms = new MemoryStream();
            roverSlideBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();

            return bi;
        }

        public ImageSource getSimulationElevationImage()
        {
            Bitmap bitmap = simulationManager.getElevationPathImage();
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();

            return bi;
        }

        public ImageSource getSimulationSlopeImage()
        {
            Bitmap bitmap = simulationManager.getSlopePathImage();
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();

            return bi;
        }

        public ImageSource getSimulationHazardImage()
        {
            Bitmap bitmap = simulationManager.getHazardPathImage();
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();

            return bi;
        }


        public ImageSource getSimulationComboImage()
        {
            Bitmap bitmap = simulationManager.getComboPathImage();
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();

            return bi;
        }

        public ImageSource getSimulationAerialImage()
        {
            Bitmap bitmap = simulationManager.getAerialPathImage();
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();

            return bi;
        }

        public ImageSource getSimulationRoverImage()
        {
            return simulationManager.getRoverCamImage();
        }

        public String getEnvironmentString()
        {
            return navigationMapManager.getEnvironementString();
        }

        public String getEnvironmentPath()
        {
            return navigationMapManager.getEnvironementPath();
        }

        #endregion



        #region SET

        public void setEnviornmentString(String environment)
        {
            navigationMapManager.setEnvironmentText(environment);
        }

        public void setEnviornmentPath(String environmentPath)
        {
            navigationMapManager.setEnvironmentPath(environmentPath);
        }

        public void setVehicleValues(int startX, int startY, int targetX, int targetY, String algorithm, bool knownMap)
        {
            simulationManager.setSimulation(navigationMapManager, PANGU_Manager.getSkyBitmap(navigationMapManager.getDistanceStep(), navigationMapManager.getAreaSize()));
            simulationManager.setSimulationValues(startX, startY, targetX, targetY, algorithm, knownMap);
        }

        public void setRoverSize(float roverSize)
        {
            simulationManager.setRoverSize(roverSize);
        }

        public void setRoverSlope(float roverSlope)
        {
            simulationManager.setRoverSlope(roverSlope);
        }

        #endregion


        public InterfaceManager(TextBox tBox)
        {
            PANGU_Manager.killPANGU();
            navigationMapManager = new NavigationMapManager();
            logManager = new LogManager(tBox);
            logManager.clearSimulationLog();
        }



        public bool connectToPANGU(String pan)
        {
            panguStartError = false;

            String hostname = "localhost";
            int portNumber = 10363;

            Process[] pname = Process.GetProcessesByName("viewer");
            if (pname.Length == 0)
            {
                startPANGU(pan);
            }

            if (panguStartError == false)
            {
                //if connection was established connect
                if (PANGU_Manager.connect(hostname, portNumber) == true)
                {
                    addLogEntry("Pangu Connection Established");
                    return true;
                }
            }

            return false;
        }

        private void startPANGU(String pan)
        {

            try
            {
                String filename = Properties.Settings.Default.panguDirectory + "/bin/viewer.exe";
                addLogEntry("Starting PANGU : " + filename);
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = filename;
                proc.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(filename);
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.Arguments = "-server \"" + pan + "\"";
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.UseShellExecute = false;
                proc.Start();
                System.Threading.Thread.Sleep(1000);
            }
            catch
            {
                panguStartError = true;
                addLogEntry("ERROR: Could not start PANGU Server!");
            }
        }


        public void disconnectFromPANGU()
        {
            addLogEntry("Disconnected from PANGU");
            PANGU_Manager.endConnection();
        }

        public void addLogEntry(String entry)
        {
            logManager.addEntry(entry );
        }

        public void generateElevationModel(float distance , int size)
        {
            addLogEntry("Generating Elevation Model");
            navigationMapManager.generateElevationModel(distance , size);
        }

        public void generateSlopeModel(String type)
        {
            addLogEntry("Generating Slope Model");
            navigationMapManager.generateSlopeModel(type);
        }

        public void generateHazardModel(int size)
        {
            addLogEntry("Generating Hazard Model");
            navigationMapManager.generateHazardModel(size);
        }

        public void generateModels()
        {
            navigationMapManager = new NavigationMapManager();
            navigationMapManager.generateElevationModel(1,1);
            navigationMapManager.generateSlopeModel("Horn");
            navigationMapManager.generateHazardModel(10);
        }

        public void startSimulation()
        {
            logManager.addEntry("Simulation Started");
            simulationManager.setSimulation(navigationMapManager , PANGU_Manager.getSkyBitmap(navigationMapManager.getDistanceStep() , navigationMapManager.getAreaSize()));
            simulationManager.startSimulation();
            logManager.addEntry("Simulation Complete");
        }


        public void simulationStepSetUp(int startX, int startY, int targetX, int targetY, String algorithm, bool knownMap)
        {
            simulationManager.setSimulation(navigationMapManager, PANGU_Manager.getSkyBitmap(navigationMapManager.getDistanceStep(), navigationMapManager.getAreaSize()));
            simulationManager.setSimulationValues(startX, startY, targetX, targetY, algorithm, knownMap);
            simulationManager.simulationStepSetUp();
        }

        public void simulationStep()
        {
            if (simulationManager.isStepSet())
            {
                simulationManager.simulationStep();
            }
            else
            {
                simulationStepSetUp(simulationManager.getStartX(), simulationManager.getStartY(), simulationManager.getTargetX(), simulationManager.getTargetY(), simulationManager.getAlgorithm(), simulationManager.getKnownMap());
                simulationManager.simulationStep();
            }
        }


        public void setRoverSlide()
        {
            roverSlideBitmap = (Bitmap)navigationMapManager.getHazardBitmap().Clone();
        }

        public void updateRoverSlideStartPosition(int x, int y)
        {
            if( (x > 0) && ( y >0))
            {
                if( (x < navigationMapManager.getHazardWidth()) && (y < navigationMapManager.getHazardHeight()))
                {

                    BitmapHelper bH = new BitmapHelper(roverSlideBitmap);
                    bH.LockBitmap();

                    x = x * navigationMapManager.getHazardSectorSize();
                    y = y * navigationMapManager.getHazardSectorSize();

                    int size = (int)((float)navigationMapManager.getAreaSize() * 0.02f);
                    System.Drawing.Color color = System.Drawing.Color.Blue;
                    for (int a = (x - (size / 2)); a < (x + (size / 2)); a++)
                    {
                        for (int b = (y - (size / 2)); b < (y + (size / 2)); b++)
                        {
                            if ((a> 0) && (b > 0))
                            {
                                if ((a < roverSlideBitmap.Width) && (b < roverSlideBitmap.Height))
                                {
                                    bH.SetPixel(a, b, color);
                                }
                            }
                        }

                    }
                    bH.UnlockBitmap();
                    roverSlideBitmap = bH.Bitmap;
                }
            }
        }

        public bool vehicleValuesValid(int startX , int startY, int targetX , int targetY)
        {
            if ((startX <= 0) || (startY <= 0) || (targetX <= 0) || (targetY <= 0)) 
            {
                addLogEntry("Vehicle Positions Not Valid");
                return false;
            }

            if ((startX >= navigationMapManager.getHazardWidth()) || (startY >= navigationMapManager.getHazardHeight()) || (targetX >= navigationMapManager.getHazardWidth()) || (targetY >= navigationMapManager.getHazardHeight()))
            {
                addLogEntry("Vehicle Positions Not Valid");
                return false;
            }

            if ((startX == targetX) && (startY == targetY))
            {
                addLogEntry("Vehicle Positions Not Valid");
                return false;
            }

            return true;
        }

        public void updateRoverSlideTargetPosition(int x, int y)
        {
            if ((x > 0) && (y > 0))
            {
                if ((x < navigationMapManager.getHazardWidth()) && (y < navigationMapManager.getHazardHeight()))
                {

                    BitmapHelper bH = new BitmapHelper(roverSlideBitmap);
                    bH.LockBitmap();

                    x = x * navigationMapManager.getHazardSectorSize();
                    y = y * navigationMapManager.getHazardSectorSize();

                    int size =  (int)((float)navigationMapManager.getAreaSize() * 0.02f);
                    System.Drawing.Color color = System.Drawing.Color.BlueViolet;
                    for (int a = (x - (size / 2)); a < (x + (size / 2)); a++)
                    {
                        for (int b = (y - (size / 2)); b < (y + (size / 2)); b++)
                        {
                            if ((a > 0) && (b > 0))
                            {
                                if ((a < roverSlideBitmap.Width) && (b < roverSlideBitmap.Height))
                                {
                                    bH.SetPixel(a, b, color);
                                }
                            }
                        }
                    }

                    bH.UnlockBitmap();
                    roverSlideBitmap = bH.Bitmap;

                }
            }
        }

        public void updateRoverCam(float pitch, float yaw)
        {
            simulationManager.updateRoverCam(pitch, yaw);
        }

        public bool isSimulationComplete()
        {
            return simulationManager.isComplete();
        }

        public void resetSimulation()
        {
            addLogEntry("Simulation Reset");
            simulationManager = new SimulationManager(); 
        }

        public void runCompareSimulation()
        {
            addLogEntry("Simulation Compare Started");
            simulationManager.runCompareSimulation();
            addLogEntry("Simulation Compare Complete");
        }


        public void exportResults()
        {
            addLogEntry("Result Export Started");

            String simName = navigationMapManager.getEnvironementString() + " " + DateTime.Now.ToString("dd-MM-yyyy-HH-mm");

            ResultManager results = new ResultManager(simName);
            results.createSimulationDetails(navigationMapManager.getEnvironementString(), navigationMapManager.getAreaSize(), navigationMapManager.getDistanceStep(), navigationMapManager.getSlopeAlgorithm(), navigationMapManager.getHazardSectorSize(), simulationManager.getStartX(), simulationManager.getStartY(), simulationManager.getTargetX(), simulationManager.getTargetY(), simulationManager.getSteps(), simulationManager.getDKnownSteps(), simulationManager.getOptimalSteps(), (int)simulationManager.getPathLikenessDknownToA(), (int)simulationManager.getPathLikenessDunknownToA(), (int)simulationManager.getPathLikenessDunknownToDknown()); 
            results.createSimulationLog(logManager.getEntries());
            results.createSimulationData(navigationMapManager.getElevationModel(), navigationMapManager.getSlopeModel(), navigationMapManager.getHazardModel() , simulationManager.getRoverInternalMap() , simulationManager.getPath());
            results.createSimulationImages(PANGU_Manager.getSkyBitmap(navigationMapManager.getDistanceStep() , navigationMapManager.getAreaSize()), navigationMapManager.getElevationBitmap(), navigationMapManager.getSlopeBitmap(),navigationMapManager.getHazardBitmap(), simulationManager.getAerialPathImage(), simulationManager.getElevationPathImage(), simulationManager.getSlopePathImage(), simulationManager.getHazardPathImage(), simulationManager.getAerialCompareImage(), simulationManager.getElevationCompareImage(), simulationManager.getSlopeCompareImage(), simulationManager.getHazardCompareImage(), simulationManager.getVehicleInternalBitmap());
            
        }

    }
}
