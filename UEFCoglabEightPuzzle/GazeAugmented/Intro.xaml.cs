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
using System.Windows.Shapes;

namespace EightPuzzle_GazeAugmented
{
    /// <summary>
    /// Interaction logic for Intro.xaml
    /// </summary>
    public partial class Intro : Window
    {
        public Intro()
        {
            InitializeComponent();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //Check which key was typed
            if (e.Key == Key.Escape)
            {
                //exit the application
                Application.Current.Shutdown();
            }
            else
            {
                //open the think aloud window
                Window thinkAloud = new ThinkAloud();
                thinkAloud.Show();
                this.Close();
            }
        }
    }
}
