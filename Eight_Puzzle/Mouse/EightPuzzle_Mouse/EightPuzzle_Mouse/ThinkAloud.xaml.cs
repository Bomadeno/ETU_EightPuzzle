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

namespace EightPuzzle_Mouse
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ThinkAloud : Window
    {
        public ThinkAloud()
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
            else if (e.Key == Key.PageDown)
            {
                //move to the next screen
                Window preTrial = new PreTrail();
                preTrial.Show();
                this.Close();
            }
        }
    }
}
