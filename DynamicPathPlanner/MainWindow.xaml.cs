/*      MainWindow Class
 *	    AUTHOR: STEWART TAYLOR
 *------------------------------------
 * This class is used to control the interface
 * It mainly controls screen animations and passes commands to the InterfaceManager
 *
 * Last Updated: 13/03/2013
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.ComponentModel;
using System.Windows.Threading;
using DynamicPathPlanner.Code;
using System.Drawing;
using System.IO;
using System.Diagnostics;

namespace DynamicPathPlanner
{
    public partial class MainWindow : Window 
    {

        private InterfaceManager interfaceManager;
        private Grid oldGrid;
        private Grid activeGrid;
        private Storyboard activeStoryboard;
        private Storyboard startSlideIn;

        private Storyboard startup_wait;
        private Storyboard elevation_wait;
        private Storyboard slope_wait;
        private Storyboard hazard_wait;

        private BackgroundWorker startup_worker = new BackgroundWorker();
        private BackgroundWorker elevation_worker = new BackgroundWorker();
        private BackgroundWorker slope_worker = new BackgroundWorker();
        private BackgroundWorker hazard_worker = new BackgroundWorker();
        private BackgroundWorker step_worker = new BackgroundWorker();

        private bool started = false;
        private float elevationDistance;
        private int elevationSize;
        private int hazardSectorSize;
        private String slopeType;

        private int simulationInterval = 1;

        private DispatcherTimer dispatcherTimer = new DispatcherTimer();

        private ImageSource loadImage;

        public MainWindow()
        {
            InitializeComponent();
           
            //Set Storyboards
            startup_wait = (System.Windows.Media.Animation.Storyboard)FindResource("Startup_Wait");
            elevation_wait = (System.Windows.Media.Animation.Storyboard)FindResource("Elevation_Wait");
            slope_wait = (System.Windows.Media.Animation.Storyboard)FindResource("Slope_Wait");
            hazard_wait = (System.Windows.Media.Animation.Storyboard)FindResource("Hazard_Wait");

            //Set Events
            hazard_worker.DoWork += new DoWorkEventHandler(hazardScreen);
            hazard_worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(hazard_worker_complete);
            
            slope_worker.DoWork += new DoWorkEventHandler(slopeScreen);
            slope_worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(slope_worker_complete);

            elevation_worker.DoWork += new DoWorkEventHandler(elevationScreen);
            elevation_worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(elevation_worker_complete);

            dispatcherTimer.Tick += new EventHandler(simulationTick);

            startup_worker.DoWork += new DoWorkEventHandler(panguStartUp);
            startup_worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(startup_worker_complete);

            loadImage = img_hazardSlide.Source;

            applicationSetUp();

            fastSetup(); // Testing Only
        }

        private void applicationSetUp()
        {
            interfaceManager = new InterfaceManager(txt_simulationConsole);

            grid_startup_slide.Visibility = Visibility.Visible;
            grid_elevation_slide.Visibility = Visibility.Hidden;
            grid_pangu_slide.Visibility = Visibility.Hidden;
            grid_slope_slide.Visibility = Visibility.Hidden;
            grid_hazard_slide.Visibility = Visibility.Hidden;
            grid_rover_slide.Visibility = Visibility.Hidden;
            grid_simulation.Visibility = Visibility.Hidden;
            grid_results.Visibility = Visibility.Hidden;

            //Reset Images
            img_elevationSlide.Source = loadImage;
            img_slopeSlide.Source = loadImage;
            img_hazardSlide.Source = loadImage;
            img_roverSlide.Source = loadImage;
            img_simulationMain.Source = loadImage;
            img_simulationInternal.Source = loadImage;
            img_simulationRover.Source = loadImage;

            if (startSlideIn == null)
            {
                startSlideIn = (System.Windows.Media.Animation.Storyboard)FindResource("Startup_SlideIn");
                startSlideIn.Completed += new EventHandler(startSlideIn_Completed);
            }
           
            BeginStoryboard(startSlideIn);

            started = true;
        }

        private void panguStartUp(object sender, EventArgs e)
        {
            if (interfaceManager.connectToPANGU() == true)
            {
                //System.Threading.Thread.Sleep(2000); // REMOVE
            }
        }

        //Used for testing 
        private void fastSetup()
        {
        /*   interfaceManager.connectToPANGU();
            interfaceManager.setEnviornmentString("Moon.pan");
           interfaceManager.generateElevationModel(0.1f, 1024);
           interfaceManager.generateSlopeModel("HORN");
           interfaceManager.generateHazardModel(20);
            interfaceManager.setVehicleValues(2, 2, 25, 40, "D_STAR", false);
            nextSlide(grid_startup_slide, grid_simulation, "Startup_SlideOut", "Simulation_SlideIn");
            */

            interfaceManager.connectToPANGU();
            interfaceManager.setEnviornmentString("Moon.pan");
            interfaceManager.generateElevationModel(0.1f, 1024);
            interfaceManager.generateSlopeModel("HORN");
            interfaceManager.generateHazardModel(1);
            interfaceManager.setVehicleValues(146, 57, 402, 431, "D_STAR", false);
            nextSlide(grid_startup_slide, grid_simulation, "Startup_SlideOut", "Simulation_SlideIn");
    
        }

        private void startSlideIn_Completed(object sender, EventArgs e)
        {
            startup_worker.RunWorkerAsync();

            BeginStoryboard(startup_wait);
        }

        private void startup_worker_complete(object sender, EventArgs e)
        {
            nextSlide(grid_startup_slide, grid_pangu_slide, "Startup_SlideOut", "Pangu_SlideIn");
            startup_wait.Stop();
        }

      
        private void elevation_worker_complete(object sender, EventArgs e)
        {
            elevation_wait.Stop();
            lbl_elevationWait.Text = "";
            img_elevationSlide.Source = interfaceManager.getElevationModelImage();
            btn_elevationNext.Visibility = Visibility.Visible;
        }

        private void slope_worker_complete(object sender, EventArgs e)
        {
            cmb_slopeType.IsEnabled = true;
            slope_wait.Stop();
            lbl_slopeWait.Text = "";
            img_slopeSlide.Source = interfaceManager.getSlopeModelImage();
            btn_slopeNext.Visibility = Visibility.Visible;
        }


        private void hazard_worker_complete(object sender, EventArgs e)
        {
            hazard_wait.Stop();
            lbl_hazardWait.Text = "";
            img_hazardSlide.Source = interfaceManager.getHazardModelImage();
        }


        private void nextSlide(Grid startGrid, Grid nextGrid , String outSlide , String inSlide)
        {
            Storyboard slideOut = (System.Windows.Media.Animation.Storyboard)FindResource(outSlide);
            Storyboard slideIn = (System.Windows.Media.Animation.Storyboard)FindResource(inSlide);
            slideOut.Completed += new EventHandler(slideOut_Completed);
            activeStoryboard = slideIn;
            oldGrid = startGrid;
            activeGrid = nextGrid;

            BeginStoryboard(slideOut);
        }

        private void slideOut_Completed(object sender, EventArgs e)
        {
            oldGrid.Visibility = Visibility.Hidden;
            activeGrid.Visibility = Visibility.Visible;
            BeginStoryboard(activeStoryboard);
        }

        private void elevationScreen(object sender, EventArgs e)
        {
            interfaceManager.generateElevationModel(elevationDistance, elevationSize);
        }

        private void slopeScreen(object sender, EventArgs e)
        {
            interfaceManager.generateSlopeModel(slopeType);
        }

        private void hazardScreen(object sender, EventArgs e)
        {
            interfaceManager.generateHazardModel(hazardSectorSize);
        }

        private void btn_connect_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            bool valid = false;

            String selectedText = "";
            lbl_elevationWait.Text = "";

            if (lst_environment.SelectedValue != null)
            {
                TextBlock temp = (TextBlock)lst_environment.SelectedItem;
                selectedText = temp.Text;
            }

            if (selectedText == "Moon.pan")
            {
                valid = true;
            }

            if (valid == true)
            {
                interfaceManager.setEnviornmentString(selectedText);

                img_elevationSlide.Source = interfaceManager.getQuickView();
                btn_elevationNext.Visibility = Visibility.Hidden;
                nextSlide(grid_pangu_slide, grid_elevation_slide, "Pangu_SlideOut", "Elevation_SlideIn");
            }
        }

        private void btn_roverNext_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            int startX;
            int startY;
            int targetX;
            int targetY;

            String textStartX;
            String textStartY;
            String textTargetX;
            String textTargetY;

            interfaceManager.setRoverSlide();
            try
            {
                textStartX = txt_startX.Text;
                textStartY = txt_startY.Text;

                startX = int.Parse(textStartX);
                startY = int.Parse(textStartY);

                textTargetX = txt_targetX.Text;
                textTargetY = txt_targetY.Text;

                targetX = int.Parse(textTargetX);
                targetY = int.Parse(textTargetY);

                if (interfaceManager.vehicleValuesValid(startX, startY , targetX , targetY))
                {
                    interfaceManager.setVehicleValues(startX, startY, targetX, targetY, "a_star", false);
                   // nextSlide(grid_rover_slide, grid_layout, "Rover_SlideOut", "Main_SlideIn");
                    nextSlide(grid_rover_slide, grid_simulation , "Rover_SlideOut", "Simulation_SlideIn");
                }
            }
            catch
            {
                //Error message
            }
        }

        private void btn_hazardNext_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(interfaceManager.isHazardMapGenerated() == true)
            {
                nextSlide(grid_hazard_slide, grid_rover_slide, "Hazard_SlideOut", "Rover_SlideIn");
                interfaceManager.setRoverSlide();
                img_roverSlide.Source = interfaceManager.getRoverSlideImage();
            }
        }

        private void btn_slopeNext_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (interfaceManager.isSlopeMapGenerated() == true)
            {
                nextSlide(grid_slope_slide, grid_hazard_slide, "Slope_SlideOut", "Hazard_SlideIn");
            }
        }

        private void btn_elevationNext_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (interfaceManager.isElevationMapGenerated())
            {
                nextSlide(grid_elevation_slide, grid_slope_slide, "Elevation_SlideOut", "Slope_SlideIn");
            }
        }

        private void cmb_slopeType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string slopeTypeT = "";
            bool valid = false;

            System.Windows.Controls.ComboBoxItem curItem = ((System.Windows.Controls.ComboBoxItem)cmb_slopeType.SelectedItem);
            if (curItem != null)
            {
                slopeTypeT = curItem.Content.ToString();
            }

            if (slopeType != "")
            {
                valid = true;
            }

            if (valid == true)
            {
                slopeType = slopeTypeT;

                slope_wait.Begin();

                if (slope_worker.IsBusy == false)
                {
                    cmb_slopeType.IsEnabled = false;
                    slope_worker.RunWorkerAsync();
                }
            }
        }

        private void slider_sectorSize_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            if (started == true)
            {
                if (hazard_worker.IsBusy == false)
                {
                    hazardSectorSize = (int)slider_sectorSize.Value;

                    hazard_wait.Begin();
                    hazard_worker.RunWorkerAsync();
                }
            }
        }

        private void roverPositionsUpdated(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (started == true)
            {
                int startX;
                int startY;
                int targetX;
                int targetY;

                String textStartX;
                String textStartY;
                String textTargetX;
                String textTargetY;

                interfaceManager.setRoverSlide();
                try
                {
                    textStartX = txt_startX.Text;
                    textStartY = txt_startY.Text;

                    startX = int.Parse(textStartX);
                    startY = int.Parse(textStartY);

                    interfaceManager.updateRoverSlideStartPosition(startX, startY);
                    img_roverSlide.Source = interfaceManager.getRoverSlideImage();
                }
                catch { }

                try
                {
                    textTargetX = txt_targetX.Text;
                    textTargetY = txt_targetY.Text;

                    targetX = int.Parse(textTargetX);
                    targetY = int.Parse(textTargetY);

                    interfaceManager.updateRoverSlideTargetPosition(targetX, targetY);
                    img_roverSlide.Source = interfaceManager.getRoverSlideImage();
                }
                catch { }
            }
        }

        private void btn_elevationUpdate_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            String distanceTemp;
            String sizeTemp;

            try
            {
                distanceTemp = txt_elevationDistance.Text;
                sizeTemp = txt_elevationSize.Text;

                elevationDistance = float.Parse(distanceTemp);
                elevationSize = int.Parse(sizeTemp);
               
                elevation_worker.RunWorkerAsync();
                BeginStoryboard(elevation_wait);
                btn_elevationNext.Visibility = Visibility.Hidden;
                elevation_wait.Begin();
            }
            catch { }
        }

        private void btn_simulationSky_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            img_simulationMain.Source = interfaceManager.getSimulationAerialImage();
        }

        private void btn_simulationElevation_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            img_simulationMain.Source = interfaceManager.getSimulationElevationImage();
        }

        private void btn_simulationSlope_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            img_simulationMain.Source = interfaceManager.getSimulationSlopeImage();
        }

        private void btn_simulationHazard_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            img_simulationMain.Source = interfaceManager.getSimulationHazardImage();
        }

        private void btn_simulationStart_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            runSimulation();
        }

        private void btn_pause1_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            interfaceManager.resetSimulation();
            nextSlide(grid_simulation, grid_rover_slide, "Simulation_SlideOut", "Rover_SlideIn");
        }

        private void btn_start_Copy2_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            runSimulation();
        }

        private void simulationTick(object sender, EventArgs e)
        {
            if (interfaceManager.isSimulationComplete() == false)
            {
                interfaceManager.simulationStep();
                img_simulationInternal.Source = interfaceManager.getRoverInternalMap();
                img_simulationRover.Source = interfaceManager.getRoverCam();
            }
            else
            {
                interfaceManager.simulationStep();
                dispatcherTimer.Stop();
            }

        }

        private void runSimulation()
        {
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, simulationInterval);
            dispatcherTimer.Start();
        }

        private void img_roverSlide_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                System.Windows.Point pos = Mouse.GetPosition(img_roverSlide);
                double x = (pos.X / (float)interfaceManager.getHazardSectorSize() * ((float)interfaceManager.getAreaSize() / 256));
                double y = (pos.Y / (float)interfaceManager.getHazardSectorSize() * ((float)interfaceManager.getAreaSize() / 256));
                txt_startX.Text = ((int)x).ToString();
                txt_startY.Text = ((int)y).ToString();

                interfaceManager.updateRoverSlideStartPosition((int)x, (int)y);
            }
            else if (e.RightButton == MouseButtonState.Pressed)
            {
                System.Windows.Point pos = Mouse.GetPosition(img_roverSlide);
                double x = (pos.X / (float)interfaceManager.getHazardSectorSize() * ((float)interfaceManager.getAreaSize() / 256));
                double y = (pos.Y / (float)interfaceManager.getHazardSectorSize() * ((float)interfaceManager.getAreaSize() / 256));
                txt_targetX.Text = ((int)x).ToString();
                txt_targetY.Text = ((int)y).ToString();

                interfaceManager.updateRoverSlideTargetPosition((int)x, (int)y);
            }
        }


        private void men_about_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Window aboutWindow = new About();
            aboutWindow.Show(); 
        }

        private void men_new_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            applicationSetUp();
        }

        private void men_exit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_simulationNext_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (interfaceManager.isSimulationComplete())
            {
                interfaceManager.runCompareSimulation();
                lbl_resultSteps.Text = "Steps  [Simulation: " + interfaceManager.getSimulationSteps() + "]  [Optimal: " + interfaceManager.getOptimalSteps() + "]";
                lbl_resultLikeness.Text = "Path Likeness: " + interfaceManager.getPathLikeness() + "%";
                nextSlide(grid_simulation, grid_results, "Simulation_SlideOut", "Results_SlideIn");
            }
        }

        private void btn_resultsSky_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            img_resultsMain.Source = interfaceManager.getResultsAerial();
        }

        private void btn_resultsElevation_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            img_resultsMain.Source = interfaceManager.getResultsElevation();
        }

        private void btn_resultsSlope_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            img_resultsMain.Source = interfaceManager.getResultsSlope();
        }

        private void btn_resultsHazard_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            img_resultsMain.Source = interfaceManager.getResultsHazard();
        }

        private void btn_simulationInstant_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            interfaceManager.startSimulation();
            img_simulationInternal.Source = interfaceManager.getRoverInternalMap();
            img_simulationRover.Source = interfaceManager.getSimulationRoverImage();
        }

    }
}