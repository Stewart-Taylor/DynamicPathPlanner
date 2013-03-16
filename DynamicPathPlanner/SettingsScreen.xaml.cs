/*      SettingsScreenWindow Class
 *	    AUTHOR: STEWART TAYLOR
 *------------------------------------
 * This class is used to control the System Settings
 * 
 * Last Updated: 16/03/2013
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
		public SettingsScreen()
		{
			this.InitializeComponent();

            setValues();
		}

        private void setValues()
        {
            txt_intervalTime.Text = Properties.Settings.Default.IntervalTime.ToString();
        }

		private void btn_okay_Click(object sender, System.Windows.RoutedEventArgs e)
		{
            saveSettings();
            this.Close();
		}

		private void btn_cancel_Click(object sender, System.Windows.RoutedEventArgs e)
		{
            this.Close();
		}

        private void saveSettings()
        {
            saveInterval();
            Properties.Settings.Default.Save();
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
            }
        }


 

	}
}