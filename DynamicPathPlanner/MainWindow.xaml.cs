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

namespace DynamicPathPlanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private InterfaceManager interfaceManager = new InterfaceManager();


        public MainWindow()
        {
            InitializeComponent();

            grid_layout.Visibility = Visibility.Hidden;
          //  grid_pangu.Visibility = Visibility.Hidden;

        }

        private void btn_connect_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            String hostname = "";
            int portNumber = 0;
            bool validInput = true;

            try
            {

                hostname = txt_hostname.Text;

            }
            catch(Exception ex)
            {
                validInput = false;
            }

            try
            {
                String portText = txt_port.Text;
                portNumber = int.Parse(portText);
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



  
        }



        void storyBoard_Completed(object sender, EventArgs e)
        {
            grid_pangu.Visibility = Visibility.Hidden;
            grid_layout.Visibility = Visibility.Visible;

            System.Windows.Media.Animation.Storyboard storyBoard = (System.Windows.Media.Animation.Storyboard)FindResource("MainSlideIn");
            
            BeginStoryboard(storyBoard);
 
        }


        void mainSlideOut_Completed(object sender, EventArgs e)
        {
            grid_pangu.Visibility = Visibility.Visible;
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



    }
}
