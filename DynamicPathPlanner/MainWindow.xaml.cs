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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private InterfaceManager interfaceManager = new InterfaceManager();


        private Grid oldGrid;
        private Grid activeGrid;
        private Storyboard activeStoryboard;


        private Storyboard startup_wait;
        private Storyboard elevation_wait;


        BackgroundWorker startup_worker = new BackgroundWorker();
        BackgroundWorker elevation_worker = new BackgroundWorker();

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

            Storyboard startSlideIn = (System.Windows.Media.Animation.Storyboard)FindResource("Startup_SlideIn");
            startSlideIn.Completed += new EventHandler(startSlideIn_Completed);
            BeginStoryboard(startSlideIn);
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
            interfaceManager.generateElevationModel();
            

        }

        private void btn_connect_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            elevation_worker.DoWork += new DoWorkEventHandler(elevationScreen);
            elevation_worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(elevation_worker_complete);

            elevation_worker.RunWorkerAsync();

            //BeginStoryboard(elevation_wait);
            elevation_wait.Begin();

            nextSlide(grid_pangu_slide, grid_elevation_slide, "Pangu_SlideOut", "Elevation_SlideIn");
            
            /*
            String hostname = "";
            int portNumber = 0;
            bool validInput = true;

            try
            {

             //   hostname = txt_hostname.Text;

            }
            catch(Exception ex)
            {
                validInput = false;
            }

            try
            {
            //    String portText = txt_port.Text;
             //   portNumber = int.Parse(portText);
            }
            catch(Exception ex)
            {
                validInput = false;
            }

            if (validInput == true)
            {
                if (interfaceManager.connectToPANGU(hostname, portNumber) == true)
                {
                    System.Windows.Media.Animation.Storyboard storyBoard = (System.Windows.Media.Animation.Storyboard)FindResource("PanguSlideOut");
                    storyBoard.Completed += new EventHandler(storyBoard_Completed);
                    BeginStoryboard(storyBoard);

                    interfaceManager.generateModels();
                    img_elevation.Source = interfaceManager.getElevationModelImage();
                    img_pangu.Source = interfaceManager.getSkyview();
                    img_slope.Source = interfaceManager.getSlopeModelImage();
                    img_hazard.Source = interfaceManager.getHazardModelImage();
                }
                else
                {
                    //DING ERROR
                    lbl_connectError.Text = "Error: Could not connect to PANGU server";
                    lbl_connectError.Visibility = Visibility.Visible;
                }
            }
            else
            {
                //DING ERROR
                lbl_connectError.Text = "Error: Please enter valid connection parameters";
                lbl_connectError.Visibility = Visibility.Visible;
            }


            */
  
        }


        private void panguStartUp(object sender, EventArgs e)
        {
            if (interfaceManager.connectToPANGU() == true)
            {
                System.Threading.Thread.Sleep(2000); // REMOVE
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
            nextSlide(grid_rover_slide, grid_layout, "Rover_SlideOut", "Main_SlideIn");
        }

        private void btn_hazardNext_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            nextSlide(grid_hazard_slide, grid_rover_slide, "Hazard_SlideOut", "Rover_SlideIn");
        }

        private void btn_slopeNext_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            nextSlide(grid_slope_slide, grid_hazard_slide , "Slope_SlideOut" , "Hazard_SlideIn");
        }

        private void btn_elevationNext_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            nextSlide(grid_elevation_slide, grid_slope_slide, "Elevation_SlideOut", "Slope_SlideIn");
        }

        private void btn_startupNext_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            nextSlide(grid_startup_slide, grid_pangu_slide, "Startup_SlideOut", "Pangu_SlideIn");
        }





    }
}
