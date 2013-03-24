/*      LogManager Class
 *	    AUTHOR: STEWART TAYLOR
 *------------------------------------
 * This class is used to log information by the system
 * It will print out all the information to PathPlannerLog.txt
 * This class can also be used to provide information to a output console
 *
 * Last Updated: 24/03/2013
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.IO;

namespace DynamicPathPlanner.Code
{
    class LogManager
    {

        private String filepath = "Log.txt";
        private TextBox textConsole;
        private List<String> entries = new List<String>();

        public List<String> getEntries()
        {
            return entries;
        }


        public LogManager(TextBox tBox)
        {
            textConsole = tBox;
        }

        public void clearSimulationLog()
        {
            entries.Clear();
        }

        public void addEntry(String entryText)
        {
            entries.Add(entryText);
            printToLogFile(entryText);
            printToConsole(entryText);
        }

        private void printToLogFile(String entry)
        {
            using (StreamWriter w = File.AppendText(filepath))
            {
                w.WriteLine(entry);
            }
        }

        public  void printToConsole(String entry)
        {
         //  textConsole.Text += '\r' + entry ;
        }

    }
}
