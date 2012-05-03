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

namespace EightPuzzle_GazeAgumented
{
    /// <summary>
    /// Interaction logic for ThankYou.xaml
    /// </summary>
    public partial class ThankYou : Window
    {
        public ThankYou()
        {
            InitializeComponent();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Application.Current.Shutdown();
            }
            else if (e.Key == Key.PageUp)
            {
                Window puzzle3 = new Puzzle3();
                puzzle3.Show();
                this.Close();
            }
        }
    }
}
