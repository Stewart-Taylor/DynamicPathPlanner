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
        public MainWindow()
        {
            InitializeComponent();

          //  grid_layout.Visibility = Visibility.Hidden;
            grid_pangu.Visibility = Visibility.Hidden;

        }

        private void btn_connect_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
          //  grid_layout.Visibility = Visibility.Visible;
          //  grid_pangu.Visibility = Visibility.Hidden;

            System.Windows.Media.Animation.Storyboard storyBoard = (System.Windows.Media.Animation.Storyboard)FindResource("PanguSlideOut");
            storyBoard.Completed += new EventHandler(storyBoard_Completed);
            BeginStoryboard(storyBoard);
        }



        void storyBoard_Completed(object sender, EventArgs e)
        {
            grid_pangu.Visibility = Visibility.Hidden;
            grid_layout.Visibility = Visibility.Visible;

            System.Windows.Media.Animation.Storyboard storyBoard = (System.Windows.Media.Animation.Storyboard)FindResource("MainSlideIn");
            
            BeginStoryboard(storyBoard);
 
        }
    }
}
