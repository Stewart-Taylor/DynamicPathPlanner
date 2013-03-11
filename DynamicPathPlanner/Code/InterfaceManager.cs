﻿/*      InterfaceManager Class
 *	    AUTHOR: STEWART TAYLOR
 *------------------------------------
 * This class is used to connect the interface code to the main system
 * It seperates how the interface works from how the system works
 *
 * Last Updated: 03/03/2013
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
  

        public InterfaceManager(TextBox tBox)
        {
            navigationMapManager = new NavigationMapManager();
            logManager = new LogManager(tBox);
        }


        public bool connectToPANGU()
        {
            String hostname = "localhost";
            int portNumber = 10363;

            Process[] pname = Process.GetProcessesByName("viewer");
            if (pname.Length == 0)
            {
                startPANGU();
            }

            //if connection was established connect
            if (PANGU_Manager.connect(hostname, portNumber) == true)
            {
                addLogEntry("Pangu Started");
                return true;
            }

            return false;
        }

        private void startPANGU()
        {
            String filename = "C:/Users/Stewart/Desktop/Pangu3.30/Pangu3.30/models/PathPlanner_Model/viewer.bat"; // GET FROM CONFIG

            System.Diagnostics.Process proc = new System.Diagnostics.Process(); // Declare New Process
            proc.StartInfo.FileName = filename;
            proc.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName("C:/Users/Stewart/Desktop/Pangu3.30/Pangu3.30/models/PathPlanner_Model/"); // GET FROM CONFIG
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.UseShellExecute = false;

            proc.Start();
            System.Threading.Thread.Sleep(1000);
        }

        public void setEnviornmentString(String environment)
        {
            navigationMapManager.setEnvironmentText(environment);
        }

        public void disconnectFromPANGU()
        {
            PANGU_Manager.endConnection();
        }

        public void addLogEntry(String entry)
        {
            logManager.addEntry(entry );
        }

        public void generateElevationModel(float distance , int size)
        {
            navigationMapManager.generateElevationModel(distance , size);
        }

        public void generateSlopeModel(String type)
        {
            navigationMapManager.generateSlopeModel(type);
        }

        public void generateHazardModel(int size)
        {
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
            return PANGU_Manager.getSkyView(0.1f ,512 );
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
                    x = x * navigationMapManager.getHazardSectorSize();
                    y = y * navigationMapManager.getHazardSectorSize();

                    int size = 10;
                    System.Drawing.Color color = System.Drawing.Color.Blue;
                    for (int a = (x - (size / 2)); a < (x + (size / 2)); a++)
                    {
                        for (int b = (y - (size / 2)); b < (y + (size / 2)); b++)
                        {
                            if ((a> 0) && (b > 0))
                            {
                                if ((a < roverSlideBitmap.Width) && (b < roverSlideBitmap.Height))
                                {
                                    roverSlideBitmap.SetPixel(a, b, color);
                                }
                            }
                        }

                    }
                }
            }
        }

        public bool vehicleValuesValid(int startX , int startY, int targetX , int targetY)
        {
            if ((startX <= 0) || (startY <= 0) || (targetX <= 0) || (targetY <= 0)) 
            {
                return false;
            }

            if ((startX >= navigationMapManager.getHazardWidth()) || (startY >= navigationMapManager.getHazardHeight()) || (targetX >= navigationMapManager.getHazardWidth()) || (targetY >= navigationMapManager.getHazardHeight()))
            {
                return false;
            }

            if ((startX == targetX) && (startY == targetY))
            {
                return false;
            }

            return true;
        }

        public void setVehicleValues(int startX , int startY, int targetX , int targetY , String algorithm, bool knownMap)
        {
                        simulationManager.setSimulation(navigationMapManager , PANGU_Manager.getSkyBitmap(navigationMapManager.getDistanceStep() , navigationMapManager.getAreaSize()));
            simulationManager.setSimulationValues(startX, startY, targetX, targetY, algorithm, knownMap);
        }

        public void updateRoverSlideTargetPosition(int x, int y)
        {
            if ((x > 0) && (y > 0))
            {
                if ((x < navigationMapManager.getHazardWidth()) && (y < navigationMapManager.getHazardHeight()))
                {
                    x = x * navigationMapManager.getHazardSectorSize();
                    y = y * navigationMapManager.getHazardSectorSize();

                    int size = 10;
                    System.Drawing.Color color = System.Drawing.Color.BlueViolet;
                    for (int a = (x - (size / 2)); a < (x + (size / 2)); a++)
                    {
                        for (int b = (y - (size / 2)); b < (y + (size / 2)); b++)
                        {
                            if ((a > 0) && (b > 0))
                            {
                                if ((a < roverSlideBitmap.Width) && (b < roverSlideBitmap.Height))
                                {
                                    roverSlideBitmap.SetPixel(a, b, color);
                                }
                            }
                        }
                    }
                }
            }
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
            Bitmap bitmap = simulationManager.getSkyPathImage();
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

        public bool isSimulationComplete()
        {
            return simulationManager.isComplete();
        }

        public void resetSimulation()
        {
            simulationManager = new SimulationManager(); // CHANGE!
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
    }
}
