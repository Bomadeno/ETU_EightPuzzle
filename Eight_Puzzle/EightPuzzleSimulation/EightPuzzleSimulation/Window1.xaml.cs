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

namespace EightPuzzleSimulation
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Exit the Application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();  //Shut down the application
        }

        /// <summary>
        /// Open the puzzle using MOUSE interaction
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMPuzzle_Click(object sender, RoutedEventArgs e)
        {
            Window mousePuzzle = new MousePuzzle();     //Open the mouse puzzle window
            mousePuzzle.Show();
        }

        /// <summary>
        /// Open Gaze Augmented Puzzle window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGAPuzzle_Click(object sender, RoutedEventArgs e)
        {
            Window gazeAugmentedPuzzle = new GazeAugmentedPuzzle();   //Open the gaze-agumented window
            gazeAugmentedPuzzle.Show();
        }

        /// <summary>
        /// Open the dwell time puzzle window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDTPuzzle_Click(object sender, RoutedEventArgs e)
        {
            Window dwellTimePuzzle = new DwellTimePuzzle();     //Open the dwell-time window
            dwellTimePuzzle.Show();
        }
    }
}
