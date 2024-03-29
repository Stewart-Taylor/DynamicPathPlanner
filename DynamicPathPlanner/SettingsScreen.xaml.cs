﻿/*      SettingsScreenWindow Class
 *	    AUTHOR: STEWART TAYLOR
 *------------------------------------
 * This class is used to control the System Settings
 * It will load in settings and allow them to be adjusted and then saved
 * Distance step has been disabled. It can be turned on by altering the settings XAML
 * 
 * Last Updated: 25/04/2013
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
using Microsoft.Win32;

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
            txt_panguPath.Text = Properties.Settings.Default.panguDirectory;
            txt_environmentPath.Text = Properties.Settings.Default.panDirectory;
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
            savePanguDirectory();
            saveEnvironementDirectory();

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
                if ((interval >= 10) && (interval <= 10000))
                {
                    Properties.Settings.Default.IntervalTime = interval;
                }
                else
                {
                    lbl_error.Text = "Error: Interval value must be between 10ms and 10,000ms";
                    saveValid = false;
                }
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

        private void savePanguDirectory()
        {
            String dirText = txt_panguPath.Text;

            try
            {
                Properties.Settings.Default.panguDirectory = dirText;
            }
            catch
            {
                lbl_error.Text = "Error: Not a valid PANGU Directory";
                saveValid = false;
            }
        }

        private void saveEnvironementDirectory()
        {
            String dirText = txt_environmentPath.Text;

            try
            {
                Properties.Settings.Default.panDirectory = dirText;
            }
            catch
            {
                lbl_error.Text = "Error: Not a valid Environment Directory";
                saveValid = false;
            }
        }


        private void btn_worldBrowse_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            txt_environmentPath.Text = dialog.SelectedPath;
        }

        private void btn_panguBrowse_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            txt_panguPath.Text = dialog.SelectedPath;
        }
 
	}
}