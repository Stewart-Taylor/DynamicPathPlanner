/*      SettingsScreenWindow Class
 *	    AUTHOR: STEWART TAYLOR
 *------------------------------------
 * This class is used to control the System Settings
 * It will load in settings and allow them to be adjusted and then saved
 * 
 * Last Updated: 21/03/2013
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DynamicPathPlanner
{
	public partial class SettingsScreen : Window
	{
        private bool saveValid = false;

		public SettingsScreen()
		{
			this.InitializeComponent();

            setValues();
		}

        private void setValues()
        {
            txt_intervalTime.Text = Properties.Settings.Default.IntervalTime.ToString();
            txt_distanceStep.Text = Properties.Settings.Default.distanceStep.ToString();
        }

        private void btn_okay_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            saveSettings();

            if (saveValid == true)
            {
                this.Close();
            }
        }

		private void btn_cancel_Click(object sender, System.Windows.RoutedEventArgs e)
		{
            this.Close();
		}

        private void saveSettings()
        {
            saveValid = true;
            saveInterval();
            saveDistanceStep();

            if (saveValid == true)
            {
                Properties.Settings.Default.Save();
            }
        }

        private void saveInterval()
        {
            String intervalText = txt_intervalTime.Text;

            try
            {
                int interval = int.Parse(intervalText);
                Properties.Settings.Default.IntervalTime = interval;
            }
            catch
            {
                lbl_error.Text = "Error: Not a valid Interval value";
                saveValid = false;
            }
        }

        private void saveDistanceStep()
        {
            String distanceText = txt_distanceStep.Text;

            try
            {
                float distance = float.Parse(distanceText);
                Properties.Settings.Default.distanceStep = distance;
            }
            catch
            {
                lbl_error.Text = "Error: Not a valid Distance Step value";
                saveValid = false;
            }
        }


 

	}
}