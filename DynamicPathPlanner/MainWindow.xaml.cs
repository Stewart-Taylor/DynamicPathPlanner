/*      MainWindow Class
 *	    AUTHOR: STEWART TAYLOR
 *------------------------------------
 * This class is used to control the interface
 * It mainly controls screen animations and passes commands to the InterfaceManager
 *
 * Last Updated: 02/03/2013
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

namespace DynamicPathPlanner
{
    public partial class MainWindow : Window
    {

        private InterfaceManager interfaceManager = new InterfaceManager();

        private Grid oldGrid;
        private Grid activeGrid;
        private Storyboard activeStoryboard;

        private Storyboard startup_wait;
        private Storyboard elevation_wait;
        private Storyboard slope_wait;
        private Storyboard hazard_wait;

        private BackgroundWorker startup_worker = new BackgroundWorker();
        private BackgroundWorker elevation_worker = new BackgroundWorker();
        private BackgroundWorker slope_worker = new BackgroundWorker();
        private BackgroundWorker hazard_worker = new BackgroundWorker();

        private bool started = false;

        private float elevationDistance;
        private int elevationSize;
        private String slopeType;
        private int hazardSectorSize;

        public MainWindow()
        {
            InitializeComponent();

            grid_startup_slide.Visibility = Visibility.Visible;
            grid_layout.Visibility = Visibility.Hidden;
            grid_elevation_slide.Visibility = Visibility.Hidden;
            grid_pangu_slide.Visibility = Visibility.Hidden;
            grid_slope_slide.Visibility = Visibility.Hidden;
            grid_hazard_slide.Visibility = Visibility.Hidden;
            grid_rover_slide.Visibility = Visibility.Hidden;
            grid_layout.Visibility = Visibility.Hidden;


            //Set Storyboards
            startup_wait = (System.Windows.Media.Animation.Storyboard)FindResource("Startup_Wait");
            elevation_wait = (System.Windows.Media.Animation.Storyboard)FindResource("Elevation_Wait");
            slope_wait = (System.Windows.Media.Animation.Storyboard)FindResource("Slope_Wait");
            hazard_wait = (System.Windows.Media.Animation.Storyboard)FindResource("Hazard_Wait");

            Storyboard startSlideIn = (System.Windows.Media.Animation.Storyboard)FindResource("Startup_SlideIn");
            startSlideIn.Completed += new EventHandler(startSlideIn_Completed);
            BeginStoryboard(startSlideIn);

            started = true;
        }


        private void startSlideIn_Completed(object sender, EventArgs e)
        {
            startup_worker.DoWork += new DoWorkEventHandler(panguStartUp);
            startup_worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(startup_worker_complete);

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
           
          

                interfaceManager.generateElevationModel(elevationDistance,elevationSize);
           

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
            //    elevation_worker.DoWork += new DoWorkEventHandler(elevationScreen);
             //   elevation_worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(elevation_worker_complete);

             //   elevation_worker.RunWorkerAsync();

                //BeginStoryboard(elevation_wait);
           //     btn_elevationNext.Visibility = Visibility.Hidden;
           //     elevation_wait.Begin();

                
                img_elevationSlide.Source = interfaceManager.getSkyview();
                btn_elevationNext.Visibility = Visibility.Hidden;
                nextSlide(grid_pangu_slide, grid_elevation_slide, "Pangu_SlideOut", "Elevation_SlideIn");

            }
       
  
        }


        private void panguStartUp(object sender, EventArgs e)
        {
            if (interfaceManager.connectToPANGU() == true)
            {
                //System.Threading.Thread.Sleep(2000); // REMOVE
            }
        }


        void storyBoard_Completed(object sender, EventArgs e)
        {
            grid_pangu_slide.Visibility = Visibility.Hidden;
            grid_layout.Visibility = Visibility.Visible;

            System.Windows.Media.Animation.Storyboard storyBoard = (System.Windows.Media.Animation.Storyboard)FindResource("MainSlideIn");
            
            BeginStoryboard(storyBoard);
 
        }


        void mainSlideOut_Completed(object sender, EventArgs e)
        {
            grid_pangu_slide.Visibility = Visibility.Visible;
            grid_layout.Visibility = Visibility.Hidden;

            System.Windows.Media.Animation.Storyboard storyBoard = (System.Windows.Media.Animation.Storyboard)FindResource("PanguSlideIn");

            BeginStoryboard(storyBoard);

        }

        private void btn_disconnect_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            interfaceManager.disconnectFromPANGU();
        	// TODO: Add event handler implementation here.
            System.Windows.Media.Animation.Storyboard storyBoard = (System.Windows.Media.Animation.Storyboard)FindResource("MainSlideOut");
            storyBoard.Completed += new EventHandler(mainSlideOut_Completed);
            BeginStoryboard(storyBoard);
        }

        private void btn_start_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            interfaceManager.startSimulation();
            img_internalMap.Source = interfaceManager.getRoverMap();
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
                    nextSlide(grid_rover_slide, grid_layout, "Rover_SlideOut", "Main_SlideIn");
                }
            }
            catch (Exception ex)
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
     //           slope_worker.DoWork += new DoWorkEventHandler(slopeScreen);
        //        slope_worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(slope_worker_complete);

         //       slope_wait.Begin();
         //       slope_worker.RunWorkerAsync();

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
              //  interfaceManager.generateSlopeModel(slopeType);
              //  img_slopeSlide.Source = interfaceManager.getSlopeModelImage();


                slope_worker.DoWork += new DoWorkEventHandler(slopeScreen);
                slope_worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(slope_worker_complete);

                slope_wait.Begin();
                slope_worker.RunWorkerAsync();


            }
            

        }



        private void slider_sectorSize_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            if (started == true)
            {
                if (hazard_worker.IsBusy == false)
                {
                    hazardSectorSize = (int)slider_sectorSize.Value;

                    hazard_worker.DoWork += new DoWorkEventHandler(hazardScreen);
                    hazard_worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(hazard_worker_complete);

                    hazard_wait.Begin();


                    hazard_worker.RunWorkerAsync();


                }
                //   interfaceManager.generateHazardModel(size);
                //   img_hazardSlide.Source = interfaceManager.getHazardModelImage();
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
                catch (Exception ex)
                {

                }

                try
                {
                    textTargetX = txt_targetX.Text;
                    textTargetY = txt_targetY.Text;

                    targetX = int.Parse(textTargetX);
                    targetY = int.Parse(textTargetY);

                    interfaceManager.updateRoverSlideTargetPosition(targetX, targetY);
                    img_roverSlide.Source = interfaceManager.getRoverSlideImage();
                }
                catch (Exception ex)
                {


                }
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
               
                elevation_worker.DoWork += new DoWorkEventHandler(elevationScreen);
                elevation_worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(elevation_worker_complete);

                elevation_worker.RunWorkerAsync();

                BeginStoryboard(elevation_wait);
                btn_elevationNext.Visibility = Visibility.Hidden;
                elevation_wait.Begin();

            }
            catch (Exception ex)
            {

            }


        }





    }
}
